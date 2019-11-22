namespace BFB.Engine.TileMap.Generators
{
    /// <summary>
    /// Used to generate a world.
    /// </summary>
    public abstract class WorldGenerator
    {
        /// <summary>
        /// The options of the world.
        /// </summary>
        protected WorldOptions WorldOptions { get; }

        /// <summary>
        /// The generator of the world.
        /// </summary>
        /// <param name="options">The options used for the world generation.</param>
        protected WorldGenerator(WorldOptions options)
        {
            WorldOptions = options;
        }

        /// <summary>
        /// Generates the chunk at the given x,y block position.
        /// </summary>
        /// <param name="chunkX">The chunk X location with the given block X location.</param>
        /// <param name="chunkY">The chunk Y location with the given block Y location.</param>
        /// <param name="tileScale"></param>
        /// <returns>Returns the generated chuck.</returns>
        public abstract Chunk GenerateChunk(int chunkX, int chunkY, int tileScale);

    }
}