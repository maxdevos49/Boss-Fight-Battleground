using System;

namespace BFB.Engine.Server.Communication
{
    [Serializable]
    public class InventoryActionMessage : DataMessage
    {
        
        public bool LeftClick { get; set; }
        
        public bool RightClick { get; set; }
        
        public byte? SlotId { get; set; }
        
    }
}