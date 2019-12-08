using System;
using System.Collections.Generic;
using BFB.Engine.Inventory;
using JetBrains.Annotations;

namespace BFB.Engine.Server.Communication
{
    [Serializable]
    public class InventorySlotMessage : DataMessage
    {

        public InventorySlotMessage()
        {
            SlotUpdates = new List<InventorySlot>();
        }
        
        [UsedImplicitly]
        public List<InventorySlot> SlotUpdates { get; set; }
    }
}