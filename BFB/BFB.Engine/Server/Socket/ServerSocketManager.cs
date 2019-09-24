//C#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

//Jetbrains
using JetBrains.Annotations;

//engine
using BFB.Engine.Server.Communication;

namespace BFB.Engine.Server.Socket
{
    public class ServerSocketManager
    {

        #region Properties
        
        /**
         * Thread Lock property
         */
        private readonly object _lock;

        /**
         * Tcp listener
         */
        private readonly TcpListener _listener;

        /**
         * Represents all of the connected tcp clients. Clients are
         * given a randomly generated client key upon connection.
         */
        private readonly Dictionary<string, ClientSocket> _clientSockets;
        
        /**
         * Holds all of the server tcp message handlers from the server
         */
        private readonly Dictionary<string,List<Action<DataMessage>>> _messageHandlers;

        /**
         * Holds a lambda function detailing the server terminal header
         */
        private Action _terminalHeader;
        
        /**
         * Holds a lambda function detailing how to authenticate a client
         */
        private Func<ClientSocket,bool> _onClientAuthentication;
        
        /**
         * Holds a lambda function detailing what should happen when a client connects
         */
        private Action<ClientSocket> _onClientConnect;
        
        /**
         * Holds a lambda function detailing what should happen when a client disconnects
         */
        private Action<ClientSocket> _onClientDisconnect;

        /**
         * Indicates whether the server is running or not
         */
        [UsedImplicitly]
        public bool IsBroadcasting { get; private set; }
        
        #endregion
        
        #region Constructor
        
        /**
         * Constructs a server manager for accepting and routing tcp clients
         */
        public ServerSocketManager(IPAddress ip, int port)
        {
            _listener = new TcpListener(ip, port);

            _lock = new object();
            _clientSockets = new Dictionary<string, ClientSocket>();
            _messageHandlers = new Dictionary<string, List<Action<DataMessage>>>();
            IsBroadcasting = false;
            
           _terminalHeader = () => Console.Write($"[Server|{DateTime.Now:h:mm:ss tt}] ");
           
            //Default client connect Handlers
           _onClientConnect = socketClient =>
           {
               PrintMessage($"Client {socketClient.Key} Connected");
           };
           
           //Default client disconnect handler
           _onClientDisconnect = socketClient =>
           {
               PrintMessage($"Client {socketClient.Key} Disconnected");
               socketClient.Disconnect("Connection Lost");
           };
           
           //Default client authentication handler
           _onClientAuthentication = socketClient =>
           {
               PrintMessage($"Client {socketClient.Key} Authenticated with Standard Validation(No Validation)");
               return true;
           };

        }
        
        #endregion

        #region Start

        public void Start()
        {
            if (IsBroadcasting) return;
            
            _listener.Start();
            IsBroadcasting = true;

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

        public void Stop(string reason = "Server is Shutting Down")
        {
            if (!IsBroadcasting) return;
            
            lock (_lock)
            {
                foreach ((string key, ClientSocket socket) in _clientSockets.ToList())
                {
                    
                    socket.Disconnect(reason);
                    PrintMessage($"Client with Id: {key} Disconnected");
                    
                    //remove forced disconnect
                    _clientSockets.Remove(key);
                }
                
                _listener.Stop();
                IsBroadcasting = false;
            }
        }

        #endregion

        #region Emit

        /**
         * Sends a message to all connected clients
         */
        public void Emit(string routeKey, DataMessage dataMessage)
        {
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
            while (IsBroadcasting)
            {
                lock(_lock)
                {
                    foreach ((string _, ClientSocket socket) in _clientSockets)
                    {
                        if (!socket.PendingData()) continue;
                        
                        DataMessage data = socket.Read();

                        if (!_messageHandlers.ContainsKey(data.Route)) continue;
                        
                        foreach (Action<DataMessage> handler in _messageHandlers[data.Route])
                        {
                            handler(data);
                        }

                    }
                }
                
                //Read only 100 times a second
                Thread.Sleep(1000/100);
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
                        foreach ((string key, ClientSocket socket) in _clientSockets.ToList())
                        {
                            if (socket.IsConnected()) continue;

                            _onClientDisconnect(socket);
                            _clientSockets.Remove(key);
                        }
                    }
                    
                    //Check for new connections
                    if (_listener.Pending())
                    {
                        ClientSocket newSocket = new ClientSocket(Guid.NewGuid().ToString(), _listener.AcceptTcpClient());
                        
                        lock (_lock)
                        {
                            
                            if (_onClientAuthentication(newSocket))
                            {
                                _clientSockets.Add(newSocket.Key, newSocket);
                                _onClientConnect(newSocket);
                            }
                            else
                            {
                                newSocket.Disconnect("Authentication Failed");
                                _onClientDisconnect(newSocket);
                            }
                        }
                        
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
            _terminalHeader = action;
        }
        
        #endregion

        #region OnClientAuthentication

        public void OnClientAuthentication(Func<ClientSocket,bool> handler)
        {
            _onClientAuthentication= handler;
        }

        #endregion
        
        #region OnClientConnect

        public void OnClientConnect(Action<ClientSocket> handler)
        {
            _onClientConnect= handler;
        }

        #endregion
        
        #region OnClientDisconnect

        public void OnClientDisconnect(Action<ClientSocket> handler)
        {
            _onClientDisconnect= handler;
        }

        #endregion
        
        #region PrintMessage

        public void PrintMessage(string message = null, bool printHeader = true)
        {
            if (message != null)
                Console.WriteLine(message);

            if (!printHeader) return;
            
            _terminalHeader();
        }
        
        #endregion
        
    }
}