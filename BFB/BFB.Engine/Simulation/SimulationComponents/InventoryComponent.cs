using System;
using System.Linq;
using BFB.Engine.Entity;
using BFB.Engine.Inventory;

namespace BFB.Engine.Simulation.SimulationComponents
{
    public class InventoryComponent : SimulationComponent
    {

        private bool _leftClick;
        private int _leftHoldCount;
        private bool _rightClick;
        private int _rightHoldCount;

        public InventoryComponent() : base(false)
        {
            _leftClick = false;
            _leftHoldCount = 0;
            
            _rightClick = false;
            _rightHoldCount = 0;
        }

        public override void Init(SimulationEntity entity)
        {
            entity.CollideWithFilters.Add("items");
            entity.Inventory = new InventoryManager(20, 10);
        }

        public override void Update(SimulationEntity entity, Simulation simulation)
        {
            if (entity.Inventory == null || entity.ControlState == null)
                return;

            IItem activeItem = entity.Inventory.GetActiveSlot();
            
            if (entity.ControlState.LeftClick)
            {
                _leftHoldCount++;

                if (!_leftClick)
                {
                    _leftClick = true;
                    //TODO left click components

                }
                else
                {
                    //TODO left hold components
                    
                }

            }else if (entity.ControlState.RightClick)
            {
                _rightHoldCount++;

                if (!_rightClick)
                {
                    _rightClick = true;
                    //TODO right click components
                    
                }
                else
                {
                    //TODO right hold components
                    
                }
            }

            if (_leftClick && !entity.ControlState.Left)
                _leftClick = false;

            if (_rightClick && !entity.ControlState.Right)
                _rightClick = false;

        }

        /// <summary>
        /// Pick up Items you come across
        /// </summary>
        public override bool OnEntityCollision(Simulation simulation, SimulationEntity primaryEntity, SimulationEntity secondaryEntity)
        {
            if(secondaryEntity.EntityType != EntityType.Item)
                return true;

            if (secondaryEntity.Inventory == null || primaryEntity.Inventory == null || secondaryEntity.Inventory.MaxInventorySize() == 1)
                return true;

            IItem item = secondaryEntity.Inventory.GetSlot(0);
            
            if (primaryEntity.Inventory.Insert(item) == null)
                simulation.RemoveEntity(secondaryEntity.EntityId);//Maybe needs more?

            return false;
        }
    }
}