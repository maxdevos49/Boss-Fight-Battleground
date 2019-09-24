//C#
using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Extensions.Configuration;

//Engine
using System.Net;
using System.Threading;
using BFB.Engine.Server.Communication;
using BFB.Engine.Server.Socket;

namespace BFB.Server
{
    public class Server
    {
        
        #region Properties
        
        /**
         * Thread Locker
         */
        private readonly object _lock;
        
        /**
         * Server configuration. Contains all settings defined in appsettings.json or settings overridden by environment variables
         */
        private readonly IConfiguration _configuration;
        
        /**
         * This manages the simulation and does all of the calculations for entities and such
         */
        private readonly Simulation _simulation;
        
        /**
         * Manages the server connections and routes all tcp request to the correct handelers
         */
        private readonly ServerSocketManager _server;

        #endregion

        #region Constructor
        
        private Server(IConfiguration configuration)
        {
            //DI
            _configuration = configuration;
            
            _lock = new object();
            _simulation = new Simulation();
            
            //server
            IPAddress ip = IPAddress.Parse(_configuration["Server:IPAddress"]);
            int port = Convert.ToInt32(_configuration["Server:Port"]);
            _server = new ServerSocketManager(ip,port);

        }
        
        #endregion

        #region Init

        private void Init()
        {
            //Terminal Prep
            Console.Clear();
            Console.Title = "BFB Server";
            
            //Terminal Header format
            _server.SetTerminalHeader(() =>
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"[BFB-Server|{DateTime.Now:h:mm:ss tt}|T:{Process.GetCurrentProcess().Threads.Count - System.Diagnostics.}] ");
                Console.ResetColor();
            });

            //Client authentication strategy
            _server.OnClientAuthentication(socket =>
            {
                _server.PrintMessage($"Client {socket.Key} Authenticated with Standard Validation(No Validation)");
                return true;
            });

            //What to do when a client connects
            _server.OnClientConnect(socket =>
            {
                _server.PrintMessage($"Client {socket.Key} Connected");
                
                _server.PrintMessage("Sending Ping...");
                
                socket.Emit("ping", new DataMessage{Message = "Hey hey Hey"});
                
            });

            //what to do when a client disconnects
            _server.OnClientDisconnect(socket =>
            {
                _server.PrintMessage($"Client {socket.Key} Disconnected");
            });

            //Test
            _server.On("pong", message =>
            {
                _server.PrintMessage("Pong received!!");
            });
            
            //Print initial console header
            _server.PrintMessage();
        }
        
        #endregion
        
        #region Start
        
        public void Start()
        {
            Init();
            _server.Start();
            _server.PrintMessage($"BFB Server is now Listening on {_configuration["Server:IPAddress"]}:{_configuration["Server:Port"]}");
            HandleTerminalInput();
        }
        
        #endregion

        #region Stop
        
        public void Stop()
        {
            _server.Stop();
            Console.WriteLine();
        }
        
        #endregion
        
        #region HandleTerminalInput
        
        private void HandleTerminalInput()
        {
            while (true)
            {
                string text = Console.ReadLine();
                
                _server.PrintMessage();
                
                if (string.IsNullOrEmpty(text))
                    continue;
               
                //exit command
                if (text.ToLower() == "stop")//Convert to server command
                    break;

                //TODO in the future do something with text/make commands emit event??
            }

            Stop();
        }
        
        #endregion
        
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
