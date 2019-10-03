//C#

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using BFB.Engine.Server.Communication;
using JetBrains.Annotations;
//Jetbrains

//engine

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
        public Func<DataMessage,bool> OnClientAuthentication { get; set; }
        public Action<ClientSocket> OnClientConnect { get; set; }
        public Action<ClientSocket> OnClientDisconnect { get; set; }

        private bool _isBroadcasting;
        
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
           
           OnClientConnect = null;
           OnClientDisconnect = null;
           OnClientAuthentication = m => true;

        }
        
        #endregion

        #region Start

        public void Start()
        {
            if (_isBroadcasting) return;
            
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

        public void Stop(string reason = "Server is Shutting Down")
        {
            if (!_isBroadcasting) return;
            
            lock (_lock)
            {
                foreach ((string key, ClientSocket socket) in _clientSockets.ToList())
                {
                    
                    socket.Disconnect(reason);
                    PrintMessage($"Client {key} Disconnected");
                    
                    //remove forced disconnect
                    _clientSockets.Remove(key);
                }
                
                _listener.Stop();
                _isBroadcasting = false;
            }
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
            while (_isBroadcasting)
            {
                lock(_lock)
                {
                    
                    foreach ((string _, ClientSocket socket) in _clientSockets)
                    {
                        if (!socket.PendingData()) continue;
                        
                        DataMessage message = socket.Read();

                        if (message.Route == "authentication")
                        {
                            if (OnClientAuthentication?.Invoke(message) ?? true)
                            {
                                socket.Disconnect("Authentication Failed");
                            }
                            else
                            {
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
                
                //Read only 100 times a second
                Thread.Sleep(1000/100);
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
                        foreach ((string key, ClientSocket socket) in _clientSockets.ToList())
                        {
                            if (socket.IsConnected()) continue;

                            OnClientDisconnect?.Invoke(socket);
                            _clientSockets.Remove(key);
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
                        
                        //Fire on connection event
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
        
    }
}