using BFB.Engine.TileMap.Generators;

namespace BFB.Engine.TileMap.IO
{
    /// <summary>
    /// The schema for the world data.
    /// </summary>
    public class WorldDataSchema
    {
        /// <summary>
        /// The options to configure the world.
        /// </summary>
        public WorldOptions WorldConfig { get; set; }

        /// <summary>
        /// The chunks for the world.
        /// </summary>
        public Chunk[,] Chunks { get; set; }
    }
}