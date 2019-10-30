namespace BFB.Engine.TileMap.TileComponent
{
    public interface ITileComponent
    {
         void TickTile(Chunk chunk, int blockX, int blockY);
    }
}