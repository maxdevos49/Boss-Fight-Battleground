namespace BFB.Engine.Inventory
{
    public class InventorySlot
    {
        public int Version { get; set; }
        
        /// <summary>
        /// True means remove items at the slot. False means update
        /// </summary>
        public bool Mode { get; set; }
        
        public ushort SlotId { get; set; }
        public string TextureKey { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }

        public byte Count { get; set; }
    }
}