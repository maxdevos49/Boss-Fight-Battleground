//C#
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

//Engine
using BFB.Engine.Server.Communication;

//Jetbrains
using JetBrains.Annotations;

namespace BFB.Engine.Server.Socket
{
    public class ClientSocketManager
    {
        
        #region Properties

        private readonly object _lock;
        private readonly TcpClient _socket;
        private readonly NetworkStream _stream;
        private readonly Dictionary<string, List<Action<DataMessage>>> _handlers;

        #endregion

        #region Constructor
        
        public ClientSocketManager(string ip, int port)
        {
            _lock = new object();
            _socket = new TcpClient(ip,port);
            _stream = _socket.GetStream();
            _handlers = new Dictionary<string, List<Action<DataMessage>>>();
            
            //Setup the read thread
            Thread t = new Thread(Read)
            {
                IsBackground = true,
                Name = "ClientReadThread"
            };
            t.Start();
        }
        
        #endregion
        
        #region Disconnect

        public void Disconnect()
        {
            _socket.Dispose();
        }
        
        #endregion
        
        #region Emit
        
        public void Emit(string routeKey, DataMessage message)
        {
            //Assign message to route
            message.Route = routeKey;
            
            byte[] messageData;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(memoryStream, message);
                messageData = memoryStream.ToArray();
            }

            //get and convert length to bytes
            byte[] messageLength = BitConverter.GetBytes(messageData.Length);

            //send message length
            _stream.Write(messageLength, 0, 4);

            //send message
            _stream.Write(messageData, 0, messageData.Length);
        }
        
        #endregion
        
        #region On

        [UsedImplicitly]
        public void On(string routeKey, Action<DataMessage> handler)
        {
            lock (_lock)
            {
                if (_handlers.ContainsKey(routeKey))
                {
                    _handlers[routeKey].Add(handler);
                }
                else
                {
                    _handlers.Add(routeKey, new List<Action<DataMessage>>{ handler} );
                }
            }
        }
        
        #endregion

        #region Read
        
        private void Read()
        {
            byte[] packetSize = new byte[4];

            try
            {

                while (_stream.Read(packetSize, 0, 4) != 0)
                {
                    int messageSize = BitConverter.ToInt32(packetSize);
                    byte[] messageData = new byte[messageSize];

                    //Safe read that will guarantee a full message
                    int bytesRead = 0;
                    do
                    {
                        bytesRead += _stream.Read(messageData, bytesRead, messageSize - bytesRead);
                    } while (bytesRead < messageSize);

                    //Get full message
                    DataMessage message;
                    using (MemoryStream memoryStream = new MemoryStream(messageData))
                    {
                        message =  (DataMessage)new BinaryFormatter().Deserialize(memoryStream);
                    }
                    
                    //Announce to handlers
                    if (!_handlers.ContainsKey(message.Route)) continue;
                    
                    lock (_lock)
                    {
                        foreach (Action<DataMessage> handler in _handlers[message.Route])
                        {
                            handler(message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: {0}", ex);
                _socket.Dispose();
            }
            
        }
        
        #endregion
        
    }
}