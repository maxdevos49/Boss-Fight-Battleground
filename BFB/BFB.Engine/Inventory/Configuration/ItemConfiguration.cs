using System.Collections.Generic;
using BFB.Engine.TileMap;
using JetBrains.Annotations;

namespace BFB.Engine.Inventory.Configuration
{
    [UsedImplicitly]
    public class ItemConfiguration
    {
        [UsedImplicitly]
        public string TextureKey { get; set; }
        
        [UsedImplicitly]
        public int StackLimit { get; set; }
        
        [UsedImplicitly]
        public int CoolDown { get; set; }
        
        [UsedImplicitly]
        public byte Reach { get; set; }
        
        [UsedImplicitly]
        public ushort ManaGain { get; set; }
        
        [UsedImplicitly]
        public ushort ManaCost { get; set; }
        
        [UsedImplicitly]
        public ushort HealthGain { get; set; }

        [UsedImplicitly]
        public ushort HealthCost { get; set; }
        
        [UsedImplicitly]
        public ushort Damage { get; set; }
        
        [UsedImplicitly]
        public List<string> LeftHoldComponents { get; set; }
        
        [UsedImplicitly]
        public List<string> LeftClickComponents { get; set; }
        
        [UsedImplicitly]
        public List<string> RightHoldComponents { get; set; }
        
        [UsedImplicitly]
        public List<string> RightClickComponents { get; set; }
        
        [UsedImplicitly]
        public ItemType ItemType { get; set; }
        
        [UsedImplicitly]
        public WorldTile TileKey { get; set; }
        
    }
}