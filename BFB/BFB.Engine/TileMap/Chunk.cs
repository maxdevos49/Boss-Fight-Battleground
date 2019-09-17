using System;
using System.Collections.Generic;
using System.Text;

namespace BFB.Engine.TileMap
{
    class Chunk
    {
        private const int CHUNKSIZE = 16;

        private int[,] hardness = new int[CHUNKSIZE, CHUNKSIZE];

        private int[,] light = new int[CHUNKSIZE, CHUNKSIZE];

        private int[,] wall = new int[CHUNKSIZE, CHUNKSIZE];

        private int[,] block = new int[CHUNKSIZE, CHUNKSIZE];

        public Chunk()
        {

        }

        public int getHardness(int x, int y)
        {
            return hardness[x, y];
        }

        public int getLight(int x, int y)
        {
            return light[x, y];
        }

        public int getWall(int x, int y)
        {
            return wall[x, y];
        }

        public int getBlock(int x, int y)
        {
            return block[x, y];
        }
    }
}
