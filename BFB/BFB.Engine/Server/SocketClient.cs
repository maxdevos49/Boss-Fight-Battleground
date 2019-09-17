using System;
using System.Net.Sockets;
using System.Numerics;
using System.Threading;

namespace BFB.Engine.Server
{
    public class SocketClient
    {
        private readonly object _lock = new object();

        private Vector2 position;
        private Vector2 mousePosition;
        private readonly TcpClient Client;

        public SocketClient()
        {
            Client = new TcpClient("127.0.0.1", 6969);

            position = Vector2.Zero;
            mousePosition = Vector2.Zero;

            //Start recieving thread
            Thread t = new Thread(RecieveThread);
            t.Start();
        }

        public void SendData(Packet data)
        {

        }

        public void RecieveThread()
        {
            NetworkStream stream = Client.GetStream();

            Packet message;
            byte[] packetSize = new byte[4];
            int i, messageSize;

            try
            {

                /**
                 * Read returns number of bytes to read so if nothing was read then looping
                 * stops. If still connected I think this just waits until there is input.
                 * Aka it only returns 0 if its disconnected
                 * */
                while ((i = stream.Read(packetSize, 0, 4)) != 0)
                {
                    //get message size and create input array size
                    messageSize = BitConverter.ToInt32(packetSize);

                    //Deserializes a packet from the stream
                    message = PacketManager.Read(stream, messageSize);

                    //from here we can use the message from the client
                    Console.WriteLine($"Message Route: {message.Route}");

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: {0}", ex);
            }

            Client.Close();
        }


    }
}
