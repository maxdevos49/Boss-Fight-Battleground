using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using BFB.Engine.Server.Communication;

namespace BFB.Engine.Server.Socket
{
    public class ClientSocket
    {
        
        #region Properties
        
        /**
         * Indicates the unique Identifier for the client
         */
        public string Key { get; }

        /**
         * Contains the client socket
         */
        private readonly TcpClient _socket;
        
        /**
         * The read/write stream
         */
        private readonly NetworkStream _stream;
        
        #endregion
        
        #region Constructor
        
        /**
         * Basically represents a socket client
         */
        public ClientSocket(string key, TcpClient socket)
        {
            Key = key;
            _socket = socket;
            _stream = _socket.GetStream();
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
        
        #region Read

        public DataMessage Read()
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

                    using (MemoryStream memoryStream = new MemoryStream(messageData))
                       return (DataMessage)new BinaryFormatter().Deserialize(memoryStream);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: {0}", ex);
                _socket.Dispose();
            }

            return null;
        }
        
        #endregion
        
        #region PendingData

        public bool PendingData()
        {
            return _stream.DataAvailable;
        }
        
        #endregion
        
        #region IsConnected

        public bool IsConnected()
        {
            if (!_socket.Client.Poll(0, SelectMode.SelectRead)) return true;
            
            byte[] buff = new byte[1];
            
            return _socket.Client.Receive( buff, SocketFlags.Peek ) != 0;
        }
        
        #endregion
        
        #region Disconnect

        public void Disconnect(string reason)
        {
            Emit("disconnect", new DataMessage{Message = reason});
            
            _stream.Dispose();
            _socket.Dispose();
        }
        
        #endregion
        
    }
}