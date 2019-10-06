using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using BFB.Engine.Server.Communication;

namespace BFB.Engine.Server
{
    public class ClientSocket
    {
        
        #region Properties
        public string ClientId { get; }
        
        private readonly object _lock;
        private readonly TcpClient _socket;
        private readonly NetworkStream _stream;
        private readonly Dictionary<string, List<Action<DataMessage>>> _handlers;
        
        #endregion
        
        #region Constructor

        /**
         * Basically represents a socket client
         */
        public ClientSocket(string clientId, TcpClient socket)
        {
            ClientId = clientId;
            _lock = new object();
            _handlers = new Dictionary<string, List<Action<DataMessage>>>();
            _socket = socket;
            _stream = _socket.GetStream();
        }
        
        #endregion

        #region Emit
        
        public void Emit(string routeKey, DataMessage message = null)
        {
            if (!IsConnected()) return;
            
            try
            {
                if (message == null)
                    message = new DataMessage();

                //Assign message to route
                message.Route = routeKey;
                message.ClientId = ClientId;

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
            catch (Exception ex)
            {
                Console.WriteLine("Write Exception {0}", ex);
                Disconnect();
            }
            
        }
        
        #endregion
        
        #region On

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
                    _handlers.Add(routeKey, new List<Action<DataMessage>>{ handler });
                }
            }
        }
        
        #endregion
        
        #region Read

        public DataMessage Read()
        {
            if (!IsConnected()) return null;
            
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
                    
                    using (MemoryStream memoryStream = new MemoryStream(messageData))
                       return (DataMessage)new BinaryFormatter().Deserialize(memoryStream);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Client Read Exception: {0}", ex);
                Disconnect();
            }
            
            Console.WriteLine("Read Error");
            return null;
        }
        
        #endregion
        
        #region ProcessHandler

        public void ProcessHandler(DataMessage message)
        {
            lock (_lock)
            {
                if (!_handlers.ContainsKey(message.Route)) return;
                foreach (Action<DataMessage> handler in _handlers[message.Route])
                    handler(message);
            }
        }
        
        #endregion
        
        #region PendingData

        public bool PendingData()
        {
            try
            {
                return _stream.DataAvailable;
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        #endregion
        
        #region IsConnected

        public bool IsConnected()
        {
            try
            {
                if (!_socket.Client.Poll(0, SelectMode.SelectRead)) return true;

                byte[] buff = new byte[1];

                return _socket.Client.Receive(buff, SocketFlags.Peek) != 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        #endregion
        
        #region Disconnect

        public void Disconnect()
        {
            lock(_lock){
                _stream.Dispose();
                _socket.Dispose();
                _handlers.Clear();
            }
        }
        
        #endregion
        
    }
}