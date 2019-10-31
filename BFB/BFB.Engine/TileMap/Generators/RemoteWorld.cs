namespace BFB.Engine.TileMap.Generators
{
    public class RemoteWorld : WorldGenerator
    {
        public RemoteWorld(WorldOptions options) : base(options)  { }

        public override Chunk GenerateChunk(int chunkX, int chunkY)
        {
            return new Chunk(WorldOptions.ChunkSize, chunkX,chunkY);
        }
    }
}