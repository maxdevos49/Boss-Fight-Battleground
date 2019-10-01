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
        
        private Func<DataMessage,DataMessage> _onAuthentication;
        private Action<DataMessage> _onConnect;
        private Action<DataMessage> _onDisconnect;
        private Action _onReady;
        
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
            
            _onConnect = (m) =>  ClientId = m.Message;
            _onDisconnect = (m) => ClientId = null;
            _onAuthentication = (m) => null;
            _onReady = () => { };
            
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
            if (!_allowEmit)
            {
                Console.WriteLine("Emits are not yet enabled. Server must make first contact");
                return;
            }

            if(message == null)
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
        
        #region OnConnect

        public void OnConnect(Action<DataMessage> handler)
        {
            _onConnect = handler;
        }
        
        #endregion
        
        #region OnDisconnect

        public void OnDisconnect(Action<DataMessage> handler)
        {
            _onDisconnect = handler;
        }
        
        #endregion
        
        #region OnAuthentication

        public void OnAuthentication(Func<DataMessage,DataMessage> handler)
        {
            _onAuthentication = handler;
        }
        
        #endregion
        
        #region OnReady

        public void OnReady(Action handler)
        {
            _onReady = handler;
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

                    if(messageData.Length == 0) continue;
                    
                    //Get full message
                    DataMessage message;
                    using (MemoryStream memoryStream = new MemoryStream(messageData))
                    {
                        message =  (DataMessage)new BinaryFormatter().Deserialize(memoryStream);
                    }
                    
                    lock (_lock)
                    {
                        //Check reserved routes firsts
                        switch (message.Route)
                        {
                            case "connect":
                                //assign client id
                                ClientId = message.ClientId;
                                _allowEmit = true;
                                _onConnect(message);
                                break;
                            case "authentication":
                            {
                                DataMessage authMessage = _onAuthentication(message);
                                Emit("authentication", message);
                                break;
                            }
                            case "ready":
                                _acceptData = true;
                                _onReady();
                                break;
                            case "disconnect":
                                _onDisconnect(message);
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
                Console.WriteLine("Exception: {0}", ex);
                _socket.Dispose();
            }
            
        }
        
        #endregion
        
    }
}