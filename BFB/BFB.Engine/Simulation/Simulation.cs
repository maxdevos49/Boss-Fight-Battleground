using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BFB.Engine.Entity;
using BFB.Engine.Server.Communication;
using BFB.Engine.TileMap;
using BFB.Engine.TileMap.Generators;
using BFB.Engine.TileMap.TileComponent;

namespace BFB.Engine.Simulation
{
    /// <summary>
    /// Creates a simulation for simulating entities and a tilemap in a game
    /// </summary>
    public class Simulation
    {

        #region Properties
        
        private readonly object _lock;
        
        private bool _simulating;
        private readonly int _tickSpeed;
        private readonly Random _random;
        public int Tick { get; private set; } 
        private readonly Dictionary<string,SimulationEntity> _entitiesIndex;
        private readonly Dictionary<string, SimulationEntity> _playerEntitiesIndex;

        /// <summary>
        /// Indicates the distance at which a player causes the simulation to simulate
        /// </summary>
        public readonly int SimulationDistance;
        public readonly WorldManager World;
        
        /// <summary>
        /// Holds the World Manager for maintaining world state
        /// </summary>
        public readonly WorldManager WorldManager;
        
        /// <summary>
        /// Callback that is fired on every 1% of world generation progress. Supplies the current world generating progress as a string
        /// </summary>
        public Action<string> OnWorldGenerationProgress { get; set; }
        
        /// <summary>
        /// Callback that is called when the simulation is started
        /// </summary>
        public Action OnSimulationStart { get; set; }
        
        /// <summary>
        /// Callback that is called when the simulation is stopped
        /// </summary>
        public Action OnSimulationStop { get; set; }
        
        /// <summary>
        /// Callback that is called when a entity is added. Supplies the entity key and a boolean indicating if the entity is a player.
        /// </summary>
        public Action<string,bool> OnEntityAdd { get; set; }
        
        /// <summary>
        /// Callback that is called when a entity is removed. Supplies the entity key and a boolean indicating if the entity is a player
        /// </summary>
        public Action<string,bool> OnEntityRemove { get; set; }
        
        /// <summary>
        /// Callback that is called when the simulation is ready to supply updates for a player entity. Supplies the player entity key and its relevant updates
        /// </summary>
        public Action<string,EntityUpdateMessage> OnEntityUpdates { get; set; }
        
        /// <summary>
        /// Callback that is called when the simulation is ready to provide updates for player entity. Supplies the player entity key and its relevant updates
        /// </summary>
        public Action<string,ChunkUpdatesMessage> OnChunkUpdates { get; set; }
        public Action<string> OnSimulationOverLoad { get; set; }
        

        #endregion
        
        #region Constructor
        
        /// <summary>
        /// Constructs a simulation
        /// </summary>
        /// <param name="worldOptions">Given world options that is given to the world manager</param>
        /// <param name="tickSpeed">A optional parameter used for indicating the ticks per second. (20tps is the default)</param>
        public Simulation(WorldOptions worldOptions, int tickSpeed = 20)
        {
            _lock = new object();
            _simulating = false;
            _tickSpeed = 1000/tickSpeed;//60 ticks a second are default
            _random = new Random();
            Tick = 0;

            World = new WorldManager(worldOptions);

            SimulationDistance = 3;

            //entities
            _entitiesIndex = new Dictionary<string, SimulationEntity>();
            _playerEntitiesIndex = new Dictionary<string, SimulationEntity>();
            
            TileComponentManager.LoadTileComponents();
        }
        
        #endregion

        #region GenerateWorld

        /// <summary>
        /// Generates the entire tilemap. Calls the OnWorldGenerationProgress during the generation
        /// </summary>
        public void GenerateWorld()
        {
            lock (_lock)
            {
                World.GenerateWorld(OnWorldGenerationProgress);
            }
        } 
        
        #endregion
        
        #region AddEntity
        
        /// <summary>
        /// Adds a entity to the simulation
        /// </summary>
        /// <param name="simulationEntity">The simulation entity</param>
        /// <param name="isPlayer">Optional parameter indicating if the entity is a player.(false is default)</param>
        public void AddEntity(SimulationEntity simulationEntity, bool isPlayer = false)
        {
            lock (_lock)
            {
                if (!_entitiesIndex.ContainsKey(simulationEntity.EntityId))
                {
                    //Add to all entities
                    _entitiesIndex.Add(simulationEntity.EntityId,simulationEntity);
                    
                    OnEntityAdd?.Invoke(simulationEntity.EntityId, isPlayer);
                    
                    //add entity to starting chunk
                    World.ChunkFromPixelLocation((int) simulationEntity.Position.X, (int) simulationEntity.Position.Y)
                        .Entities.Add(simulationEntity.EntityId, simulationEntity);

                    if (isPlayer)
                    {
                        if (!_playerEntitiesIndex.ContainsKey(simulationEntity.EntityId))
                            _playerEntitiesIndex.Add(simulationEntity.EntityId, simulationEntity);

                        simulationEntity.IsPlayer = true;
                    }

                    simulationEntity.ChunkKey = World.ChunkFromPixelLocation(
                                                                    (int) simulationEntity.Position.X,
                                                                    (int) simulationEntity.Position.Y).ChunkKey;
                }

                if (_playerEntitiesIndex.Count <= 0 || _simulating) return;
                
                Start();
            }
        }

        #endregion
        
        #region RemoveEntity
        
        /// <summary>
        /// Removes a entity from the simulation with a entity id.
        /// </summary>
        /// <param name="key">The entity Id used to determine who is being removed from the simulation.</param>
        public void RemoveEntity(string key)
        {
            bool isPlayer = false;
            
            lock (_lock)
            {
                if (_entitiesIndex.ContainsKey(key))
                {
                    SimulationEntity entity = _entitiesIndex[key];

                    //Remove from chunk
                    World.ChunkIndex[entity.ChunkKey].Entities.Remove(key);

                    //remove from entity index
                    _entitiesIndex.Remove(key);
                    
                    //remove from player index if a player
                    if (_playerEntitiesIndex.ContainsKey(key))
                    {
                        _playerEntitiesIndex.Remove(key);
                        isPlayer = true;
                    }
                }

                if (_playerEntitiesIndex.Count == 0 && _simulating)
                    Stop();

            }

            OnEntityRemove?.Invoke(key,isPlayer);
        }

        #endregion

        #region GetEntityAtPosition

        public SimulationEntity GetEntityAtPosition(int x, int y, Boolean isFacingRight)
        {
            float tileSize = World.WorldOptions.WorldScale;
            foreach (KeyValuePair<string, SimulationEntity> player in _playerEntitiesIndex)
            {
                if (player.Value.Position.X <= x && player.Value.Position.X + tileSize * 2 >= x)
                {
                    if (player.Value.Position.Y <= y && player.Value.Position.Y + tileSize * 2 >= y)
                    {
                        return player.Value;
                    }
                }
            }

            return null;
        }

        #endregion

        #region Start

        /// <summary>
        /// Starts the simulation
        /// </summary>
        public void Start()
        {
            OnSimulationStart?.Invoke();
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

        /// <summary>
        /// Stops the simulation
        /// </summary>
        public void Stop()
        {
            OnSimulationStop?.Invoke();
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
                foreach ((string _, SimulationEntity entity) in World.ChunkIndex[visibleChunkKey].Entities)
                    updates.Updates.Add(entity.GetState());

            OnEntityUpdates?.Invoke(playerEntity.EntityId, updates);
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
                if(playerEntity.ChunkVersions[visibleChunkKey] == World.ChunkIndex[visibleChunkKey].ChunkVersion) 
                    continue;
                
                if (World.ChunkIndex[visibleChunkKey].NeedChunkUpdate(playerEntity.ChunkVersions[visibleChunkKey]))//Gets an entire chunk for updating
                    updates.ChunkUpdates.Add(World.ChunkIndex[visibleChunkKey].GetChunkUpdate());
                else
                    updates.ChunkTileUpdates.Add(World.ChunkIndex[visibleChunkKey].GetChunkTileUpdates(playerEntity.ChunkVersions[visibleChunkKey]));//Gets individual tile updates in a chunk

                playerEntity.ChunkVersions[visibleChunkKey] = World.ChunkIndex[visibleChunkKey].ChunkVersion;
            }
            
            if(updates.ChunkUpdates.Any() || updates.ChunkTileUpdates.Any())
                OnChunkUpdates?.Invoke(playerEntity.EntityId,updates);
        }
        
        #endregion

        #region Simulate
        
        private void Simulate()
        {
            long nextTick = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            //Server Game loop
            while (_simulating)
            {
                Tick++;

                lock (_lock)
                {
                    //Tick all active chunks
                    foreach ((string _, Chunk chunk) in World.ChunkIndex)
                    {
                        //Tick all entities in active chunks
                        foreach ((string _, SimulationEntity entity) in chunk.Entities.ToList())
                            entity.Tick(this);//entity components are processed here
                        
                        //randomly choose three tiles in the chunk to tick
                        for (int i = 0; i < 19; i++)
                        {
                            int xBlock = _random.Next(World.WorldOptions.ChunkSize);
                            int yBlock = _random.Next(World.WorldOptions.ChunkSize);
                            
                            TileComponentManager.TickTile(World, chunk, xBlock, yBlock);
                        }
                    }
                }
                
                //This is the communication aspect
                SendUpdates();
                
                //Maintain the tick rate here
                nextTick += _tickSpeed;
                int sleepTime = (int) (nextTick - DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);
        
                if (sleepTime >= 0)
                    Thread.Sleep(sleepTime);
                else
                    OnSimulationOverLoad?.Invoke($"{sleepTime}TPS");
            }
        }
        
        #endregion
        
    }
}