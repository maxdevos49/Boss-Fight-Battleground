using System;

namespace BFB.Engine.Inventory
{
    [Serializable]
    public class InventorySlot
    {
        
        public byte SlotId { get; set; }
        
        public string TextureKey { get; set; }
        
        public string Name { get; set; }
        
        public byte Count { get; set; }
        
        public ItemType ItemType { get; set; }

        /// <summary>
        /// True means remove items at the slot. False means update
        /// </summary>
        public bool Mode { get; set; }
    }
}