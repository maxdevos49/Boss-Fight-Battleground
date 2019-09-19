using System;
using System.Net.Sockets;
using System.Numerics;
using System.Threading;

namespace BFB.Engine.Server
{
    public class SocketClient
    {
        private readonly object _lock = new object();

        private Vector2 _position;
        private Vector2 _mousePosition;
        private readonly TcpClient _client;

        public SocketClient()
        {
            _client = new TcpClient("127.0.0.1", 6969);

            _position = Vector2.Zero;
            _mousePosition = Vector2.Zero;

            //Start receiving thread
            Thread t = new Thread(ReceiveThread);
            t.Start();
        }

        public void SendData(Packet data)
        {

        }

        private void ReceiveThread()
        {
            NetworkStream stream = _client.GetStream();

            byte[] packetSize = new byte[4];

            try
            {

                /**
                 * Read returns number of bytes to read so if nothing was read then looping
                 * stops. If still connected I think this just waits until there is input.
                 * Aka it only returns 0 if its disconnected
                 * */
                while (stream.Read(packetSize, 0, 4) != 0)
                {
                    //get message size and create input array size
                    int messageSize = BitConverter.ToInt32(packetSize);

                    //Deserializes a packet from the stream
                    Packet message = PacketManager.Read(stream, messageSize);

                    //from here we can use the message from the client
                    Console.WriteLine($"Message Route: {message.Route}");

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: {0}", ex);
            }

            _client.Close();
        }


    }
}
