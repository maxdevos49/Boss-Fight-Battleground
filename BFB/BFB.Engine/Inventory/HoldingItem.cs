using System;

namespace BFB.Engine.Inventory
{
    [Serializable]
    public class HoldingItem
    {
        public string AtlasKey { get; set; }
        
        public ItemType ItemType { get; set; }
            
        public byte Reach { get; set; }
        
        public float Progress { get; set; }

        public HoldingItem()
        {
            ItemType = ItemType.Unknown;
        }
    }
}