using System;
using System.Collections.Generic;
using System.Linq;
using BFB.Engine.Entity;
using BFB.Engine.Server.Communication;

namespace BFB.Engine.TileMap
{
    public class Chunk
    {
        #region Properties
        
        public readonly string ChunkKey;
        public readonly int ChunkX;
        public readonly int ChunkY;
        
        public readonly int ChunkSize;
        
        public int ChunkVersion { get; private set; }
        
        public ushort[,] Hardness { get; }
        public byte[,] Light { get; }
        public ushort[,] Wall { get;}
        public  ushort[,] Block { get; }
        
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
        
        #region GetChunkUpdate

        /**
         * Generates a data message of either a Chunk update or a ChunkTileUpdate
         */
        public Tuple<ChunkUpdate,ChunkTileUpdates> GetChunkUpdate(int ownersOldVersion)
        {
            ChunkUpdate chunkUpdate = new ChunkUpdate();
            ChunkTileUpdates chunkTileUpdates = new ChunkTileUpdates();

            if (ownersOldVersion < ChunkVersion - _tileHistorySize)
            {
                chunkUpdate = new ChunkUpdate
                {
                    ChunkKey = ChunkKey,
                    ChunkX = ChunkX,
                    ChunkY = ChunkY,
                    Block = Block,
                    Wall = Wall,
                };
            }
            else
            {

                chunkTileUpdates = new ChunkTileUpdates
                {
                    ChunkKey = ChunkKey,
                    ChunkX = ChunkX,
                    ChunkY = ChunkY,
                    TileChanges = new List<TileUpdate>()
                };

                foreach ((int _, TileUpdate update) in _tileHistory.Where(x => x.Key > ownersOldVersion))
                {
                    chunkTileUpdates.TileChanges.Add(update);
                }
            }

            return new Tuple<ChunkUpdate, ChunkTileUpdates>(chunkUpdate,chunkTileUpdates);

        }
        
        #endregion
        
    }
    
}
