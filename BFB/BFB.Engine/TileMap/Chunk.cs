using System;
using System.Collections.Generic;
using System.Text;

namespace BFB.Engine.TileMap
{
    public class Chunk
    {
        public readonly int ChunkSize;
        private readonly int[,] _hardness;
        private readonly int[,] _light;
        private readonly int[,] _wall;
        private readonly int[,] _block;

        public Chunk(int chunkSize)
        {
            ChunkSize = chunkSize;
            _hardness = new int[ChunkSize, ChunkSize];
            _light = new int[ChunkSize, ChunkSize];
            _wall = new int[ChunkSize, ChunkSize];
            _block = new int[ChunkSize, ChunkSize];
        }

        #region GetTileHardness

        public int GetTileHardness(int x, int y)
        {
            return _hardness[x, y];
        }
        
        #endregion

        #region GetTileLight
        
        public int GetTileLight(int x, int y)
        {
            return _light[x, y];
        }
        
        #endregion
        
        #region GetTileWall

        public int GetTileWall(int x, int y)
        {
            return _wall[x, y];
        }
        
        #endregion

        #region GetTileBlock
        
        public WorldTile GetTileBlock(int x, int y)
        {
            return (WorldTile)_block[x,y];
        }

        #endregion

        #region SetTileHardness

        public void SetTileHardness(int x, int y, int hardnessValue)
        {
            _hardness[x, y] = hardnessValue;
        }
        
        #endregion

        #region SetTileLight

        public void SetTileLight(int x, int y, int lightValue)
        {
            _light[x, y] = lightValue;
        }

        #endregion
        
        #region SetTileWall
        
        public void SetTileWall(int x, int y, int wallValue)
        {
            _wall[x, y] = wallValue;
        }
        
        #endregion

        #region SetTileBlock
        
        public void SetTileBlock(int x, int y, WorldTile blockValue)
        {
            _block[x, y] = (int)blockValue;
        }

        #endregion
    }
}
