using System;

namespace BFB.Engine.TileMap
{
    [Serializable]
    public class ChunkUpdate
    {
        public string ChunkKey { get; set; }

        public int ChunkX { get; set; }

        public int ChunkY { get; set; }
        
        public ushort[,] Wall { get; set; }
        
        public  ushort[,] Block { get; set; }
    }
}