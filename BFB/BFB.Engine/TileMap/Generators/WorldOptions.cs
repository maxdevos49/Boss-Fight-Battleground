using System;
using Newtonsoft.Json;

namespace BFB.Engine.TileMap.Generators
{
    public class WorldOptions
    {
        public int Seed { get; set; }
        public int ChunkSize { get; set; }
        
        public int WorldChunkWidth { get; set; }

        public int WorldChunkHeight { get; set; }
        
        [JsonIgnore]
        public Func<WorldOptions, WorldGenerator> WorldGenerator { get; set; }
    }
}