using System.Collections.Generic;
using BFB.Engine.TileMap;
using JetBrains.Annotations;

namespace BFB.Engine.Inventory.Configuration
{
    [UsedImplicitly]
    public class ItemConfiguration
    {
        public string TextureKey { get; set; }
        
        public int StackLimit { get; set; }
        
        public int CoolDown { get; set; }
        
        public List<string> LeftHoldComponents { get; set; }
        
        public List<string> LeftClickComponents { get; set; }
        
        public List<string> RightHoldComponents { get; set; }
        
        public List<string> RightClickComponents { get; set; }
        
        public ItemType ItemType { get; set; }
        
        public WorldTile TileKey { get; set; }
        
    }
}