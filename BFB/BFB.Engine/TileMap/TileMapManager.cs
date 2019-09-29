using System;
using System.Collections.Generic;
using System.Text;

using BFB.Engine.TileMap;

namespace BFB.Engine.TileMap
{

    public class TileMapManager
    {
        //Tile Map will be 30x20 Chunks
        private const int XCHUNKS = 30;
        private const int YCHUNKS = 20;
        private const int CHUNKSIZE = 16;
        private Chunk[,] myChunk;

        private int chunkX;
        private int chunkY;
        private int extraX;
        private int extraY;


        public TileMapManager()
        {
            myChunk = new Chunk[XCHUNKS, YCHUNKS];
            chunkX = 0;
            chunkY = 0;
            extraX = 0;
            extraY = 0;

            int x;
            int y;
            for (x = 0; x < XCHUNKS; x++)
            {
                for (y = 0; y < YCHUNKS; y++)
                {
                    myChunk[x, y] = new Chunk();
                }
            }
        }



        public void getChunkInfo(int x, int y)
        {
            extraX = x % CHUNKSIZE;
            extraY = y % CHUNKSIZE;
            chunkX = x / CHUNKSIZE;
            chunkY = y / CHUNKSIZE;
        }

        #region getTile 
        public int getTileHardness(int x, int y)
        {
            getChunkInfo(x, y);
            return myChunk[chunkX,chunkY].getHardness(extraX, extraY);
        }

        public int getTileLight(int x, int y)
        {
            getChunkInfo(x, y);
            return myChunk[chunkX, chunkY].getLight(extraX, extraY);
        }

        public int getTileWall(int x, int y)
        {
            getChunkInfo(x, y);
            return myChunk[chunkX, chunkY].getWall(extraX, extraY);
        }

        public int getTileBlock(int x, int y)
        {
            getChunkInfo(x, y);
            return myChunk[chunkX, chunkY].getBlock(extraX, extraY);
        }

        #endregion

        #region setTile
        public void setTileHardness(int x, int y, int hardnessValue)
        {
            getChunkInfo(x, y);
            myChunk[chunkX, chunkY].setHardness(extraX, extraY, hardnessValue);
        }

        public void setTileLight(int x, int y, int lightValue)
        {
            getChunkInfo(x, y);
            myChunk[chunkX, chunkY].setLight(extraX, extraY, lightValue);
        }

        public void setTileWall(int x, int y, int wallValue)
        {
            getChunkInfo(x, y);
            myChunk[chunkX, chunkY].setWall(extraX, extraY, wallValue);
        }

        public void setTileBlock(int x, int y, int blockValue)
        {
            getChunkInfo(x, y);
            myChunk[chunkX, chunkY].setBlock(extraX, extraY, blockValue);
        }

        public void setTileAll(int x, int y, int hardnessValue, int lightValue, int wallValue, int blockValue)
        {
            getChunkInfo(x, y);
            myChunk[chunkX, chunkY].setHardness(extraX, extraY, hardnessValue);
            myChunk[chunkX, chunkY].setLight(extraX, extraY, lightValue);
            myChunk[chunkX, chunkY].setWall(extraX, extraY, wallValue);
            myChunk[chunkX, chunkY].setBlock(extraX, extraY, blockValue);
        }

        #endregion

    }
}
