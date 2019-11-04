namespace BFB.Engine.TileMap.TileComponent
{
    /// <summary>
    /// The interface to implement the tile component.
    /// </summary>
    public interface ITileComponent
    {
        /// <summary>
        /// The update for the tile component.
        /// </summary>
        /// <param name="worldManager">The manager for the world.</param>
        /// <param name="chunk">The chunk in a given world.</param>
        /// <param name="blockX">The x block in the given chunk.</param>
        /// <param name="blockY">The y block in the given chunk.</param>
         void TickTile(WorldManager worldManager,Chunk chunk, int blockX, int blockY);
    }
}