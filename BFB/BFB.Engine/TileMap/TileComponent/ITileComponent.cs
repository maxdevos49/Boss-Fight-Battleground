namespace BFB.Engine.TileMap.TileComponent
{
    /// <summary>
    /// The interface for the 
    /// </summary>
    public interface ITileComponent
    {
         void TickTile(WorldManager worldManager,Chunk chunk, int blockX, int blockY);
    }
}