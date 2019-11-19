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

        private IItem _defaultItem;

        public InventoryComponent() : base(false)
        {
            _leftClick = false;
            _leftHoldCount = 0;
            
            _rightClick = false;
            _rightHoldCount = 0;
            
            _defaultItem = new Item("Default");
        }

        public override void Init(SimulationEntity entity)
        {
            if(!entity.CollideWithFilters.Contains("item"))
                entity.CollideWithFilters.Add("item");
            
            entity.Inventory = new InventoryManager(10, 10);

            //Temp grass items
//            Item item = new Item("Grass");
//            item.SetStackSize(64);
//            entity.Inventory.Insert(item);
        }

        /// <summary>
        /// Make use of the active item based on the controlState
        /// </summary>
        public override void Update(SimulationEntity entity, Simulation simulation)
        {
            if (entity.Inventory == null || entity.ControlState == null)
                return;


            entity.Inventory.MoveActiveSlot(entity.ControlState.HotBarLeft);

            IItem activeItem = entity.Inventory.GetActiveSlot() ?? _defaultItem;

            //left click actions
            if (entity.ControlState.LeftClick)
            {
                _leftHoldCount++;

                if (!_leftClick)
                    activeItem.UseItemLeftClick(simulation, entity);//left click
                else
                    activeItem.UseItemLeftHold(simulation, entity, _leftHoldCount);//left hold
                
                _leftClick = true;

                return;
            }
            
            //Right click actions
            if (entity.ControlState.RightClick)
            {
                _rightHoldCount++;

                if (!_rightClick)
                    activeItem.UseItemRightClick(simulation, entity);//right click
                else
                    activeItem.UseItemRightHold(simulation, entity, _rightHoldCount);//right hold
                
                _rightClick = true;
                
                return;
            }

            _leftClick = false;
            _rightClick = false;
            _leftHoldCount = 0;
            _leftHoldCount = 0;
            activeItem.TileTarget.Progress = 0;
        }

        /// <summary>
        /// Pick up Items you come across
        /// </summary>
        public override bool OnEntityCollision(Simulation simulation, SimulationEntity primaryEntity, SimulationEntity secondaryEntity)
        {
            
            if(secondaryEntity.EntityType != EntityType.Item)
                return true;

            if (secondaryEntity.Inventory == null || primaryEntity.Inventory == null || secondaryEntity.Inventory.MaxInventorySize() != 1)
                return true;

            
            IItem item = secondaryEntity.Inventory.GetSlot(0);
            
            if (primaryEntity.Inventory.Insert(item) == null)
                simulation.RemoveEntity(secondaryEntity.EntityId);//Maybe needs more?
            
            return false;
        }
    }
}