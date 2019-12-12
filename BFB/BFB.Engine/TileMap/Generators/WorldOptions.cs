using System;
using Newtonsoft.Json;

namespace BFB.Engine.TileMap.Generators
{
    /// <summary>
    /// Used to know the options of the world.
    /// </summary>
    public class WorldOptions
    {
       
       /// <summary>
       /// The seed in order to seed the world generator.
       /// </summary>
        public int Seed { get; set; }
       
        /// <summary>
        /// The dimension of 1 square chunk. (How many tiles tall/wide it is)
        /// </summary>
        public int ChunkSize { get; set; }
        

        /// <summary>
        /// The width in chunks of the map.
        /// </summary>
        public int WorldChunkWidth { get; set; }
        
        /// <summary>
        /// The height in chunks of the map.
        /// </summary>
        public int WorldChunkHeight { get; set; }
        
        /// <summary>
        /// The scale in pixels of how large any given tile is.
        /// </summary>
        public int WorldScale { get; set; }
        
        /// <summary>
        /// The function for the world options into the world generator.
        /// </summary>
        [JsonIgnore]
        public Func<WorldOptions, WorldGenerator> WorldGenerator { get; set; }
    }
}