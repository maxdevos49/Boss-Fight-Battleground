using System;
using System.Collections.Generic;

namespace BFB.Engine.TileMap.TileComponent
{
    public static class TileComponentManager
    {
        private static Dictionary<WorldTile, ITileComponent> _components;

        public static void LoadTileComponents()
        {
            _components = new Dictionary<WorldTile, ITileComponent>
            {
                {WorldTile.Grass, new SpreadTileComponent(WorldTile.Grass, WorldTile.Dirt, WorldTile.Air, 255)},
                {WorldTile.Stone, new SpreadTileComponent(WorldTile.Stone, WorldTile.Dirt,randomness:255)},
                {WorldTile.Dirt, new SpreadTileComponent(WorldTile.Dirt, WorldTile.Stone,randomness:255)},
            };

            //TODO load json configuration for block components
        }

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