using System;
using BFB.Engine.TileMap.Generators;

namespace BFB.Engine.Server.Communication
{
    [Serializable]
    public class WorldDataMessage : DataMessage
    {
        public int ChunkSize { get; set; }
        
        public int WorldChunkWidth { get; set; }

        public int WorldChunkHeight { get; set; }
     
        public int WorldScale { get; set; }
        
        public string[,] ChunkMapIds { get; set; }
    }
}