//C#
using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Threading;
using BFB.Engine.Entity;
using BFB.Engine.Entity.Components.Graphics;
using BFB.Engine.Entity.Components.Input;
using BFB.Engine.Entity.Components.Physics;
using BFB.Engine.Math;

//Engine
using BFB.Engine.Server;

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
            _configuration = configuration;
            
            IPAddress ip = IPAddress.Parse(_configuration["Server:IPAddress"]);
            int port = Convert.ToInt32(_configuration["Server:Port"]);
            
            _lock = new object();
            _server = new ServerSocketManager(ip,port);
            _simulation = new Simulation(_server);
        }
        
        #endregion

        #region Init

        private void Init()
        {
            
            #region Terminal Header
            
            //Terminal Header format
            _server.SetTerminalHeader(() =>
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"[BFB-Server|{DateTime.Now:h:mm:ss tt}|T:{Process.GetCurrentProcess().Threads.Count}] ");
                Console.ResetColor();
            });
            
            #endregion

            #region Handle Connection

            _server.OnClientConnect = socket =>
            {
                _server.PrintMessage($"Client {socket.ClientId} Connected");
            };
            
            #endregion
            
            #region Handle Authentication
            
            _server.OnClientAuthentication = m =>
            {
                _server.PrintMessage($"Client {m.ClientId} Authenticated.");
                
                
                //Add to simulation
                ServerEntity entity = new ServerEntity(
                    m.ClientId, 
                    new EntityOptions
                    {
                        Position = new BfbVector(200,200),
                        Dimensions = new BfbVector(100,100),
                        Rotation = 0,
                        Origin = new BfbVector(50,50),
                    }, new ComponentOptions
                    {
                        Physics = new AccelerateComponent(),
                        Input = new RemoteInputComponent(_server.GetClient(m.ClientId))
                    });
                
                _simulation.AddEntity(entity);
                
                return true;
            };
            
            #endregion

            #region Handle Disconnect
            
            _server.OnClientDisconnect = socket =>
            {
                _simulation.RemoveEntity(socket.ClientId);
                _server.PrintMessage($"Client {socket.ClientId} Disconnected");
            };
            
            #endregion
            
            #region Terminal Prep
            
            Console.Clear();
            Console.Title = "BFB Server";
            _server.PrintMessage();
            
            #endregion

        }
        
        #endregion
        
        #region Start
        
        public void Start()
        {
            Init();
            _simulation.Start();
            _server.Start();
            _server.PrintMessage($"BFB Server is now Listening on {_configuration["Server:IPAddress"]}:{_configuration["Server:Port"]}");
            HandleTerminalInput();
        }
        
        #endregion

        #region Stop
        
        public void Stop()
        {
            _server.PrintMessage("Server shutting down...", false);
            _server.Stop();
            _simulation.Stop();
            Console.WriteLine();
        }
        
        #endregion
        
        #region Handle Terminal
        
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
