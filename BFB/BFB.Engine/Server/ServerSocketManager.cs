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
    /// <summary>
    /// Manages TCP server
    /// </summary>
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
        
        /// <summary>
        /// Callback that is called when a client tries to authenticate
        /// </summary>
        public Func<DataMessage,bool> OnClientAuthentication { get; set; }
        
        /// <summary>
        /// Callback that is called when a client tries to connect
        /// </summary>
        public Action<ClientSocket> OnClientConnect { get; set; }
        
        /// <summary>
        /// Callback that is called when a client is ready
        /// </summary>
        public Action<ClientSocket> OnClientReady { get; set; }
        
        /// <summary>
        /// Called when a client disconnects
        /// </summary>
        public Action<string> OnClientDisconnect { get; set; }
        
        /// <summary>
        /// Called when the server is started
        /// </summary>
        public Action OnServerStart { get; set; }
        
        /// <summary>
        /// Called when the server is stopped
        /// </summary>
        public Action OnServerStop { get; set; }

        #endregion
        
        #endregion
        
        #region Constructor
        
        /// <summary>
        /// Constructs a Socket Manager that accepts, routes and handles a tcp connection
        /// </summary>
        /// <param name="ip">The ip to bind to</param>
        /// <param name="port">The port to bind to</param>
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

        /// <summary>
        /// Starts the server
        /// </summary>
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

        /// <summary>
        /// Stops the server
        /// </summary>
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

        /// <summary>
        /// Sends a DataMessage to all ocnnected clients
        /// </summary>
        /// <param name="routeKey">The route to send too</param>
        /// <param name="dataMessage">The message to send</param>
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

        /// <summary>
        /// Used to define routes from any connected client. 
        /// </summary>
        /// <param name="routeKey">The route to listen for</param>
        /// <param name="messageHandler">The callback to use for processing any received messaged</param>
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
        
        /// <summary>
        /// Gets a connected client with a given key
        /// </summary>
        /// <param name="clientKey">The clients key</param>
        /// <returns>A clientSocket object</returns>
        public ClientSocket GetClient(string clientKey)
        {
            lock (_lock)
            {
                return _clientSockets.ContainsKey(clientKey) ? _clientSockets[clientKey] : null;
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

        /// <summary>
        /// Used define what the terminal header should look like when printing to the console
        /// </summary>
        /// <param name="action"></param>
        public void SetTerminalHeader(Action action)
        {
            lock(_consoleLock){
                _terminalHeader = action;
            }
        }
        
        #endregion

        #region PrintMessage

        /// <summary>
        /// Prints a message with terminal header to the console
        /// </summary>
        /// <param name="message">Message to print</param>
        /// <param name="printHeader">Indicate whether the header should be printed</param>
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

        /// <summary>
        /// Disposes the server
        /// </summary>
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