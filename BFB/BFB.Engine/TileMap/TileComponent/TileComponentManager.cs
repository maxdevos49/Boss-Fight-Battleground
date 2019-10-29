using System.Collections.Generic;

namespace BFB.Engine.TileMap.TileComponent
{
    public static class TileComponentManager
    {
        private static Dictionary<WorldTile, ITileComponent> _components;

        public static void LoadTileComponents()
        {
            _components = new Dictionary<WorldTile, ITileComponent>();
            
            //TODO load json configuration for block components
        }

        public static void TickTile(Chunk chunk, int blockX, int blockY)
        {
            if (_components == null)
                return;
            
            if (!_components.ContainsKey((WorldTile) chunk.Block[blockX, blockY]))
                return;
            
            _components[(WorldTile) chunk.Block[blockX, blockY]]?.TickTile(chunk, blockX, blockY);
        }
    }
}