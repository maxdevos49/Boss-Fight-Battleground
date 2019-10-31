using System.Collections.Generic;
using BFB.Engine.Math;
using BFB.Engine.Simulation.InputComponents;
using BFB.Engine.Simulation.PhysicsComponents;
using BFB.Engine.TileMap;

namespace BFB.Engine.Entity
{
    public class SimulationEntity : Entity
    {
        
        #region Properties
        
        /**
         * Indicates the chunk that the entity is currently in
         */
        public string ChunkKey { get; set; }
        
        public bool IsPlayer { get; set; }
        
        public BfbVector DesiredVector { get; }
        
        /**
         * Indicates the chunks that the client can see. Only used if IsPlayer equals true
         */
        public List<string> VisibleChunks { get; }
        
        /**
         * Used to determine if the chunk needs to be sent to the client
         */
        public Dictionary<string, int> ChunkVersions { get; }
        
        #endregion
        
        #region Components
        
        private readonly IInputComponent _input;
        private readonly IPhysicsComponent _physics;
        
        #endregion

        #region Constructor
        
        public SimulationEntity(string entityId, EntityOptions options, ComponentOptions components) : base(entityId, options)
        {
            //Components
            _input = components.Input;
            _physics = components.Physics;
            
            DesiredVector = new BfbVector();
            VisibleChunks = new List<string>();
            ChunkVersions = new Dictionary<string, int>();
        }
        
        #endregion

        #region Update
        
        public void Tick(Simulation.Simulation simulation)
        {
            
            //Component Processing
            _input?.Update(this, simulation);
            _physics?.Update(this, simulation);
            
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
    
    public class ComponentOptions
    {
        public IInputComponent Input { get; set; }
        public IPhysicsComponent Physics { get; set; }
    }
    
    #endregion
    
}