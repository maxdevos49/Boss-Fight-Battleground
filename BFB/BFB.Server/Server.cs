//C#
using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

//Engine
using System.Net;
using System.Threading;

//Server
using BFB.Server.Client;
using BFB.Engine.Server;
using BFB.Engine.Event;
using JetBrains.Annotations;

namespace BFB.Server
{
    public class Server
    {

        private readonly TcpListener _listener;
        private readonly ClientManager _clients;
        [UsedImplicitly] private readonly IDictionary<string, Command> _commands;
        [UsedImplicitly] private readonly EventManager _eventManager;
        private readonly IConfiguration _configuration;

        private Server(IConfiguration configuration)
        {
            _configuration = configuration;

            _eventManager = new EventManager();
            _clients = new ClientManager();

            _listener = new TcpListener(IPAddress.Parse(_configuration["Server:IPAddress"]), Convert.ToInt32(_configuration["Server:Port"]));

            //Load/Register all commands
            _commands = new Dictionary<string, Command>();

            //Create/Load Simulation World

        }

        private void HandleTerminalInput()
        {
            while (true)
            {
                string text = Console.ReadLine();
                
                PrintToTerminal();
                
                if (string.IsNullOrEmpty(text))
                    continue;
               
                //exit command
                if (text.ToLower() == "stop")//Convert to server command
                    break;

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
                    if (_listener.Pending())
                    {
                        TcpClient client = _listener.AcceptTcpClient();

                        _clients.Add(client);
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

        private void HandleClientStream(TcpClient client)
        {

            NetworkStream stream = client.GetStream();

            // ReSharper disable once NotAccessedVariable
            Packet message;
            byte[] packetSize = new byte[4];

            try
            {

                /**
                 * Read returns number of bytes to read so if nothing was read then looping
                 * stops. If still connected I think this just waits until there for input.
                 * Aka it only returns 0 if its disconnected
                 * */
                while (stream.Read(packetSize, 0, 4) != 0)
                {
                    //get message size and create input array size
                    int messageSize = BitConverter.ToInt32(packetSize);

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
                    using (MemoryStream memoryStream = new MemoryStream(messageData))
                        // ReSharper disable once RedundantAssignment
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

        [UsedImplicitly]
        public void Start()
        {
            //Terminal Prep
            Console.Clear();
            Console.Title = "BFB Server";

            PrintToTerminal();
            PrintToTerminal($"BFB Server is now Listening on {_configuration["Server:IPAddress"]}:{_configuration["Server:Port"]}");

            _listener.Start();

            Thread t = new Thread(HandleClientConnections)
            {
                Name = "ClientConnections",
                IsBackground = true
            };

            t.Start();

            HandleTerminalInput();
        }

        [UsedImplicitly]
        public void Stop()
        {
            PrintToTerminal("Shutting down...", false);
            _listener.Stop();
            Console.WriteLine();
        }

        private void PrintToTerminal(string message = null, bool printHeader = true)
        {
            if (message != null)
                Console.WriteLine(message);

            if (!printHeader) return;
            
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"[BFB-Server|{DateTime.Now:h:mm:ss tt}|c:{_clients.Count()}] ");
            Console.ResetColor();
        }

        #region Main

        private static void Main()
        {
            Thread.CurrentThread.Name = "Main";

            //get configuration
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true);

            Server server = new Server(builder.Build());
            server.Start();
        }

        #endregion
    }
}
