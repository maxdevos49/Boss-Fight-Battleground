//C#
using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Threading;
using BFB.Engine.Entity;
using BFB.Engine.Math;
using BFB.Engine.Server;
using BFB.Engine.Server.Communication;
using BFB.Engine.Simulation;
using BFB.Engine.Simulation.InputComponents;
using BFB.Engine.Simulation.PhysicsComponents;
using BFB.Engine.TileMap.Generators;
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
            
            _simulation = new Simulation(new WorldOptions
            {
                Seed = 1234,
                ChunkSize = 16,
                WorldChunkWidth = 20,
                WorldChunkHeight = 10,
                WorldScale = 15,
                WorldGenerator = options => new FlatWorld(options)
            }, 20);
        }
        
        #endregion

        #region Init

        private void Init()
        {
            #region Server Callbacks
            
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

            #region OnClientPrepare

            _server.OnClientPrepare = (socket) =>
            {
                socket.Emit("prepare", _simulation.World.GetInitWorldData());
            };
            
            #endregion
            
            #region Handle Client Ready

            _server.OnClientReady = (socket) =>
            {
                //Add to simulation
                _simulation.AddEntity(new SimulationEntity(
                    socket.ClientId,
                    new EntityOptions
                    {
                        AnimatedTextureKey = "Boss",
                        Position = new BfbVector(200, 185),
                        Dimensions = new BfbVector(3 * _simulation.World.WorldOptions.WorldScale, 4 * _simulation.World.WorldOptions.WorldScale),
                        Rotation = 0,
                        Origin = new BfbVector(0, 0),
                    }, new ComponentOptions
                    {
                        Physics = new BossPhysicsComponent(),
                        Input = new RemoteInputComponent(socket)
                    }),true);
                
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
            
            #region GenerateWorld
            
            //probably should just be triggered to generate the first time the simulation starts
            _server.OnServerStart = () => _simulation.GenerateWorld();
            
            #endregion
            
            #endregion

            #region Simulation Callbacks
            
            #region OnSimulationStart

            _simulation.OnSimulationStart = () => _server.PrintMessage("Simulation exiting Hibernation");
            
            #endregion
            
            #region OnSimulationStop

            _simulation.OnSimulationStop = () => _server.PrintMessage("Simulation entering Hibernation");
            
            #endregion
            
            #region OnWorldGenerationProgress

            _simulation.OnWorldGenerationProgress = (progress) => _server.PrintMessage("World Generation: " + progress); 
            
            #endregion
            
            #region OnEntityAdd

            _simulation.OnEntityAdd = (entityKey, isPlayer) =>
            {
                //do something here for entities if needed
            };
            
            #endregion
            
            #region OnEntityRemove

            _simulation.OnEntityRemove = (entityKey, isPlayer) =>
            {
                _server.Emit("/player/disconnect", new DataMessage {Message = entityKey});
            };
            
            #endregion
            
            #region OnEntityUpdates

            _simulation.OnEntityUpdates = (entityKey, updates) =>
            {
                _server.GetClient(entityKey).Emit("/players/updates", updates);
            };
            
            #endregion
            
            #region OnChunkUpdates

            _simulation.OnChunkUpdates = (entityKey, updates) =>
            {
                _server.GetClient(entityKey).Emit("/players/chunkUpdates", updates);
            };
            
            #endregion
            
            #region OnSimulationOverload

            _simulation.OnSimulationOverLoad = ticksBehind => _server.PrintMessage($"SERVER IS OVERLOADED. ({ticksBehind}).");
            
            #endregion

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
            _server.PrintMessage($"BFB Server is now Listening on {_configuration["Server:IPAddress"]}:{_configuration["Server:Port"]}");
            _server.Start();

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
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development"}.json"/*Development settings by default*/, true, true)
                .AddEnvironmentVariables();
                
            Server server = new Server(builder.Build());
            server.Start();
        }

        #endregion
    }
}
