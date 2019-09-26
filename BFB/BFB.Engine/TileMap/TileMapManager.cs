using System;
using System.Collections.Generic;
using System.Text;

using BFB.Engine.TileMap;

namespace BFB.Engine.TileMap
{

    class TileMapManager
    {

        const int XCHUNKS = 2;
        const int YCHUNKS = 2;
        const int CHUNKSIZE = 16;
        public Chunk[,] myChunk = new Chunk[XCHUNKS, YCHUNKS];
        int chunkNum = 0;

        int chunkX = 0;
        int chunkY = 0;
        int extraX = 0;
        int extraY = 0;


        public TileMapManager()
        {

        }

        public void getChunkInfo(int x, int y)
        {
            extraX = x % CHUNKSIZE;
            extraY = y % CHUNKSIZE;
            chunkX = x / CHUNKSIZE;
            chunkY = y / CHUNKSIZE;
        }

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

        public int getTilelight(int x, int y)
        {
            getChunkInfo(x, y);
            return myChunk[chunkX, chunkY].getLight(extraX, extraY);
        }

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

        public void getTileWall(int x, int y, int wallValue)
        {
            getChunkInfo(x, y);
            myChunk[chunkX, chunkY].setWall(extraX, extraY, wallValue);
        }

        public void getTilelight(int x, int y, int lightValue)
        {
            getChunkInfo(x, y);
            myChunk[chunkX, chunkY].setLight(extraX, extraY, lightValue);
        }


    }
}
