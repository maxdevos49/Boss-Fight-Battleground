using System;
using BFB.Engine.Inventory;
using JetBrains.Annotations;

namespace BFB.Engine.Entity
{
    [Serializable]
    public class EntityMeta
    {
       [UsedImplicitly]
        public ushort Health { get; set; }
        
        [UsedImplicitly]
        public ushort Mana { get; set; }
        
        [UsedImplicitly]
        public ushort MaxHealth { get; set; }
        
        [UsedImplicitly]
        public ushort MaxMana { get; set; }
        
        [UsedImplicitly]
        public HoldingItem Holding { get; set; }
        
        [CanBeNull]
        [UsedImplicitly]
        public InventorySlot MouseSlot { get; set; }
        
        public EntityMeta()
        {
            //defaults
            MaxHealth = Health = 20;
            MaxMana = Mana = 1000;
            Holding = new HoldingItem();
        }
        
    }
}