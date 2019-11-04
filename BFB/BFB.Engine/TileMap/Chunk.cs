using System;
using System.Collections.Generic;
using System.Linq;
using BFB.Engine.Entity;

namespace BFB.Engine.TileMap
{
    public class Chunk
    {
        #region Properties
        
        public string ChunkKey { get; set; }
        public readonly int ChunkX;
        public readonly int ChunkY;
        
        public readonly int ChunkSize;
        
        public int ChunkVersion { get; private set; }
        
        public ushort[,] Hardness { get; }
        public byte[,] Light { get; set; }
        public ushort[,] Wall { get; set;  }
        public  ushort[,] Block { get; set; }
        
        public Dictionary<string,SimulationEntity> Entities { get; }

        private readonly Dictionary<int, TileUpdate> _tileHistory;
        private readonly int _tileHistorySize;

        #endregion

        #region Constructor

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

        public void ApplyBlockUpdate(TileUpdate tileUpdate, bool doNotRecord = false)
        {

            //If the block is already the same lets not record it
            if (tileUpdate.Mode)
            {
                if (Block[tileUpdate.X, tileUpdate.Y] == tileUpdate.TileValue)
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

        public bool NeedChunkTileUpdate(int playerChunkVersion)
        {
            return ChunkVersion - _tileHistorySize <= playerChunkVersion;
        }
        
        #endregion

        #region GetChunkTileUpdate

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

        public bool NeedChunkUpdate(int playerChunkVersion)
        {
            return ChunkVersion - _tileHistorySize > playerChunkVersion || playerChunkVersion < 0;
        }
        
        #endregion

        #region GetChunkUpdate

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
