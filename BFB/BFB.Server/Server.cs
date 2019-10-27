//C#
using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Threading;
using BFB.Engine.Entity;
using BFB.Engine.Entity.Components.Input;
using BFB.Engine.Entity.Components.Physics;
using BFB.Engine.Math;

//Engine
using BFB.Engine.Server;
using JetBrains.Annotations;

namespace BFB.Server
{
    public class Server
    {
        
        #region Properties
        
        private readonly IConfiguration _configuration;
        private readonly Simulation _simulation;
        private readonly ServerSocketManager _server;

        #endregion

        #region Constructor
        
        private Server(IConfiguration configuration)
        {
            _configuration = configuration;
            
            IPAddress ip = IPAddress.Parse(_configuration["Server:IPAddress"]);
            int port = Convert.ToInt32(_configuration["Server:Port"]);
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

            #region Handle Client Connection

            _server.OnClientConnect = socket =>
            {
                _server.PrintMessage($"Client {socket.ClientId} Connected");
            };
            
            #endregion
            
            #region Handle Client Authentication
            
            _server.OnClientAuthentication = m =>
            {
                _server.PrintMessage($"Client {m.ClientId} Authenticated.");
                return true;
            };
            
            #endregion

            #region Handle Client Ready

            _server.OnClientReady = (socket) =>
            {
                //Add to simulation
                _simulation.AddEntity(new ServerEntity(
                    socket.ClientId,
                    new EntityOptions
                    {
                        AnimatedTextureKey = "Player",
                        Position = new BfbVector(200, 200),
                        Dimensions = new BfbVector(100, 100),
                        Rotation = 0,
                        Origin = new BfbVector(50, 50),
                    }, new ComponentOptions
                    {
                        Physics = new PlayerPhysicsComponent(),
                        Input = new RemoteInputComponent(socket)
                    }));
                
                _server.PrintMessage($"Client {socket.ClientId} Ready and added to Simulation");

            };
            
            #endregion
            
            #region Handle Client Disconnect
            
            _server.OnClientDisconnect = id =>
            {
                _simulation.RemoveEntity(id);
                _server.PrintMessage($"Client {id} Disconnected");
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
        
        [UsedImplicitly]
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
        
        [UsedImplicitly]
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
                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Production")
                {
                    string text = Console.ReadLine();

                    _server.PrintMessage();

                    if (string.IsNullOrEmpty(text))
                        continue;

                    //exit command
                    if (text.ToLower() == "stop") //Convert to server command
                        break;

                    //TODO in the future do something with text/make commands emit events??
                }
                else
                {
                    Thread.Sleep(1000);
                }
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
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development"}.json", true, true)
                .AddEnvironmentVariables();
                
            Server server = new Server(builder.Build());
            server.Start();
        }

        #endregion
    }
}
