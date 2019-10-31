namespace BFB.Engine.TileMap.Generators
{
    public abstract class WorldGenerator
    {
        protected WorldOptions WorldOptions { get; }

        protected WorldGenerator(WorldOptions options)
        {
            WorldOptions = options;
        }
        
        public abstract Chunk GenerateChunk(int chunkX, int chunkY);

    }
}