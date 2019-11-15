using System.Collections.Generic;

namespace BFB.Engine.TileMap.TileComponent
{
    /// <summary>
    /// Used to manage the tile component.
    /// </summary>
    public static class TileComponentManager
    {
        /// <summary>
        /// The dictionary for the tilemap.
        /// </summary>
        private static Dictionary<WorldTile, ITileComponent> _components;

        /// <summary>
        /// How the tile components are loaded.
        /// </summary>
        public static void LoadTileComponents()
        {
            _components = new Dictionary<WorldTile, ITileComponent>
            {
                {WorldTile.Grass, new SpreadTileComponent(WorldTile.Grass, WorldTile.Dirt, WorldTile.Air, 70)},
//                {WorldTile.Stone, new SpreadTileComponent(WorldTile.Stone, WorldTile.Dirt,randomness:255)},
//                {WorldTile.Dirt, new SpreadTileComponent(WorldTile.Dirt, WorldTile.Stone,randomness:255)},
//                {WorldTile.Dirt, new SpreadTileComponent(WorldTile.Air, WorldTile.Dirt, WorldTile.Air)},
            };

            //TODO load json configuration for block components
        }

        /// <summary>
        /// The update for the tile component.
        /// </summary>
        /// <param name="worldManager">The manager for the world.</param>
        /// <param name="chunk">The chunk in a given world.</param>
        /// <param name="blockX">The x block in the given chunk.</param>
        /// <param name="blockY">The y block in the given chunk.</param>
        public static void TickTile(WorldManager worldManager, Chunk chunk, int blockX, int blockY)
        {

            if (_components == null)
                return;
            
            
            if (!_components.ContainsKey((WorldTile) chunk.Block[blockX, blockY]))
                return;
            
            _components[(WorldTile) chunk.Block[blockX, blockY]]?.TickTile(worldManager, chunk, blockX, blockY);
        }
    }
}