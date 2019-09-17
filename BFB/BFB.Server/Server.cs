//C#
using System;
using System.IO;
using System.Net.Sockets;
using Microsoft.Extensions.Configuration;
using System.Runtime.Serialization.Formatters.Binary;

//Engine
using System.Net;
using System.Threading;

//Server
using BFB.Server.Client;
using BFB.Engine.Server;

namespace BFB.Server
{
    public class Server
    {

        public bool LineComplete { get; set; }
        public TcpListener Listener { get; set; }
        public ClientManager Clients { get; set; }

        public IConfiguration Configuration { get; set; }

        public Server(IConfiguration configuration)
        {
            LineComplete = true;
            Configuration = configuration;

            Clients = new ClientManager();
            Listener = new TcpListener(IPAddress.Parse(Configuration["Server:IPAddress"]), Convert.ToInt32(Configuration["Server:Port"]));
        }

        private void HandleTerminalInput()
        {
            while (true)
            {
                //reads a line
                string text = Console.ReadLine();
                PrintToTerminal();

                //exit command
                if (text.ToLower() == "stop")
                    break;


                if (text == string.Empty)
                    continue;

                //TODO in the future do something with text/make commands emit event??
            }

            Stop();
        }

        private void HandleClientConnections()
        {
            try
            {
                while (true)
                {
                    if (Listener.Pending())
                    {
                        TcpClient client = Listener.AcceptTcpClient();

                        Clients.Add(client);//client manager
                        PrintToTerminal("New client Connected..");

                        Thread t = new Thread(() => HandleClientStream(client))
                        {
                            Name = "ClientStream",
                            IsBackground = true
                        };
                        t.Start();
                    }
                    else
                    {
                        Thread.Sleep(300);
                    }
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
                Stop();
            }
        }

        private void HandleClientStream(TcpClient client)
        {

            var stream = client.GetStream();

            Packet message;
            byte[] packetSize = new byte[4];
            int i, messageSize;

            try
            {

                /**
                 * Read returns number of bytes to read so if nothing was read then looping
                 * stops. If still connected I think this just waits until there for input.
                 * Aka it only returns 0 if its disconnected
                 * */
                while ((i = stream.Read(packetSize, 0, 4)) != 0)
                {
                    //get message size and create input array size
                    messageSize = BitConverter.ToInt32(packetSize);

                    //Deserializes a packet from the stream
                    byte[] messageData = new byte[messageSize];

                    //Safe read
                    int bytesRead = 0;
                    do
                    {
                        Console.WriteLine($"Byte position: {bytesRead}");
                        bytesRead += stream.Read(messageData, bytesRead, messageSize - bytesRead);
                    } while (bytesRead < messageSize);

                    Console.WriteLine($"Total bytes Read: {bytesRead}");

                    //do something with message now
                    using (var memoryStream = new MemoryStream(messageData))
                        message = (Packet)new BinaryFormatter().Deserialize(memoryStream);

                    //TODO send message to server side route

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

        public void Start()
        {

            Listener.Start();

            PrintToTerminal();
            PrintToTerminal($"BFB Server is now Listening on {Configuration["Server:IPAddress"]}:{Configuration["Server:Port"]}");

            Thread t = new Thread(HandleClientConnections)
            {
                Name = "ClientConnections",
                IsBackground = true
            };

            t.Start();

            HandleTerminalInput();
        }

        public void Stop()
        {
            PrintToTerminal("Shutting down...", false);
            Listener.Stop();
            Console.WriteLine();
            Environment.Exit(Environment.ExitCode);
        }

        private void PrintToTerminal(string message = null, bool printHeader = true)
        {
            if (message != null)
                Console.WriteLine(message);

            if (printHeader)
                Console.Write($"[Server|{DateTime.Now.ToString("h:mm:ss tt")}|c:{Clients.Count()}] ");
        }

        #region Main

        static void Main(string[] args)
        {
            Thread.CurrentThread.Name = "Main";

            //get configuration
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true);

            var server = new Server(builder.Build());
            server.Start();
        }

        #endregion
    }
}
