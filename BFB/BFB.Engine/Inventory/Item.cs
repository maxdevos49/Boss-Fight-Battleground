using BFB.Engine.Entity;
using BFB.Engine.Inventory.Configuration;
using BFB.Engine.Simulation;

namespace BFB.Engine.Inventory
{
    public class Item : IItem
    {
        #region Properties
        public string ItemConfigKey { get; set; }

        private readonly ItemConfiguration _configuration;
        
        private int _stackSize;
        
        #endregion
        
        #region Constructor

        public Item(string itemConfigKey)
        {
            ItemConfigKey = itemConfigKey;
            _configuration = ConfigurationRegistry.GetInstance().GetItemConfiguration(itemConfigKey);
            _stackSize = 1;
        }
        
        #endregion
        
        #region IncrementStack
        
        public bool IncrementStack()
        {
            if (_stackSize + 1 > _configuration.StackLimit)
                return false;

            _stackSize++;
            return true;
        }
        
        #endregion

        #region DecrementStack
        
        public bool DecrementStack()
        {
            if (_stackSize - 1 < 1)
                return false;

            _stackSize--;
            return true;
        }
        
        #endregion

        #region SetStackSize
        
        public bool SetStackSize(int stackSize)
        {
            if (stackSize < 0 && stackSize > _configuration.StackLimit)
                return false;

            _stackSize = stackSize;
            return true;
        }
        
        #endregion

        #region StackSize
        
        public int StackSize()
        {
            return _stackSize;
        }
        
        #endregion

        #region MaxStackSize
        
        public int MaxStackSize()
        {
            return _configuration.StackLimit;
        }
        
        #endregion

        #region IsStackFull

        public bool IsStackFull()
        {
            return _stackSize == _configuration.StackLimit;
        }
        
        #endregion

        #region Clone

        public IItem Clone()
        {
            IItem clone = new Item(ItemConfigKey);
            clone.SetStackSize(_stackSize);
            return clone;
        }
        
        #endregion

        #region UseItemLeftClick
        
        public void UseItemLeftClick(Simulation.Simulation simulation, SimulationEntity entity)
        {
            throw new System.NotImplementedException();
        }
        
        #endregion

        public void UseItemRightClick(Simulation.Simulation simulation, SimulationEntity entity)
        {
            throw new System.NotImplementedException();
        }

        public void UseItemLeftHold(Simulation.Simulation simulation, SimulationEntity entity, int holdTicks)
        {
            throw new System.NotImplementedException();
        }

        public void UseItemRightHold(Simulation.Simulation simulation, SimulationEntity entity, int holdTicks)
        {
            throw new System.NotImplementedException();
        }

    }
}