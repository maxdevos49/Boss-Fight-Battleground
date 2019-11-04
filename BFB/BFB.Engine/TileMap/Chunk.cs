using System;
using System.Collections.Generic;
using System.Linq;
using BFB.Engine.Entity;

namespace BFB.Engine.TileMap
{
    public class Chunk
    {
        #region Properties
        
        /// <summary>
        /// The key of the chunk.
        /// </summary>
        public string ChunkKey { get; set; }
        /// <summary>
        /// The x of a chunk.
        /// </summary>
        public readonly int ChunkX;
        /// <summary>
        /// The y of a chunk.
        /// </summary>
        public readonly int ChunkY;
        /// <summary>
        /// The size of a chunk.
        /// </summary>
        public readonly int ChunkSize;
        /// <summary>
        /// The chunk version of a chunk.
        /// </summary>
        public int ChunkVersion { get; private set; }
        /// <summary>
        /// The hardness value of a chunk (x,y) location.
        /// </summary>
        public ushort[,] Hardness { get; }
        /// <summary>
        /// The light value of a chunk (x,y) location.
        /// </summary>
        public byte[,] Light { get; set; }
        /// <summary>
        /// The wall value of a chunk (x,y) location.
        /// </summary>
        public ushort[,] Wall { get; set;  }
        /// <summary>
        /// The block value of a chunk (x,y) location.
        /// </summary>
        public  ushort[,] Block { get; set; }
        /// <summary>
        /// The dictionary of the chunk.
        /// </summary>
        public Dictionary<string,SimulationEntity> Entities { get; }

        private readonly Dictionary<int, TileUpdate> _tileHistory;
        private readonly int _tileHistorySize;

        #endregion

        #region Constructor

        /// <summary>
        /// The chunk constructor.
        /// </summary>
        /// <param name="chunkSize">The width and height of the chunk.</param>
        /// <param name="chunkX">The X block location of the chunk.</param>
        /// <param name="chunkY">The Y block location of the chunk.</param>
        public Chunk(int chunkSize, int chunkX, int chunkY)
        {
            //Chunk Meta data
            ChunkKey = Guid.NewGuid().ToString();
            ChunkSize = chunkSize;
            ChunkX = chunkX/ChunkSize;
            ChunkY = chunkY/ChunkSize;
            
            //Tile information
            Wall = new ushort[ChunkSize, ChunkSize];
            Block = new ushort[ChunkSize, ChunkSize];
            
            Hardness = new ushort[ChunkSize, ChunkSize];
            Light = new byte[ChunkSize, ChunkSize];
            
            //Entities
            Entities = new Dictionary<string, SimulationEntity>();
            
            ChunkVersion = 0;
            _tileHistorySize = 30;
            _tileHistory = new Dictionary<int, TileUpdate>();
        }
        
        #endregion
        
        #region ApplyBlockUpdate

        /// <summary>
        /// Applies a block update.
        /// </summary>
        /// <param name="tileUpdate">The given tile update.</param>
        /// <param name="doNotRecord">Boolean on if the block update should be recorded.</param>
        public void ApplyBlockUpdate(TileUpdate tileUpdate, bool doNotRecord = false)
        {

            if (tileUpdate.Mode)
                Block[tileUpdate.X, tileUpdate.Y] = tileUpdate.TileValue;
            else
                Wall[tileUpdate.X, tileUpdate.Y] = tileUpdate.TileValue;

            if (doNotRecord) return; //For on client
            
            _tileHistory.Add(ChunkVersion, tileUpdate);
            
            ChunkVersion++;
            
            if (_tileHistory.Count > _tileHistorySize)
                _tileHistory.Remove(ChunkVersion - _tileHistorySize);//Remove oldest history
        }
        
        #endregion
        
        #region NeedChunkTileUpdate

        /// <summary>
        /// Checks if a chunk tile update is needed.
        /// </summary>
        /// <param name="playerChunkVersion">The given player chunk version.</param>
        /// <returns>Returns true if a chunk tile update is needed.</returns>
        public bool NeedChunkTileUpdate(int playerChunkVersion)
        {
            return ChunkVersion - _tileHistorySize <= playerChunkVersion;
        }
        
        #endregion

        #region GetChunkTileUpdate

        /// <summary>
        /// Retrieves the chunk tile updates.
        /// </summary>
        /// <param name="playerChunkVersion">The given player chunk version.</param>
        /// <returns>Returns the chunk tile updates.</returns>
        public ChunkTileUpdates GetChunkTileUpdates(int playerChunkVersion)
        {
            ChunkTileUpdates chunkTileUpdates = new ChunkTileUpdates
            {
                ChunkKey = ChunkKey,
                ChunkX = ChunkX,
                ChunkY = ChunkY,
                TileChanges = new List<TileUpdate>()
            };

            foreach ((int _, TileUpdate update) in _tileHistory.Where(x => x.Key < playerChunkVersion))
            {
                chunkTileUpdates.TileChanges.Add(update);
            }

            return chunkTileUpdates;
        }
        
        #endregion
        
        #region NeedChunkUpdate
        /// <summary>
        /// Checks if a chunk update is needed.
        /// </summary>
        /// <param name="playerChunkVersion">The given player chunk version.</param>
        /// <returns>Returns true if a chunk update is needed.</returns>
        public bool NeedChunkUpdate(int playerChunkVersion)
        {
            return ChunkVersion - _tileHistorySize > playerChunkVersion || playerChunkVersion < 0;
        }
        
        #endregion

        #region GetChunkUpdate

        /// <summary>
        /// Retrieves the chunk update.
        /// </summary>
        /// <returns>Returns the chunkUpdate.</returns>
        public ChunkUpdate GetChunkUpdate()
        {
            return new ChunkUpdate
            {
                ChunkKey = ChunkKey,
                ChunkX = ChunkX,
                ChunkY = ChunkY,
                Block = Block,
                Wall = Wall,
            };
        }
        
        #endregion
        
    }
    
}
