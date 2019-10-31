using System;
using System.Collections.Generic;
using System.Linq;
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
        
        private bool _simulating;
        private readonly int _tickSpeed;
        private readonly Random _random;
        
        private readonly Dictionary<string,SimulationEntity> _entitiesIndex;
        private readonly Dictionary<string,SimulationEntity> _playerEntitiesIndex;
        
        private readonly int _simulationDistance;
        
        private readonly WorldManager _world;

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
            _simulating = false;
            _tickSpeed = 1000/tickSpeed ?? (1000 / 60);//60 ticks a second are default
            _random = new Random();
            
            _world = new WorldManager(worldOptions)
            {
                WorldGeneratorCallback = p => _server.PrintMessage("World Generated: " + p)//Remove this or add to world options
            };

            _simulationDistance = 3;

            //entities
            _entitiesIndex = new Dictionary<string, SimulationEntity>();
            _playerEntitiesIndex = new Dictionary<string, SimulationEntity>();
            
            TileComponentManager.LoadTileComponents();
        }
        
        #endregion

        #region GenerateWorld

        public void GenerateWorld()
        {
            lock (_lock)
            {
                _world.GenerateWorld();
            }

//            //Testing of saving world
//            if(_world.SaveWorld("Debug"))
//            {
//                _server.PrintMessage("World Save succeeded");
//                
//                _server.PrintMessage(_world.LoadWorld("Debug") ? "World Loading succeeded" : "World Load Failed");
//            }
//            else
//                _server.PrintMessage("World Save Failed");
        } 
        
        #endregion
        
        #region AddEntity
        
        public void AddEntity(SimulationEntity simulationEntity, bool isPlayer = false)
        {
            lock (_lock)
            {
                if (!_entitiesIndex.ContainsKey(simulationEntity.EntityId))
                {
                    //Add to all entities
                    _entitiesIndex.Add(simulationEntity.EntityId,simulationEntity);
                    
                    //add entity to starting chunk
                    _world.ChunkFromPixelLocation((int) simulationEntity.Position.X, (int) simulationEntity.Position.Y)
                        .Entities.Add(simulationEntity.EntityId, simulationEntity);

                    if (isPlayer)
                    {
                        if (!_playerEntitiesIndex.ContainsKey(simulationEntity.EntityId))
                            _playerEntitiesIndex.Add(simulationEntity.EntityId, simulationEntity);

                        simulationEntity.IsPlayer = true;
                    }

                    simulationEntity.ChunkKey = _world.ChunkFromPixelLocation(
                                                                    (int) simulationEntity.Position.X,
                                                                    (int) simulationEntity.Position.Y).ChunkKey;
                }

                if (_playerEntitiesIndex.Count <= 0 || _simulating) return;
                
                _server.PrintMessage("Simulation Stopping Hibernation.");
                Start();
            }
        }

        #endregion
        
        #region RemoveEntity
        
        public void RemoveEntity(string key)
        {
            lock (_lock)
            {
                if (_entitiesIndex.ContainsKey(key))
                {
                    SimulationEntity entity = _entitiesIndex[key];

                    //Remove from chunk
                    _world.ChunkIndex[entity.ChunkKey].Entities.Remove(key);

                    //remove from entity index
                    _entitiesIndex.Remove(key);
                    
                    //remove from player index if a player
                    if (_playerEntitiesIndex.ContainsKey(key))
                        _playerEntitiesIndex.Remove(key);
                }

                if (_playerEntitiesIndex.Count == 0 && _simulating)
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
            _simulating = true;
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
            _simulating = false;
        }
        
        #endregion
        
        #region SendUpdates

        private void SendUpdates()
        {
            lock (_lock)
            {
                foreach ((string _, SimulationEntity playerEntity) in _playerEntitiesIndex)
                {
                    SendPlayerUpdates(playerEntity);
                    SendChunkUpdates(playerEntity);
                }
            }
        }

        #endregion
        
        #region SendPlayerUpdates

        private void SendPlayerUpdates(SimulationEntity playerEntity)
        {
            EntityUpdateMessage updates = new EntityUpdateMessage();

            //Loop through visible chunks and grab entities that should be displayed
            foreach (string visibleChunkKey in playerEntity.VisibleChunks)
                foreach ((string _, SimulationEntity entity) in _world.ChunkIndex[visibleChunkKey].Entities)
                    updates.Updates.Add(entity.GetState());

            _server.GetClient(playerEntity.EntityId).Emit("/players/updates", updates);//TODO change route
        }
        
        #endregion
        
        #region SendChunkUpdates

        private void SendChunkUpdates(SimulationEntity playerEntity)
        {
            ChunkUpdatesMessage updates = new ChunkUpdatesMessage();

            //Loop through visible chunks and grab entities that should be displayed
            foreach (string visibleChunkKey in playerEntity.VisibleChunks)
            {
                //If the versions are the same we do not need to send the chunk
                if(playerEntity.ChunkVersions[visibleChunkKey] == _world.ChunkIndex[visibleChunkKey].ChunkVersion) 
                    continue;
                
                if (_world.ChunkIndex[visibleChunkKey].NeedChunkUpdate(playerEntity.ChunkVersions[visibleChunkKey]))//Gets an entire chunk for updating
                    updates.ChunkUpdates.Add(_world.ChunkIndex[visibleChunkKey].GetChunkUpdate());
                else
                    updates.ChunkTileUpdates.Add(_world.ChunkIndex[visibleChunkKey].GetChunkTileUpdates(playerEntity.ChunkVersions[visibleChunkKey]));//Gets individual tile updates in a chunk

                playerEntity.ChunkVersions[visibleChunkKey] = _world.ChunkIndex[visibleChunkKey].ChunkVersion;
            }
            
            if(updates.ChunkUpdates.Any() || updates.ChunkTileUpdates.Any())
                _server.GetClient(playerEntity.EntityId).Emit("/players/chunkUpdates", updates);
        }
        
        #endregion

        #region Simulate
        
        private void Simulate()
        {
            long nextTick = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            //Server Game loop
            while (_simulating)
            {
                lock (_lock)
                {
                    //Tick all active chunks
                    foreach ((string _, Chunk chunk) in _world.ChunkIndex)
                    {
                        //Tick all entities in active chunks
                        foreach ((string _, SimulationEntity entity) in chunk.Entities.ToList())
                            entity.Tick(_world, _simulationDistance);//entity components are processed here
                        
                        //randomly choose three tiles in the chunk to tick
                        for (int i = 0; i < 2; i++)
                        {
                            int xBlock = _random.Next(_world.WorldOptions.ChunkSize);
                            int yBlock = _random.Next(_world.WorldOptions.ChunkSize);
                            
                            TileComponentManager.TickTile(_world, chunk, xBlock, yBlock);
                        }
                    }
                }
                
                //This is 
                SendUpdates();
                
                //Maintain the tick rate here
                nextTick += _tickSpeed;
                int sleepTime = (int) (nextTick - DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);
        
                if (sleepTime >= 0)
                    Thread.Sleep(sleepTime);
                else
                    _server.PrintMessage($"SERVER IS OVERLOADED. ({sleepTime}TPS).");
            }
        }
        
        #endregion
        
    }
}