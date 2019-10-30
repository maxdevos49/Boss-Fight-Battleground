using System;
using System.Collections.Generic;
using BFB.Engine.Server.Communication;

namespace BFB.Engine.TileMap
{
    
    /**
     * Holds a collection of individual tile updates for a specific chunk
     */
    [Serializable]
    public class ChunkTileUpdates
    {
        public string ChunkKey { get; set; }
        
        public int ChunkX { get; set; }
        
        public int ChunkY { get; set; }
        
        public List<TileUpdate> TileChanges { get; set; }
    }

    /**
     * Represents a tile update that can be used to update the tile map inside a specific chunk
     */
    [Serializable]
    public class TileUpdate
    {
        public byte X { get; set; }
        public byte Y { get; set; }
        
        //True = block, False = wall
        public bool Mode { get; set; }

        public ushort TileValue { get; set; }
        
    }
}