using System;
using System.Collections.Generic;

namespace BFB.Engine.TileMap
{
    

    /// <summary>
    /// Holds a collection of individual tile updates for a specific chunk
    /// </summary>
    [Serializable]
    public class ChunkTileUpdates
    {
        /// <summary>
        /// The key of the chunk.
        /// </summary>
        public string ChunkKey { get; set; }
        /// <summary>
        /// The X location of the chunk.
        /// </summary>
        public int ChunkX { get; set; }
        /// <summary>
        /// The Y location of the chunk.
        /// </summary>
        public int ChunkY { get; set; }
        
        public List<TileUpdate> TileChanges { get; set; }
    }


    /// <summary>
    /// Represents a tile update that can be used to update the tile map inside a specific chunk.
    /// </summary>
    [Serializable]
    public class TileUpdate
    {
<<<<<<< HEAD

=======
        /// <summary>
        /// The X location of the tile.
        /// </summary>
>>>>>>> 5d8a6aa64e8cd5934e74929028a6f2a776a530a2
        public byte X { get; set; }
        /// <summary>
        /// The Y location of the tile.
        /// </summary>
        public byte Y { get; set; }
        
   
        /// <summary>
        /// True if it is a block, False if it is a wall.
        /// </summary>
        public bool Mode { get; set; }
        /// <summary>
        /// The value of a tile.
        /// </summary>
        public ushort TileValue { get; set; }
        
    }
}