using System.Collections.Generic;
using BFB.Engine.Entity.InputComponents;
using BFB.Engine.Entity.PhysicsComponents;
using BFB.Engine.Math;
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
        
        public void Tick(WorldManager worldManager, int simulationDistance)
        {
            
            //Component Processing
            _input?.Update(this);
            _physics?.Update(this, worldManager);
            
            //Place entity in correct chunk if in new position
            string chunkKey = worldManager.ChunkFromPixelLocation((int)Position.X, (int)Position.Y)?.ChunkKey; //If this is null then we are outside of map... Bad
            
            if (chunkKey != ChunkKey && chunkKey != null)
            {
                worldManager.MoveEntity(EntityId, worldManager.ChunkIndex[ChunkKey], worldManager.ChunkIndex[chunkKey]);
                ChunkKey = chunkKey;
            }

            if (!IsPlayer || ChunkKey == null) return;
            
            //Clear visible chunks so we dont have to figure out which chunks are no longer being seen
            VisibleChunks.Clear();
            Chunk rootChunk = worldManager.ChunkIndex[ChunkKey];
            
            //find the chunks that the player is currently simulating
            for (int y = rootChunk.ChunkY - simulationDistance; y < rootChunk.ChunkY + simulationDistance; y++)
            {
                for (int x = rootChunk.ChunkX - simulationDistance; x < rootChunk.ChunkX + simulationDistance; x++)
                {
                    //Get a chunk if it exist at the location
                    Chunk visibleChunk;
                    if ((visibleChunk = worldManager.ChunkFromChunkLocation(x, y)) == null) 
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