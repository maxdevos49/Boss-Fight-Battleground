//C#
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using BFB.Engine.Server;
using Microsoft.Extensions.Configuration;

namespace BFB.Server
{
    class GameServer
    {
        private TcpListener Server;
        private readonly IConfiguration _configuration;

        public GameServer(IConfiguration configuration)
        {
            _configuration = configuration;

            IConfigurationSection serverConfig = _configuration.GetSection("Server");

            IPAddress localAddress = IPAddress.Parse(serverConfig["IPAddress"]);

            Server = new TcpListener(localAddress, Convert.ToInt32(serverConfig["Port"]));
        }

        #region Start

        public void Start()
        {
            Server.Start();
            Console.WriteLine($"BFB Server operating at: {_configuration["Server:IPAddress"]}:{_configuration["Server:Port"]}");
            HandleConnections();
        }

        #endregion

        #region Stop

        public void Stop()
        {
            Server.Stop();
        }

        #endregion

        #region HandleConnections

        private void HandleConnections()
        {
            try
            {
                while (true)
                {
                    Console.WriteLine($"Waiting for connections...");

                    TcpClient client = Server.AcceptTcpClient();

                    Console.WriteLine("New client Connected..");

                    Thread t = new Thread(() => HandleClient(client));

                    t.Start();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
                Server.Stop();
            }
        }

        #endregion

        #region HandleClient

        private void HandleClient(TcpClient client)
        {

            //connection stream. This is treated simialr to writing/reading to/from a file
            var clientStream = client.GetStream();

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
                while ((i = clientStream.Read(packetSize, 0,4)) != 0)
                {
                    //get message size and create input array size
                    messageSize = BitConverter.ToInt32(packetSize);

                    //Deserializes a packet from the stream
                    message = PacketManager.Read(clientStream, messageSize);

                    //from here we can use the message from the client
                    Console.WriteLine($"Message Route: {message.Route}, {message.Namespace}");

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: {0}", ex);
                client.Close();
            }

            Console.WriteLine("Client Disconnected");
            client.Close();
        }

        #endregion

    }
}
