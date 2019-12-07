using System.Collections.Generic;
using BFB.Engine.Inventory;
using JetBrains.Annotations;

namespace BFB.Engine.Server.Communication
{
    public class InventorySlotMessage : DataMessage
    {
        [UsedImplicitly]
        public List<InventorySlot> SlotUpdates { get; set; }
    }
}