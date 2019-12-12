using System;
using System.Collections.Generic;
using System.Linq;
using BFB.Engine.Entity;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

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
        public int ChunkX { get; set; }
        
        /// <summary>
        /// The y of a chunk.
        /// </summary>
        public int ChunkY { get; set; }
        
        /// <summary>
        /// The size of a chunk.
        /// </summary>
        public int ChunkSize { get; set; }
        
        /// <summary>
        /// The scale of the chunks tiles
        /// </summary>
        public int TileScale { get; set; }
        
        /// <summary>
        /// The chunk version of a chunk.
        /// </summary>
        [JsonIgnore]
        public int ChunkVersion { get; private set; }
        
        /// <summary>
        /// The light value of a chunk (x,y) location.
        /// </summary>
        [JsonIgnore]
        public byte[,] Light { get; set; }
        /// <summary>
        /// The wall value of a chunk (x,y) location.
        /// </summary>
        public ushort[,] Wall { get; set;  }
        /// <summary>
        /// The block value of a chunk (x,y) location.
        /// </summary>
        public  ushort[,] Block { get; set; }
        

        #region Pixel Position Helpers
        
        public Rectangle Bounds => new Rectangle(Left,Right,Width,Height);

        public int Width => ChunkSize * TileScale;
        public int Height => ChunkSize * TileScale;
        
        public int Left => ChunkX * TileScale;
        public int Right => ChunkX * TileScale + ChunkSize * TileScale;
        public int Top => ChunkY * TileScale;
        public int Bottom => ChunkY * TileScale + ChunkSize * TileScale;
        
        #endregion
        
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
        public Chunk(int chunkSize, int chunkX, int chunkY, int tileScale)
        {
            //Chunk Meta data
            ChunkKey = Guid.NewGuid().ToString();
            ChunkSize = chunkSize;
            ChunkX = chunkX/ChunkSize;
            ChunkY = chunkY/ChunkSize;
            TileScale = tileScale;
            
            //Tile information
            Wall = new ushort[ChunkSize, ChunkSize];
            Block = new ushort[ChunkSize, ChunkSize];
            
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

            //If the block is already the same lets not record it
            if (tileUpdate.Mode)
            {
                if (Block[tileUpdate.X, tileUpdate.Y] == tileUpdate.TileValue)//TODO hmmm stuff happens here
                    return;
            }
            else
            {
                if (Wall[tileUpdate.X, tileUpdate.Y] == tileUpdate.TileValue)
                    return;
            }

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

            foreach ((int _, TileUpdate update) in _tileHistory.Where(x => x.Key <= playerChunkVersion))//TODO possibly true when should not be
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
