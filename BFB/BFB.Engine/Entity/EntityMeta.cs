using System;
using BFB.Engine.Inventory;

namespace BFB.Engine.Entity
{
    [Serializable]
    public class EntityMeta
    {
       
        public ushort Health { get; set; }
        
        public ushort Mana { get; set; }
        
        public HoldingItem Holding { get; set; }
        
        public EntityMeta()
        {
            //defaults
            Health = 20;
            Mana = 1000;
            Holding = new HoldingItem();
        }
        
    }
}