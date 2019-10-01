using System;
using System.Collections.Generic;
using System.Text;

namespace BFB.Engine.TileMap
{
    public class Chunk
    {
        private const int CHUNKSIZE = 16;

        private int[,] hardness;
        private int[,] light;
        private int[,] wall;
        private int[,] block;

        public Chunk()
        {
            hardness = new int[CHUNKSIZE, CHUNKSIZE];
            light = new int[CHUNKSIZE, CHUNKSIZE];
            wall = new int[CHUNKSIZE, CHUNKSIZE];
            block = new int[CHUNKSIZE, CHUNKSIZE];

            for (int x = 0; x < CHUNKSIZE; x++)
            {
                for (int y = 0; y < CHUNKSIZE; y++)
                {
                    hardness[x, y] = 0;
                    light[x, y] = 0;
                    wall[x, y] = 0;
                    block[x, y] = 0;
                }
            }
        }

        #region "get" methods

        public int getTileHardness(int x, int y)
        {
            return hardness[x, y];
        }

        public int getTileLight(int x, int y)
        {
            return light[x, y];
        }

        public int getTileWall(int x, int y)
        {
            return wall[x, y];
        }

        public int getTileBlock(int x, int y)
        {
            return block[x, y];
        }

        #endregion

        #region "set" methods

        public void setTileHardness(int x, int y, int hardnessValue)
        {
            hardness[x, y] = hardnessValue;
        }

        public void setTileLight(int x, int y, int lightValue)
        {
            light[x, y] = lightValue;
        }

        public void setTileWall(int x, int y, int wallValue)
        {
            wall[x, y] = wallValue;
        }

        public void setTileBlock(int x, int y, int blockValue)
        {
            block[x, y] = blockValue;
        }

        #endregion
    }
}
