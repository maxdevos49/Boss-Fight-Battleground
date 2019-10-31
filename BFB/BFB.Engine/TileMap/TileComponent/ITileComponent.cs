namespace BFB.Engine.TileMap.TileComponent
{
    public interface ITileComponent
    {
         void TickTile(WorldManager worldManager,Chunk chunk, int blockX, int blockY);
    }
}