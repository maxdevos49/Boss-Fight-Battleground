using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using BFB.Engine.Server.Communication;
using JetBrains.Annotations;

namespace BFB.Engine.Server
{
    public class ServerSocketManager
    {

        #region Properties
        
        private readonly object _lock;
        private readonly object _consoleLock;
        
        private readonly TcpListener _listener;
        private readonly Dictionary<string, ClientSocket> _clientSockets;
        private readonly Dictionary<string,List<Action<DataMessage>>> _messageHandlers;
        private Action _terminalHeader;
        private bool _isBroadcasting;
        
        #region Server Event Properties
        public Func<DataMessage,bool> OnClientAuthentication { get; set; }
        public Action<ClientSocket> OnClientConnect { get; set; }
        public Action<ClientSocket> OnClientReady { get; set; }
        
        /// <summary>
        /// Called before the OnClientReady callback. Use this to send any setup information to the client.
        /// </summary>
        public Action<ClientSocket> OnClientPrepare { get; set; }
        public Action<string> OnClientDisconnect { get; set; }
        public Action OnServerStart { get; set; }
        public Action OnServerStop { get; set; }

        #endregion
        
        #endregion
        
        #region Constructor
        
        /**
         * Constructs a server manager for accepting and routing tcp clients
         */
        public ServerSocketManager(IPAddress ip, int port)
        {
            _listener = new TcpListener(ip, port);

            _lock = new object();
            _consoleLock = new object();
            _clientSockets = new Dictionary<string, ClientSocket>();
            _messageHandlers = new Dictionary<string, List<Action<DataMessage>>>();
            _isBroadcasting = false;
            
           _terminalHeader = () => Console.Write($"[Server|{DateTime.Now:h:mm:ss tt}] ");
           
           OnClientAuthentication = m => true;
        }
        
        #endregion

        #region Start

        public void Start()
        {
            if (_isBroadcasting) return;
            
            OnServerStart?.Invoke();
            
            _listener.Start();
            _isBroadcasting = true;

            //Start connection thread
            Thread t1 = new Thread(HandleConnection)
            {
                IsBackground = true,
                Name = "Handle Connections"
            };

            //Start read thread
            Thread t2 = new Thread(Read)
            {
                IsBackground = true,
                Name = "Read Clients"
            };
            
            t1.Start();
            t2.Start();
            
        }

        #endregion

        #region Stop

        public void Stop()
        {
            if (!_isBroadcasting) return;
            
            OnServerStop?.Invoke();

            lock (_lock)
            {
                //Disconnect all clients
                foreach ((string _, ClientSocket socket) in _clientSockets.ToList())
                    socket.Disconnect();
                _clientSockets.Clear();
            }
            
            Dispose();
        }

        #endregion

        #region Emit

        /**
         * Sends a message to all connected clients
         */
        public void Emit(string routeKey, DataMessage dataMessage = null)
        {
            if(dataMessage == null)
                dataMessage = new DataMessage();
            
            lock (_lock)
            {
                foreach (KeyValuePair<string, ClientSocket> client in _clientSockets)
                {
                    client.Value.Emit(routeKey, dataMessage);
                }
            }
        }

        #endregion
        
        #region On

        /**
         * Used to listen for messages from the client
         */

        [UsedImplicitly]
        public void On(string routeKey, Action<DataMessage> messageHandler)
        {
            lock (_lock)
            {
                if (_messageHandlers.ContainsKey(routeKey))
                {
                    _messageHandlers[routeKey].Add(messageHandler);
                }
                else
                {
                    _messageHandlers.Add(routeKey, new List<Action<DataMessage>> {messageHandler});
                }
            }
        }

        #endregion

        #region Read

        private void Read()
        {
            try
            {
                while (_isBroadcasting)
                {
                    lock (_lock)
                    {

                        foreach ((string _, ClientSocket socket) in _clientSockets.Where(s => s.Value.IsConnected()))
                        {
                            if (!socket.PendingData()) continue;

                            DataMessage message = socket.Read();

                            if (message.Route == "authentication")
                            {
                                if (!OnClientAuthentication?.Invoke(message) ?? false)
                                {
                                    socket.Disconnect();
                                }
                                else
                                {
                                    OnClientPrepare?.Invoke(socket);
                                    OnClientReady?.Invoke(socket);
                                    socket.Emit("ready");
                                }

                            }
                            else
                            {
                                //Check socket specific handlers
                                socket.ProcessHandler(message);

                                //check global handlers
                                if (!_messageHandlers.ContainsKey(message.Route)) continue;
                                foreach (Action<DataMessage> handler in _messageHandlers[message.Route])
                                    handler(message);
                            }

                        }
                    }

                    //check reading 200x a second
                    Thread.Sleep(1000/200);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Server Read Exception: {0}", ex);
            }
        }

        #endregion
        
        #region GetClient

        public ClientSocket GetClient(string clientId)
        {
            lock (_lock)
            {
                return _clientSockets.ContainsKey(clientId) ? _clientSockets[clientId] : null;
            }
        }
        
        #endregion

        #region HandleConnection

        private void HandleConnection()
        {
            try
            {
                while (true)
                {
                    //Check status of current connections
                    lock (_lock)
                    {
                        foreach ((string key, ClientSocket socket) in _clientSockets.ToList().Where(x => !x.Value.IsConnected()))
                        {
                            socket.Disconnect();
                            _clientSockets.Remove(key);
                            OnClientDisconnect?.Invoke(socket.ClientId);
                        }
                    }
                    
                    //Check for new connections
                    if (_listener.Pending())
                    {
                        //Accept new client
                        ClientSocket newSocket = new ClientSocket(Guid.NewGuid().ToString(), _listener.AcceptTcpClient());
                        
                        //Add new client to list
                        lock (_lock)
                        {
                            _clientSockets.Add(newSocket.ClientId, newSocket);
                        }
                        
                        OnClientConnect?.Invoke(newSocket);
                        
                        //Fire client connection and authentication events
                        newSocket.Emit("connect");
                        newSocket.Emit("authentication");
                        
                    }
                    else
                    {
                        //Waits half a second if no pending connections to preserve performance of machine
                        Thread.Sleep(500);
                    }
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
                Stop();
            }
        }

        #endregion

        #region SetTerminalHeader

        public void SetTerminalHeader(Action action)
        {
            lock(_consoleLock){
                _terminalHeader = action;
            }
        }
        
        #endregion

        #region PrintMessage

        public void PrintMessage(string message = null, bool printHeader = true)
        {
            lock (_consoleLock)
            {
                if (message != null)
                    Console.WriteLine(message);

                if (!printHeader) return;

                _terminalHeader();
            }
        }
        
        #endregion
        
        #region Dispose

        [UsedImplicitly]
        public void Dispose()
        {
            lock (_lock)
            {
                _listener.Stop();
                _clientSockets.Clear();
                _messageHandlers.Clear();
                _isBroadcasting = false;
            }
        }
        
        #endregion
        
    }
}