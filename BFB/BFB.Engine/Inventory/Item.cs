using BFB.Engine.Entity;
using BFB.Engine.InventoryManager;

namespace BFB.Engine.Inventory
{
    public class Item : IItem
    {
//        public static ComponentRegistry component;
        public string ItemConfigKey { get; private set; }
        
        private int _stackSize;

        public Item(string itemConfigKey)
        {
            ItemConfigKey = itemConfigKey;
            _stackSize = 0;
        }
        
        public bool IncrementStack()
        {
            throw new System.NotImplementedException();
        }

        public bool DecrementStack()
        {
            throw new System.NotImplementedException();
        }

        public bool SetStackSize(int stackSize)
        {
            throw new System.NotImplementedException();
        }

        public int StackSize()
        {
            throw new System.NotImplementedException();
        }

        public int MaxStackSize()
        {
            throw new System.NotImplementedException();
        }

        public bool IsStackFull()
        {
            throw new System.NotImplementedException();
        }

        public IItem Clone()
        {
            throw new System.NotImplementedException();
        }

//        public void RightClickAction(Simulation.Simulation simulation, SimulationEntity itemEntityOwner)
//        {
//            throw new System.NotImplementedException();
//        }
//
//        public void LeftClickAction(Simulation.Simulation simulation, SimulationEntity itemEntityOwner)
//        {
//            throw new System.NotImplementedException();
//        }
    }
}