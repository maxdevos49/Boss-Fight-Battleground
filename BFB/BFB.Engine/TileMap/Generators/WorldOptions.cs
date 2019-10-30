using System;
using Newtonsoft.Json;

namespace BFB.Engine.TileMap.Generators
{
    public class WorldOptions
    {
       /**
        * Seed to seed the world generator
        */
        public int Seed { get; set; }
        
        /**
         * The dimension of 1 square chunk. (How many tiles tall/wide it is)
         */
        public int ChunkSize { get; set; }
        
        /**
         * The width in chunks of the map
         */
        public int WorldChunkWidth { get; set; }

        /**
         * The height in chunks of the map
         */
        public int WorldChunkHeight { get; set; }
        
        /**
         * The scale in pixels of how big a tile is.
         */
        public int WorldScale { get; set; }
        
        
        [JsonIgnore]
        public Func<WorldOptions, WorldGenerator> GetWorldGenerator { get; set; }
    }
}