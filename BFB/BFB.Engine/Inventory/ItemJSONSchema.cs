using System.Collections.Generic;
using BFB.Engine.TileMap;

namespace BFB.Engine.Inventory
{
    public class ItemJSONSchema
    {
        public Dictionary<string, ItemConfiguration> Items { get; set; }
        
        public Dictionary<WorldTile, BlockConfiguration> Blocks { get; set; }
        
        public Dictionary<WorldTile, WallConfiguration> Walls { get; set; }
    }
}