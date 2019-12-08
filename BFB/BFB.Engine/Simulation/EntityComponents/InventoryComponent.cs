using System;
using System.Collections.Generic;
using BFB.Engine.Entity;
using BFB.Engine.Inventory;

namespace BFB.Engine.Simulation.EntityComponents
{
    public class InventoryComponent : EntityComponent
    {

        private bool _leftClick;
        private int _leftHoldCount;
        private bool _rightClick;
        private int _rightHoldCount;

        private IItem _defaultItem;
        private List<IItem> _defaultItems;

        public InventoryComponent(List<IItem> items = null) : base(false)
        {
            _leftClick = false;
            _leftHoldCount = 0;
            
            _rightClick = false;
            _rightHoldCount = 0;
            
            _defaultItem = new Item("Default");

            _defaultItems = items??new List<IItem>();
        }

        public override void Init(SimulationEntity entity)
        {
            if(!entity.CollideWithFilters.Contains("item"))
                entity.CollideWithFilters.Add("item");
            
            entity.Inventory = new InventoryManager(27, 7);

            //Temp items
            Item item = new Item("Wood");
            item.SetStackSize(64);
            entity.Inventory.Insert(item);
//            
//            item = new Item("WoodWall");
//            item.SetStackSize(64);
//            entity.Inventory.Insert(item);
//            
//            item = new Item("Leaves");
//            item.SetStackSize(64);
//            entity.Inventory.Insert(item);
//            
//            item = new Item("LeavesWall");
//            item.SetStackSize(64);
//            entity.Inventory.Insert(item);
//            
//            item = new Item("Plank");
//            item.SetStackSize(64);
//            entity.Inventory.Insert(item);
//            
//            item = new Item("PlankWall");
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

            entity.Inventory.MoveActiveSlot(entity.ControlState.HotBarPosition);

            #region Reach
            
            IItem activeItem = entity.Inventory.GetActiveSlot() ?? _defaultItem;//default item is if we are holding nothing

            Tuple<int, int> block = simulation.World.BlockLocationFromPixel((int) entity.ControlState.Mouse.X, (int) entity.ControlState.Mouse.Y);

            if (block != null)
            {
                int playerX = (int) (entity.Position.X + entity.Width / 2f);
                int playerY = (int) (entity.Position.Y + entity.Height / 2f);
                int blockPixelX = block.Item1 * simulation.World.WorldOptions.WorldScale; //x position of block mouse is over
                int blockPixelY = block.Item2 * simulation.World.WorldOptions.WorldScale; //y position of block mouse is over

                int distance =
                    (int) System.Math.Sqrt(System.Math.Pow(playerX - blockPixelX, 2) +
                                           System.Math.Pow(playerY - blockPixelY, 2)) / simulation.World.WorldOptions.WorldScale;
                int reach = activeItem.Configuration.Reach == 0 ? 100 : activeItem.Configuration.Reach;

                if(distance >= reach)
                    return;
            }
            
            #endregion

            #region Left Click
            
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
            
            #endregion

            #region Right Click

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
            
            #endregion

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
            if(secondaryEntity.EntityType != EntityType.Item || primaryEntity.EntityType != EntityType.Player)
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