using System;
using BFB.Engine.Entity;
using BFB.Engine.Inventory.Configuration;
using BFB.Engine.Simulation;

namespace BFB.Engine.Inventory
{
    public class Item : IItem
    {
        #region Properties
        public string ItemConfigKey { get; set; }

        public ItemConfiguration Configuration { get; set; }

        public TileTarget TileTarget { get; set; }

        private int _stackSize;
        
        #endregion
        
        #region Constructor

        public Item(string itemConfigKey)
        {
            ItemConfigKey = itemConfigKey;
            Configuration = ConfigurationRegistry.GetInstance().GetItemConfiguration(itemConfigKey);
            TileTarget = new TileTarget();
            _stackSize = 1;
        }
        
        #endregion
        
        #region IncrementStack
        
        public bool IncrementStack()
        {
            if (_stackSize + 1 > Configuration.StackLimit)
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
            if (stackSize < 0 && stackSize > Configuration.StackLimit)
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
            return Configuration.StackLimit;
        }
        
        #endregion

        #region IsStackFull

        public bool IsStackFull()
        {
            return _stackSize == Configuration.StackLimit;
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
            if (Configuration.LeftClickComponents.Count == 0)
            {//Do default actions if no specified item components
//                ConfigurationRegistry.GetInstance().GetItemComponent("Hit").Use(simulation, entity, this);
                ConfigurationRegistry.GetInstance().GetItemComponent("BreakBlock").Use(simulation, entity, this);

                return;
            }
            
            foreach (string componentKey in Configuration.LeftClickComponents)
                ConfigurationRegistry.GetInstance().GetItemComponent(componentKey).Use(simulation,entity,this);
        }
        
        #endregion

        #region UseItemRightClick
        
        public void UseItemRightClick(Simulation.Simulation simulation, SimulationEntity entity)
        {
            if (Configuration.RightClickComponents.Count == 0)
                return;
            
            foreach (string componentKey in Configuration.RightClickComponents)
                ConfigurationRegistry.GetInstance().GetItemComponent(componentKey).Use(simulation,entity,this);
        }
        
        #endregion
        
        #region UseItemLeftHold

        public void UseItemLeftHold(Simulation.Simulation simulation, SimulationEntity entity, int holdTicks)
        {
            if (holdTicks % Configuration.CoolDown != 0)
                return;
            
            if (Configuration.LeftHoldComponents.Count == 0)
            {
//                ConfigurationRegistry.GetInstance().GetItemComponent("Hit").Use(simulation, entity, this);
                ConfigurationRegistry.GetInstance().GetItemComponent("BreakBlock").Use(simulation, entity, this);

                return;
            }
            
            foreach (string componentKey in Configuration.LeftHoldComponents)
                ConfigurationRegistry.GetInstance().GetItemComponent(componentKey).Use(simulation,entity,this);
        }
        
        #endregion

        #region UseItemRightHold

        public void UseItemRightHold(Simulation.Simulation simulation, SimulationEntity entity, int holdTicks)
        {
            if (holdTicks % Configuration.CoolDown != 0)
                return;
            
            if (Configuration.RightHoldComponents.Count == 0)
                return;
            
            foreach (string componentKey in Configuration.RightHoldComponents)
                ConfigurationRegistry.GetInstance().GetItemComponent(componentKey).Use(simulation,entity,this);
        }
        
        #endregion

    }
}