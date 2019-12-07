namespace BFB.Engine.Server.Communication
{
    public class InventoryDataMessage : DataMessage
    {
        
        public int InventorySize { get; set; }
        
        public int HotBarRange { get; set; }
        
    }
}