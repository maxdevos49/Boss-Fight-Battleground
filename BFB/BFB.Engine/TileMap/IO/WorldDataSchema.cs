using BFB.Engine.TileMap.Generators;

namespace BFB.Engine.TileMap.IO
{
    public class WorldDataSchema
    {
        public WorldOptions WorldConfig { get; set; }

        public Chunk[,] Chunks { get; set; }
    }
}