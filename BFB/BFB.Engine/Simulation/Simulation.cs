using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BFB.Engine.Entity;
using BFB.Engine.Inventory;
using BFB.Engine.Server.Communication;
using BFB.Engine.Simulation.BlockComponent;
using BFB.Engine.TileMap;
using BFB.Engine.TileMap.Generators;
using JetBrains.Annotations;

namespace BFB.Engine.Simulation
{
    /// <summary>
    /// Creates a simulation for simulating entities and a tilemap in a game
    /// </summary>
    public class Simulation
    {

        #region Properties
        
        private readonly object _lock;
        
        private SimulationState _state;
        private readonly int _tickSpeed;
        private readonly Random _random;
        
        public int Tick { get; private set; } 
        private readonly Dictionary<string,SimulationEntity> _entitiesIndex;
        private readonly Dictionary<string, SimulationEntity> _playerEntitiesIndex;
        
//        public readonly List<GameModeComponent> GameComponents;
//        public GameState GameState;

        public int ConnectedClients;

        /// <summary>
        /// Indicates the distance at which a player causes the simulation to simulate
        /// </summary>
        public readonly int SimulationDistance;
        
        /// <summary>
        /// Holds the World Manager for maintaining world state
        /// </summary>
        public readonly WorldManager World;

        
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
        
        /// <summary>
        /// Callback that is called when the simulation is ready to provide updates for a players inventory
        /// </summary>
        public Action<string, InventorySlotMessage> OnInventoryUpdate { get; set; }
        
        public Action OnSimulationTick { get; set; }
        
        public Action<string> OnSimulationOverLoad { get; set; }
        
        [CanBeNull] 
        private SimulationGameMode GameMode { get; set; }

        #endregion
        
        #region Constructor

        /// <summary>
        /// Constructs a simulation
        /// </summary>
        /// <param name="gameMode"></param>
        /// <param name="worldOptions">Given world options that is given to the world manager</param>
        /// <param name="tickSpeed">A optional parameter used for indicating the ticks per second. (20tps is the default)</param>
        public Simulation(SimulationGameMode gameMode ,WorldOptions worldOptions, int tickSpeed = 20)
        {
            _lock = new object();
            GameMode = gameMode;
            _state = SimulationState.Shutdown;
            _tickSpeed = 1000/tickSpeed;
            _random = new Random();
            Tick = 0;
            ConnectedClients = 0;

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
        /// <param name="entity">The simulation entity</param>
        public void AddEntity(SimulationEntity entity)
        {
            //init components
            entity.Init();
            
            lock (_lock)
            {
                //Apply proper dimensions
                entity.Dimensions.Mult(World.WorldOptions.WorldScale);
                
                //Apply proper origin
                entity.Origin.X *= entity.Dimensions.X;
                entity.Origin.Y *= entity.Dimensions.Y;

                if (!_entitiesIndex.ContainsKey(entity.EntityId))
                {
                    //Add to all entities
                    _entitiesIndex.Add(entity.EntityId,entity);
                    
                    OnEntityAdd?.Invoke(entity.EntityId, entity.EntityType == EntityType.Player);
                    GameMode?.EmitEntityAdd(this, entity);
                    
                    //add entity to starting chunk
                    Chunk chunk = World.ChunkFromPixelLocation((int) entity.Position.X, (int) entity.Position.Y);
                        
                    if(chunk == null)
                        return;
                            
                    chunk.Entities.Add(entity.EntityId, entity);

                    if (entity.EntityType == EntityType.Player)
                    {
                        if (!_playerEntitiesIndex.ContainsKey(entity.EntityId))
                            _playerEntitiesIndex.Add(entity.EntityId, entity);
                    }

                    entity.ChunkKey = World.ChunkFromPixelLocation((int) entity.Position.X,
                                                                   (int) entity.Position.Y).ChunkKey;
                }

                if (_state == SimulationState.ShuttingDown)
                    _state = SimulationState.Running;

                if (_playerEntitiesIndex.Count <= 0 || _state == SimulationState.Running) return;
                
                Start();
            }
        }

        #endregion
        
        #region RemoveEntity

        /// <summary>
        /// Removes a entity from the simulation with a entity id.
        /// </summary>
        /// <param name="key">The entity Id used to determine who is being removed from the simulation.</param>
        /// <param name="reason"></param>
        public void RemoveEntity(string key, EntityRemovalReason? reason = null)
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

                    entity.EmitOnSimulationRemoval(this, reason);
//                    EmitRemoveEntity(entity, reason);
                    GameMode?.EmitEntityRemove(this, entity,reason);
                }

                if (_playerEntitiesIndex.Count == 0 && _state == SimulationState.Running)
                    Stop();

            }

            OnEntityRemove?.Invoke(key,isPlayer);
        }

        #endregion

        #region GetAllEntities

        public List<SimulationEntity> GetAllEntities()
        {
            return _entitiesIndex.Values.ToList();
        }

        #endregion

        #region GetEntity

        [CanBeNull]
        public SimulationEntity GetEntity(string entityId)
        {
            return _entitiesIndex.ContainsKey(entityId) ? _entitiesIndex[entityId] : null;
        }

        #endregion

        #region GetPlayerEntities
        
        public List<SimulationEntity> GetPlayerEntities()
        {
            return _playerEntitiesIndex.Values.ToList();
        }

        #endregion

        #region Start

        /// <summary>
        /// Starts the simulation
        /// </summary>
        [UsedImplicitly]
        public void Start()
        {
            OnSimulationStart?.Invoke();
            GameMode?.EmitSimulationStart(this);
            
            _state = SimulationState.Running;
            
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
            GameMode?.EmitSimulationStop(this);
            _state = SimulationState.ShuttingDown;
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
                    SendInventoryUpdates(playerEntity);
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
        
        #region SendInventoryUpdates

        private void SendInventoryUpdates(SimulationEntity playerEntity)
        {
            InventorySlotMessage updates = new InventorySlotMessage();
            List<byte> slots = new List<byte>();

            if (playerEntity.Inventory == null) 
                return;
            
            foreach ((byte slotId, IItem item) in playerEntity.Inventory.GetAllItems())
            {
                if (item == null)
                    playerEntity.Inventory.Remove(slotId);
                else
                    updates.SlotUpdates.Add(new InventorySlot
                    {
                        Count = item.StackSize(),
                        Name = item.ItemConfigKey,
                        ItemType = item.Configuration.ItemType,
                        Mode = false,
                        SlotId = slotId,
                        TextureKey = item.Configuration.TextureKey
                    });
                
                slots.Add(slotId);
            }

            foreach (byte slotId in playerEntity.InventorySlotHistory.Where(slotId => !slots.Contains(slotId)))
            {
                updates.SlotUpdates.Add(new InventorySlot
                {
                    Mode = true,
                    SlotId =  slotId
                });
            }

            playerEntity.InventorySlotHistory = slots;

            if(updates.SlotUpdates.Any())
                OnInventoryUpdate?.Invoke(playerEntity.EntityId,updates);
        }

        #endregion

        #region Simulate
        
        private void Simulate()
        {
            long nextTick = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            if (_state == SimulationState.ShuttingDown)
                _state = SimulationState.Shutdown;
            
            //Server Game loop
            while (_state == SimulationState.Running)
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
                        for (int i = 0; i < 3; i++)
                        {
                            int xBlock = _random.Next(World.WorldOptions.ChunkSize);
                            int yBlock = _random.Next(World.WorldOptions.ChunkSize);
                            
                            TileComponentManager.TickTile(World, chunk, xBlock, yBlock);
                        }
                    }

                    // Run all of the components to simulate gameplay.
                    GameMode?.Update(this);
                }
                
                //This is the communication aspect
                SendUpdates();
                
                //Maintain the tick rate here
                nextTick += _tickSpeed;
                int sleepTime = (int) (nextTick - DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);

                if (sleepTime >= 0)
                {
                    OnSimulationTick?.Invoke();
                    Thread.Sleep(sleepTime);
                }
                else
                    OnSimulationOverLoad?.Invoke($"{sleepTime}TPS");
            }
        }
        
        #endregion
        
    }

    public enum SimulationState
    {
        Running,
        Shutdown,
        ShuttingDown
    }
}