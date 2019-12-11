using System;
using System.Collections.Generic;
using System.Drawing;
using BFB.Engine.Collisions;
using BFB.Engine.Entity.Configuration;
using BFB.Engine.Input.PlayerInput;
using BFB.Engine.Inventory;
using BFB.Engine.Math;
using BFB.Engine.Server;
using BFB.Engine.Simulation;
using BFB.Engine.Simulation.EntityComponents;
using BFB.Engine.TileMap;
using JetBrains.Annotations;

namespace BFB.Engine.Entity
{
    /// <summary>
    /// An entity stored in the game server's simulation 
    /// </summary>
    public class SimulationEntity : Entity
    {

        #region Properties

        private int _currentTick;

        public EntityConfiguration EntityConfiguration { get; set; }


        public string ParentEntityId { get; set; }
        
        /// <summary>
        /// The amount of ticks since the creation of the entity
        /// </summary>
        public int TicksSinceCreation { get; private set; }
        
        /// <summary>
        /// Vector describing a position an entity is attempting to move to 
        /// </summary>
        public BfbVector SteeringVector { get; set; }

        /// <summary>
        /// The old position of the entity
        /// </summary>
        public BfbVector OldPosition { get; private set; }

        /// <summary>
        /// A list of visible chunks a player can see
        /// </summary>
        public List<string> VisibleChunks { get; }

        /// <summary>
        /// A dictionary of chunk versions that the player is aware of
        /// </summary>
        public Dictionary<string, int> ChunkVersions { get; }
        
        /// <summary>
        /// Indication of what to send to the client
        /// </summary>
        public List<byte> InventorySlotHistory { get; set; }
        
        /// <summary>
        /// The Inventory of the Entity
        /// </summary>
        [CanBeNull]
        public IInventoryManager Inventory { get; set; }
        
        /// <summary>
        /// The ControlState of the Entity
        /// </summary>
        [CanBeNull]
        public ControlState ControlState { get; set; }
        
        /// <summary>
        /// The socket connection a entity may have if its a player
        /// </summary>
        [CanBeNull]
        public ClientSocket Socket { get; set; }
        
        /// <summary>
        /// Indicates the collision group  the entity is from
        /// </summary>
        public string CollideFilter { get; set; }
        
        /// <summary>
        /// Indicates the collision groups the entity will collide with
        /// </summary>
        public List<string> CollideWithFilters { get; set; }
        

        /// <summary>
        /// The entity bounds as a rectangle
        /// </summary>
        public Rectangle Bounds => new Rectangle((int) Position.X, (int) Position.Y, (int) Dimensions.X, (int) Dimensions.Y);
        
        #region Entity Frame Helpers
        
        public int OldBottom => (int) (OldPosition.Y + Height);
        public int OldLeft => (int) OldPosition.X;
        public int OldRight => (int) (OldPosition.X + Width);
        public int OldTop => (int) OldPosition.Y;
        
        #endregion

        /// <summary>
        /// Game Components
        /// </summary>
        private List<EntityComponent> EntityComponents { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new entity for the game simulations
        /// </summary>
        /// <param name="entityId">Unique ID for this entity</param>
        /// <param name="options">Sets the initial properties of this entity</param>
        /// <param name="socket">The socket object if the entity is a player</param>
        private SimulationEntity(string entityId, EntityOptions options, ClientSocket socket = null) : base(entityId, options)
        {
            _currentTick = -1;
            TicksSinceCreation = -1;
            ParentEntityId = null;
            
            SteeringVector = new BfbVector();
            OldPosition = new BfbVector();

            //Default collision info
            CollideFilter = "entity";
            CollideWithFilters = new List<string> {"tile"};
            
            //Components
            EntityComponents = new List<EntityComponent>();

            if (socket != null)
            {
                VisibleChunks = new List<string>();
                ChunkVersions = new Dictionary<string, int>();
                InventorySlotHistory = new List<byte>();
                Socket = socket;
            }
        }

        #endregion
        
        #region Init

        public void Init()
        {
            foreach (EntityComponent simulationComponent in EntityComponents)
                simulationComponent?.Init(this);
        }
        
        #endregion

        #region Update

        /// <summary>
        /// Updates this entity as part of the chunks in the simulation of this entity
        /// </summary>
        /// <param name="simulation"></param>
        public void Tick(Simulation.Simulation simulation)
        {
            TicksSinceCreation++;
            
            //Only tick entity once per frame
            if (simulation.Tick == _currentTick)
                return;

            //Record last tick
            _currentTick = simulation.Tick;

            //Record current position
            OldPosition = new BfbVector(Position.X, Position.Y);

            //the future
            foreach (EntityComponent gameComponent in EntityComponents)
                gameComponent?.Update(this,simulation);

            MoveEntity(simulation);
        }

        #endregion 
        
        #region MoveEntity

        private void MoveEntity(Simulation.Simulation simulation)
        {
            //Place entity in correct chunk if in new position
            string chunkKey =
                simulation.World.ChunkFromPixelLocation((int) Position.X, (int) Position.Y)
                    ?.ChunkKey;

            if (chunkKey != ChunkKey && chunkKey != null)
            {
                simulation.World.MoveEntity(EntityId, simulation.World.ChunkIndex[ChunkKey],
                    simulation.World.ChunkIndex[chunkKey]);
                ChunkKey = chunkKey;
            }
            
            if (EntityType != EntityType.Player || ChunkKey == null) return;

            UpdateVisibleChunks(simulation);

        }
        
        #endregion

        public void AddComponent(EntityComponent component)
        {
            EntityComponents.Add(component);
        }
        
        #region UpdateVisibleChunks

        private void UpdateVisibleChunks(Simulation.Simulation simulation)
        {
            //Clear visible chunks so we dont have to figure out which chunks are no longer being seen
            VisibleChunks.Clear();
            Chunk rootChunk = simulation.World.ChunkIndex[ChunkKey];

            //find the chunks that the player is currently simulating
            for (int y = rootChunk.ChunkY - simulation.SimulationDistance;
                y < rootChunk.ChunkY + simulation.SimulationDistance;
                y++)
            {
                for (int x = rootChunk.ChunkX - simulation.SimulationDistance;
                    x < rootChunk.ChunkX + simulation.SimulationDistance;
                    x++)
                {
                    //Get a chunk if it exist at the location
                    Chunk visibleChunk;
                    if ((visibleChunk = simulation.World.ChunkFromChunkLocation(x, y)) == null)
                        continue;

                    //Add chunk to visible chunks
                    VisibleChunks.Add(visibleChunk.ChunkKey);

                    //update chunk history if new with a negative number so a full chunk update will be forced
                    if (!ChunkVersions.ContainsKey(visibleChunk.ChunkKey))
                        ChunkVersions[visibleChunk.ChunkKey] = -1;
                }
            }
        }
        
        #endregion
        
        #region EmitOnEntityCollision

        public bool EmitOnEntityCollision(Simulation.Simulation simulation, SimulationEntity otherEntity)
        {
            bool defaultAction = true;
            
            foreach (EntityComponent simulationComponent in EntityComponents)
            {
                if(!simulationComponent.OnEntityCollision(simulation, this, otherEntity))
                    defaultAction = false;
            }

            return defaultAction;
        }
        
        #endregion
        
        #region EmitOnWorldBoundaryCollision

        public bool EmitOnWorldBoundaryCollision(Simulation.Simulation simulation, CollisionSide side)
        {
            bool defaultAction = true;
            
            foreach (EntityComponent simulationComponent in EntityComponents)
                if (!simulationComponent.OnWorldBoundaryCollision(simulation, this, side))
                    defaultAction = false;

            return defaultAction;
        }
        
        #endregion

        #region EmitOnTileCollision

        public bool EmitOnTileCollision(Simulation.Simulation simulation, TileCollision tc)
        {
            bool defaultAction = true;

            foreach (EntityComponent simulationComponent in EntityComponents)
            {
                if (!simulationComponent.OnTileCollision(simulation, this, tc))
                    defaultAction = false;
            }

            return defaultAction;
        }
        
        #endregion
        
        #region EmitOnSimulationRemoval

        public void EmitOnSimulationRemoval(Simulation.Simulation simulation, EntityRemovalReason? reason)
        {
            foreach (EntityComponent simulationComponent in EntityComponents)
                simulationComponent?.OnSimulationRemove(simulation,this, reason);
        }
        
        #endregion
        
        #region SimulationEntityFactory

        public static SimulationEntity SimulationEntityFactory(string entityKey, ClientSocket socket = null)
        {
            Random rand = new Random();
            
            ConfigurationRegistry registry = ConfigurationRegistry.GetInstance();
            EntityConfiguration config = registry.GetEntityConfiguration(entityKey);
            config.EntityKey = entityKey;

            //create new entity
            SimulationEntity newEntity = new SimulationEntity(socket?.ClientId ?? Guid.NewGuid().ToString(),
                new EntityOptions
                {
                    TextureKey = config.TextureKey,
                    Dimensions = config.DimensionRange == null  ? config.Dimensions  : new BfbVector(
                                                                                            rand.Next((int)config.DimensionRange.X,(int)config.DimensionRange.Y),
                                                                                            rand.Next((int)config.DimensionRange.X,(int)config.DimensionRange.Y)),
                    Origin = config.Origin,
                    EntityType = config.EntityType
                },socket)
            {
                CollideFilter = config.CollideFilter,
                CollideWithFilters = config.CollideWithFilters,
                Meta = new EntityMeta
                {
                    Health = config.Health,
                    Mana = config.Mana,
                    MaxHealth = config.Health,
                    MaxMana = config.Mana
                },
                EntityConfiguration = config
            };
            
            //Required components
            newEntity.EntityComponents.Add(new EntityFacing());
            
            //Add a lifetime component if needed
            if(config.Lifetime > 0)
                newEntity.EntityComponents.Add(new LifetimeComponent(config.Lifetime));

            //Add components
            foreach (string configComponent in config.Components)
                newEntity.EntityComponents.Add(registry.GetEntityComponent(configComponent));
            
            if(newEntity.EntityType == EntityType.Mob || newEntity.EntityType == EntityType.Player)
                newEntity.EntityComponents.Add(new ManaRegen());
                

            if (socket == null)//We can assume if a socket is supplied then its a player
                return newEntity;
            
            newEntity.EntityType = EntityType.Player;
            newEntity.EntityComponents.Add(new InputRemote());
            newEntity.EntityComponents.Add(new InventoryComponent());
            newEntity.EntityComponents.Add(new InventoryConnection());
            newEntity.EntityComponents.Add(new AnimatedHolding());

            return newEntity;
        }
        
        #endregion
    }
}
