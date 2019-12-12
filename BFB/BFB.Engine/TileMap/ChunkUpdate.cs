using System;

namespace BFB.Engine.TileMap
{
    /// <summary>
    /// The update of a chunk.
    /// </summary>
    [Serializable]
    public class ChunkUpdate
    {
        /// <summary>
        /// The key of a chunk.
        /// </summary>
        public string ChunkKey { get; set; }
        /// <summary>
        /// The X location of a chunk.
        /// </summary>
        public int ChunkX { get; set; }
        /// <summary>
        /// The Y location of a chunk.
        /// </summary>
        public int ChunkY { get; set; }
        /// <summary>
        /// The Wall value of the block.
        /// </summary>
        public ushort[,] Wall { get; set; }
        /// <summary>
        /// The Block value of the block.
        /// </summary>
        public  ushort[,] Block { get; set; }
    }
}