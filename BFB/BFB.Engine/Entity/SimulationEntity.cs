using System.Collections.Generic;
using BFB.Engine.Math;
using BFB.Engine.Simulation.InputComponents;
using BFB.Engine.Simulation.PhysicsComponents;
using BFB.Engine.TileMap;

namespace BFB.Engine.Entity
{
    /// <summary>
    /// An entity stored in the game server's simulation 
    /// </summary>
    public class SimulationEntity : Entity
    {
        
        #region Properties
        
        /// <summary>
        /// Indicates the chunk that the entity is currently in
        /// </summary>
        public string ChunkKey { get; set; }
        
        /// <summary>
        /// Whether this is a player entity or not
        /// </summary>
        public bool IsPlayer { get; set; }
        
        /// <summary>
        /// Vector describing a position an entity is attempting to move to 
        /// </summary>
        public BfbVector DesiredVector { get; }
        
        /// <summary>
        /// Indicates the chunks that the client can see. Only used if IsPlayer equals true
        /// </summary>
        public List<string> VisibleChunks { get; }
        
        /// <summary>
        /// Used to determine if the chunk needs to be sent to the client
        /// </summary>
        public Dictionary<string, int> ChunkVersions { get; }
        
        #endregion
        
        #region Components
        
        private readonly IInputComponent _input;
        private readonly IPhysicsComponent _physics;
        private readonly IPhysicsComponent _combat;

        #endregion

        #region Constructor
        
        /// <summary>
        /// Creates a new entity for the game simulations
        /// </summary>
        /// <param name="entityId">Unique ID for this entity</param>
        /// <param name="options">Sets the initial properties of this entity</param>
        /// <param name="components">The components this entity contains</param>
        public SimulationEntity(string entityId, EntityOptions options, ComponentOptions components) : base(entityId, options)
        {
            //Components
            _input = components.Input;
            _physics = components.Physics;
            _combat = components.Combat;
            
            DesiredVector = new BfbVector();
            VisibleChunks = new List<string>();
            ChunkVersions = new Dictionary<string, int>();
        }
        
        #endregion

        #region Update
        
        /// <summary>
        /// Updates this entity as part of the chunks in the simulation of this entity
        /// </summary>
        /// <param name="simulation"></param>
        public void Tick(Simulation.Simulation simulation)
        {
            
            //Component Processing
            _input?.Update(this, simulation);
            _physics?.Update(this, simulation);
            _combat?.Update(this, simulation);
            
            //Place entity in correct chunk if in new position
            string chunkKey = simulation.WorldManager.ChunkFromPixelLocation((int)Position.X, (int)Position.Y)?.ChunkKey; //If this is null then we are outside of map... Bad
            
            if (chunkKey != ChunkKey && chunkKey != null)
            {
                simulation.WorldManager.MoveEntity(EntityId, simulation.WorldManager.ChunkIndex[ChunkKey], simulation.WorldManager.ChunkIndex[chunkKey]);
                ChunkKey = chunkKey;
            }

            if (!IsPlayer || ChunkKey == null) return;
            
            //Clear visible chunks so we dont have to figure out which chunks are no longer being seen
            VisibleChunks.Clear();
            Chunk rootChunk = simulation.WorldManager.ChunkIndex[ChunkKey];
            
            //find the chunks that the player is currently simulating
            for (int y = rootChunk.ChunkY - simulation.SimulationDistance; y < rootChunk.ChunkY + simulation.SimulationDistance; y++)
            {
                for (int x = rootChunk.ChunkX - simulation.SimulationDistance; x < rootChunk.ChunkX + simulation.SimulationDistance; x++)
                {
                    //Get a chunk if it exist at the location
                    Chunk visibleChunk;
                    if ((visibleChunk = simulation.WorldManager.ChunkFromChunkLocation(x, y)) == null) 
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
        
    }
    
    #region ComponentOptions
    
    /// <summary>
    /// Options object used for creating a component
    /// </summary>
    public class ComponentOptions
    {
        /// <summary>
        /// Input component to be applied
        /// </summary>
        public IInputComponent Input { get; set; }
        /// <summary>
        /// Physics component to be applied
        /// </summary>
        public IPhysicsComponent Physics { get; set; }

        public IPhysicsComponent Combat { get; set; }
    }
    
    #endregion
    
}