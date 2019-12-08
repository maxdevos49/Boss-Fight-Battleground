using System.Collections.Generic;
using BFB.Engine.Server.Communication;

namespace BFB.Engine.Inventory
{
    public class ClientInventory
    {
        private readonly object _lock;
        
        public int ActiveSlot { get; set; }

        private Dictionary<int, InventorySlot> Slots { get; set; }

        public ClientInventory()
        {
            _lock = new object();
            Slots = new Dictionary<int, InventorySlot>();
        }

        
        public void AddItem(int slotId, InventorySlot slot)//FOR DEBUG
        {
            lock (_lock)
            {
                Slots.Add(slotId, slot);
            }
        }

        #region GetSlots
        
        public Dictionary<int, InventorySlot> GetSlots()
        {
            lock (_lock)
            {
                return new Dictionary<int, InventorySlot>(Slots);
            }
        }
        
        #endregion
        
        #region ApplySlotUpdates

        public void ApplySlotUpdates(InventorySlotMessage sm)
        {
            lock (_lock)
            {
                foreach (InventorySlot slot in sm.SlotUpdates)
                {
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
            
        }
        
        #endregion
        
    }
}