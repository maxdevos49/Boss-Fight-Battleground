using System;
using BFB.Engine.TileMap.Generators;

namespace BFB.Engine.TileMap
{
    public class WorldOptions
    {
        public int Seed { get; set; }
        public int ChunkSize { get; set; }
        
        public int WorldChunkWidth { get; set; }

        public int WorldChunkHeight { get; set; }
        
        public Func<WorldOptions, WorldGenerator> WorldGenerator { get; set; }
    }
}