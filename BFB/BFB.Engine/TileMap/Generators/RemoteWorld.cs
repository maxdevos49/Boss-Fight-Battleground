namespace BFB.Engine.TileMap.Generators
{
    /// <summary>
    /// Used to generate a remote world.
    /// </summary>
    public class RemoteWorld : WorldGenerator
    {
        /// <summary>
        /// Generates a remote world.
        /// </summary>
        /// <param name="options">Options used to generate the world.</param>
        public RemoteWorld(WorldOptions options) : base(options)  { }

        /// <summary>
        /// Generates the chunk at the given x,y block position.
        /// </summary>
        /// <param name="chunkX">The chunk X location with the given block X location.</param>
        /// <param name="chunkY">The chunk Y location with the given block Y location.</param>
        /// <returns>Returns the generated chuck.</returns>
        public override Chunk GenerateChunk(int chunkX, int chunkY)
        {
            return new Chunk(WorldOptions.ChunkSize, chunkX,chunkY);
        }
    }
}