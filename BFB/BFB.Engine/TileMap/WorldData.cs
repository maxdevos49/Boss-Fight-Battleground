namespace BFB.Engine.TileMap
{
    public class WorldData
    {
        public WorldOptions WorldConfig { get; set; }

        public Chunk[,] Chunks { get; set; }
    }
}