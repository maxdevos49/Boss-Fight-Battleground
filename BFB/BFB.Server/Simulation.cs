using System;
using System.Collections.Generic;
using System.Threading;
using BFB.Engine.Entity;
using BFB.Engine.Server;
using BFB.Engine.Server.Communication;
using BFB.Engine.TileMap;
using BFB.Engine.TileMap.Generators;
using BFB.Engine.TileMap.TileComponent;
using JetBrains.Annotations;

namespace BFB.Server
{
    public class Simulation
    {

        #region Properties
        
        private readonly object _lock;
        private readonly int _tickSpeed;
        private readonly Random _random;
        private bool _running;
        
        private readonly Dictionary<string, SimulationEntity> _entities;
        private readonly List<string> _playerEntities;
        
        private readonly WorldManager _world;
        private readonly Dictionary<string,Chunk> _activeChunks;

        private readonly ServerSocketManager _server;//TODO try to remove from simulation later on by use of callbacks and/or events so this can be used on client for local simulations
        

        #endregion
        
        #region Constructor
        
        /**
         * Thread safe simulation class that can be ticked to move the simulation forward a single step at a time
         */
        public Simulation(ServerSocketManager server, WorldOptions worldOptions, int? tickSpeed = null)
        {
            _lock = new object();
            _server = server;
            _running = false;
            _tickSpeed = 1000/tickSpeed ?? (1000 / 60);//60 ticks a second are default
            _random = new Random();
            _activeChunks = new Dictionary<string, Chunk>();
            
            _world = new WorldManager(worldOptions)
            {
                WorldGeneratorCallback = p => _server.PrintMessage("World Generated: " + p)
            };

            //entities
            _entities = new Dictionary<string, SimulationEntity>();
            _playerEntities = new List<string>();
            
        }
        
        #endregion

        #region GenerateWorld

        public void GenerateWorld()
        {
            _world.GenerateWorld();

            //Testing of saving world
            if(_world.SaveWorld("Debug"))
            {
                _server.PrintMessage("World Save succeeded");
                
                _server.PrintMessage(_world.LoadWorld("Debug") ? "World Loading succeeded" : "World Load Failed");
            }
            else
                _server.PrintMessage("World Save Failed");
        } 
        
        #endregion
        
        #region AddEntity
        
        public void AddEntity(SimulationEntity simulationEntity, bool isPlayer = false)
        {
            lock (_lock)
            {
                if(!_entities.ContainsKey(simulationEntity.EntityId))
                    _entities.Add(simulationEntity.EntityId, simulationEntity);

                if (_entities.Count <= 0 || _running) return;
                
                Start();
                _server.PrintMessage("Simulation Stopping Hibernation.");
            }
        }

        #endregion
        
        #region RemoveEntity
        
        public void RemoveEntity(string key)
        {
            lock (_lock)
            {
                if (_entities.ContainsKey(key))
                    _entities.Remove(key);

                if (_entities.Count == 0 && _running)
                {
                    Stop();
                    _server.PrintMessage("Simulation Starting Hibernation.");
                }

            }

            _server.Emit("/player/disconnect", new DataMessage {Message = key});
        }
        
        #endregion
        
        #region Start

        public void Start()
        {
            _running = true;
            Thread t = new Thread(Simulate)
            {
                Name = "Simulation",
                IsBackground = true
            };
            t.Start();
        }
        
        #endregion   
        
        #region Stop

        public void Stop()
        {
            _running = false;
        }
        
        #endregion
        
        #region GetUpdates

        [UsedImplicitly]
        public EntityUpdateMessage GetUpdates()
        {
            lock (_lock)
            {
                EntityUpdateMessage updates = new EntityUpdateMessage();

                foreach ((string key, SimulationEntity entity) in _entities)
                {
                    updates.Updates.Add(entity.GetState());
                }
                
                return updates;
            }
        }

        #endregion

        #region Simulate
        
        private void Simulate()
        {
            long nextTick = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            //Server Game loop
            while (_running)
            {

                lock (_lock)
                {
                    Chunk[,] chunks = _world.GetChunks();

                    //Update all entities
                    foreach ((string _, SimulationEntity entity) in _entities)
                    {
                        entity.Tick(chunks);
                    }

                    //Tick all chunks
                    foreach ((string key, Chunk chunk) in _activeChunks)
                    {

                        //randomly choose three blocks in the chunk to tick
                        for (int i = 0; i < 3; i++)
                        {
                            int xBlock = _random.Next(_world.WorldOptions.ChunkSize);
                            int yBlock = _random.Next(_world.WorldOptions.ChunkSize);

                            TileComponentManager.TickTile(chunk,xBlock,yBlock);
                        }
                    }
                    
                    //update player active chunks
                    //TODO track remote users(actual players)
                    
                }

                //Send changes. In future cull updates per player to reduce sending un needed data to some clients(because that thing may not be on there screen)
                _server.Emit("/players/updates", GetUpdates());//TODO change
                
                //Ask for input from all players
                _server.Emit("/players/getUpdates");

                //Maintain the tick rate here
                nextTick += _tickSpeed;
                int sleepTime = (int) (nextTick - (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond));
        
                if (sleepTime >= 0)
                    Thread.Sleep(sleepTime);
                else
                    _server.PrintMessage($"SERVER IS OVERLOADED. ({sleepTime}TPS).");
            }
        }
        
        #endregion
        
    }
}