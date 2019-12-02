using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
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
        private TcpClient _socket;
        private NetworkStream _stream;
        private bool _acceptData;
        private bool _allowEmit;
        private Stopwatch _timer;
        private long _previousHeartBeat;
        private long _heartBeatLength;
        private readonly Dictionary<string, List<Action<DataMessage>>> _handlers;

        /// <summary>
        /// Indicates the connected clients Id
        /// </summary>
        public string ClientId { get;  private set; }
        
        /// <summary>
        /// The ip used to connect to the server
        /// </summary>
        public string Ip { get; set; }
        
        /// <summary>
        /// The port used to connect to the server
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// The ticks per second from the server
        /// </summary>
        public double Tps => System.Math.Round(1000 * (1f / _heartBeatLength));
        
        /// <summary>
        /// Callback called when the server asks the client for authentication
        /// </summary>
        public Func<DataMessage,DataMessage> OnAuthentication { get; set; }
        
        /// <summary>
        /// Callback called when the server has recognized the connection
        /// </summary>
        public Action<string> OnConnect { get; set; }
        
        /// <summary>
        /// Callback called when the client has disconnected
        /// </summary>
        public Action<string> OnDisconnect { get; set; }
        
        public Action<DataMessage> OnPrepare { get; set; }
        
        /// <summary>
        /// Callback called when the client is ready told by the server
        /// </summary>
        public Action OnReady { get; set; }
        
        #endregion

        #region Constructor
        
        /// <summary>
        /// Constructs a client socket manager
        /// </summary>
        /// <param name="ip">Ip used to connect</param>
        /// <param name="port">Port used to connect</param>
        public ClientSocketManager(string ip = "127.0.0.1", int port = 6969)
        {
            _lock = new object();
            Ip = ip;
            Port = port;
            _socket = null;
            _stream = null;
            _handlers = new Dictionary<string, List<Action<DataMessage>>>();
            ClientId = null;
            _previousHeartBeat = 0;
            _heartBeatLength = 0;
            
            _acceptData = false;
            _allowEmit = false;
            
            OnConnect = null;
            OnDisconnect = null;
            OnAuthentication = null;
            OnReady = null;
        }
        
        #endregion
        
        #region Disconnect

        /// <summary>
        /// Used to disconnect from the server or to clean up after being told to connect
        /// </summary>
        /// <param name="reason"></param>
        public void Disconnect(string reason)
        {
            OnDisconnect?.Invoke(reason);
            Dispose();
        }
        
        #endregion
        
        #region Dispose

        /// <summary>
        /// Disposes the client socket manager and clears all properties to there initial state
        /// </summary>
        [UsedImplicitly]
        public void Dispose()
        {
            _stream?.Dispose();
            _stream = null;
            _socket?.Dispose();
            _socket = null;
            _handlers?.Clear();
            _acceptData = false;
            _allowEmit = false;
        }
        
        #endregion
        
        #region Connect

        /// <summary>
        /// Connects to a server
        /// </summary>
        /// <returns>True if connection was a success</returns>
        public bool Connect()
        {
            try
            {
                _socket = new TcpClient(Ip,Port);
                _stream = _socket.GetStream();
                
                //Setup the read thread
                Thread t = new Thread(Read)
                {
                    IsBackground = true,
                    Name = "Client Read Thread"
                };
                
                t.Start();
                
                _previousHeartBeat = 0;
                _heartBeatLength = 0;
                _timer = Stopwatch.StartNew();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        #endregion

        #region Emit
        
        /// <summary>
        /// Used to send messages to the server
        /// </summary>
        /// <param name="routeKey">The route to send to</param>
        /// <param name="message">The data to send</param>
        public void Emit(string routeKey, DataMessage message = null)
        {
            try
            {
                if (!_allowEmit)
                {
                    Console.WriteLine("Emits are not enabled. Server must allow them after official connection.");
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
            catch (IOException) { /*If this happens we do not care*/ }
            catch (ObjectDisposedException) { /*If this happens we do not care*/ }
            catch (Exception ex)
            {
                Console.WriteLine("Write Exception: {0}", ex);
            }
           
        }
        
        #endregion
        
        #region On

        /// <summary>
        /// Used to listen for messages from the server.
        /// </summary>
        /// <param name="routeKey">The route to listen for</param>
        /// <param name="handler">The callback to process the message after receiving a message</param>
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
                        if (memoryStream.CanRead)
                        {
                            message = (DataMessage) new BinaryFormatter().Deserialize(memoryStream);
                        }
                        else
                        {
                            return;
                        }
                    }

                    #endregion

                    #region Distribute Messages

                    lock (_lock)
                    {
                        //Check reserved routes firsts
                        switch (message.Route)
                        {
                            case "connect":
                                //assign client id
                                ClientId = message.ClientId;
                                _allowEmit = true;
                                OnConnect?.Invoke(message.ClientId);
                                break;
                            case "authentication":
                                Emit("authentication", OnAuthentication?.Invoke(message));
                                break;
                            case "prepare":
                                OnPrepare?.Invoke(message);
                                break;
                            case "ready":
                                _acceptData = true;
                                OnReady?.Invoke();
                                break;
                            case "disconnect":
                                Disconnect("Server Requested Disconnect");
                                break;
                            case "HeartBeat":
                                _heartBeatLength = _timer.ElapsedMilliseconds - _previousHeartBeat;
                                _previousHeartBeat = _timer.ElapsedMilliseconds;
                                break;
                            default:
                            {
                                #region Distribute Messages to Routes

                                if (!_acceptData) continue;
                                
                                if(!_handlers.ContainsKey(message.Route)) continue;

                                foreach (Action<DataMessage> handler in _handlers[message.Route])
                                {
                                    handler(message);
                                }

                                break;

                                #endregion
                            }
                        }
                    }

                    #endregion
                }
            }
            catch (IOException)
            {
                Disconnect("Read Error");
            }
            catch (ObjectDisposedException) { /*If this happens we do not care*/ }
            catch (Exception ex)
            {
                Console.WriteLine("Write Exception: {0}", ex);
            }
            
        }
        
        #endregion
        
        #region EmitAllowed

        public bool EmitAllowed()
        {
            return _allowEmit;
        }
        
        #endregion
        
    }
}