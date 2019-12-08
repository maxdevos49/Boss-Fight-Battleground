using System.Collections.Generic;
using BFB.Engine.Server.Communication;

namespace BFB.Engine.Inventory
{
    public class ClientInventory
    {
        private readonly object _lock;
        
        public byte ActiveSlot { get; set; }

        private Dictionary<byte, InventorySlot> Slots { get; set; }

        public ClientInventory()
        {
            _lock = new object();
            Slots = new Dictionary<byte, InventorySlot>();
        }
        
        #region GetSlots
        
        public Dictionary<byte, InventorySlot> GetSlots()
        {
            return new Dictionary<byte, InventorySlot>(Slots);
        }
        
        #endregion
        
        #region ApplySlotUpdates

        public void ApplySlotUpdates(InventorySlotMessage sm)
        {
            foreach (InventorySlot slot in sm.SlotUpdates)
            {
                if(slot == null)
                    continue;
                
                if (Slots.ContainsKey(slot.SlotId))
                {
                    if (slot.Mode)//remove slot
                        Slots.Remove(slot.SlotId);
                    else//update slot
                        Slots[slot.SlotId] = slot;
                }
                else
                {
                    Slots.Add(slot.SlotId, slot);
                }
            }
            
        }
        
        #endregion
        
    }
}