using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using BFB.Engine.Server.Communication;
using JetBrains.Annotations;

namespace BFB.Engine.Server
{
    public class ClientSocketManager
    {
        
        #region Properties

        private readonly object _lock;
        private readonly string _ip;
        private readonly int _port;
        private Dictionary<string, List<Action<DataMessage>>> _handlers;

        private TcpClient _socket;
        private NetworkStream _stream;
        
        public Func<DataMessage,DataMessage> OnAuthentication { get; set; }
        public Action<DataMessage> OnConnect { get; set; }
        public Action<DataMessage> OnDisconnect { get; set; }
        public Action OnReady { get; set; }
        
        private bool _acceptData;
        private bool _allowEmit;

        [UsedImplicitly]
        public string ClientId { get;  set; }
        
        #endregion

        #region Constructor
        
        public ClientSocketManager(string ip, int port)
        {
            _lock = new object();
            _ip = ip;
            _port = port;
            _socket = null;
            _stream = null;
            _handlers = new Dictionary<string, List<Action<DataMessage>>>();
            
            _acceptData = false;
            _allowEmit = false;
            
            OnConnect = null;
            OnDisconnect = null;
            OnAuthentication = null;
            OnReady = null;
            
        }
        
        #endregion
        
        #region Disconnect

        public void Disconnect()
        {
            _stream.Dispose();
            _socket.Dispose();
        }
        
        #endregion
        
        #region Dispose

        public void Dispose()
        {
            _allowEmit = false;
            _acceptData = false;
            Thread.Sleep(100);
            Disconnect();
            _handlers = new Dictionary<string, List<Action<DataMessage>>>();
            _socket = null;
            _stream = null;
        }
        
        #endregion
        
        #region Connect

        /**
         * Connects to the server. Returns true if success
         */
        public bool Connect()
        {
            try
            {
                _socket = new TcpClient(_ip,_port);
                _stream = _socket.GetStream();
                
                //Setup the read thread
                Thread t = new Thread(Read)
                {
                    IsBackground = true,
                    Name = "ClientReadThread"
                };
                
                t.Start();
                
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        #endregion

        #region Emit
        
        public void Emit(string routeKey, DataMessage message = null)
        {
            try
            {
                if (!_allowEmit)
                {
                    Console.WriteLine("Emits are not enabled. Server must allow them.");
                    return;
                }

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
                Console.WriteLine("Write Exception: {0}", ex);
                Disconnect();
            }
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
                    #region Deserialize Message Size
                    
                    int messageSize = BitConverter.ToInt32(packetSize);
                    if (messageSize <= 3) return;
                    byte[] messageData = new byte[messageSize];
                    
                    #endregion

                    #region Read Message
                    
                    int bytesRead = 0;
                    do
                    {
                        bytesRead += _stream.Read(messageData, bytesRead, messageSize - bytesRead);
                    } while (bytesRead < messageSize);

                    #endregion
                    
                    #region Deserialize Message
                    
                    DataMessage message;
                    using (MemoryStream memoryStream = new MemoryStream(messageData))
                    {
                        message =  (DataMessage)new BinaryFormatter().Deserialize(memoryStream);
                    }
                    
                    #endregion
                    
                    lock (_lock)
                    {
                        //Check reserved routes firsts
                        switch (message.Route)
                        {
                            case "connect":
                                //assign client id
                                ClientId = message.ClientId;
                                _allowEmit = true;
                                OnConnect?.Invoke(message);
                                break;
                            case "authentication":
                            {
                                Emit("authentication", OnAuthentication(message));
                                break;
                            }
                            case "ready":
                                _acceptData = true;
                                OnReady?.Invoke();
                                break;
                            case "disconnect":
                                OnDisconnect?.Invoke(message);
                                Disconnect();
                                break;
                            default:
                            {
                                
                                if (!_handlers.ContainsKey(message.Route) && !_acceptData) continue;
                        
                                foreach (Action<DataMessage> handler in _handlers[message.Route])
                                {
                                    handler(message);
                                }

                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Read Exception: {0}", ex);
            }
            Disconnect();
        }
        
        #endregion
        
    }
}