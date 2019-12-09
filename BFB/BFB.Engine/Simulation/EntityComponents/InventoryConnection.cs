using System;
using System.Collections.Generic;
using System.Linq;
using BFB.Engine.Entity;
using BFB.Engine.Inventory;
using BFB.Engine.Math;
using BFB.Engine.Server.Communication;
using JetBrains.Annotations;

namespace BFB.Engine.Simulation.EntityComponents
{
    public class InventoryConnection : EntityComponent
    {
        private readonly object _lock;
        
        [CanBeNull]
        private IItem _swapItem;

        private readonly List<IItem> _dropItems;

        public InventoryConnection() : base(true)
        {
            _lock = new object();
            _dropItems = new List<IItem>();
            _swapItem = null;
        }

        #region Init
        
        public override void Init(SimulationEntity entity)
        {
            
            if(entity.Socket == null || entity.Inventory == null)
                return;
            
            if(entity.Meta == null)
                entity.Meta = new EntityMeta();
            
            //right click select
            entity.Socket.On("/inventory/select", (m) =>
            {
                lock (_lock)
                {
                    InventoryActionMessage action = (InventoryActionMessage) m;

                    if (action.SlotId == null)
                    {
                        if (_swapItem == null)
                            return;
                        
                        _dropItems.Add(_swapItem);
                        _swapItem = null;
                        entity.Meta.MouseSlot = null;
                        return;
                    }

                    if (action.LeftClick)
                    {
                        _swapItem =
                            _swapItem?.ItemConfigKey == entity.Inventory.GetSlot((byte) action.SlotId)?.ItemConfigKey
                                ? entity.Inventory.Merge((byte) action.SlotId, _swapItem)
                                : entity.Inventory.Swap((byte) action.SlotId, _swapItem);
                    }
                    else if (action.RightClick)
                    {
                        if (_swapItem == null)
                            _swapItem = entity.Inventory.Split((byte) action.SlotId);
                        else
                        {
                            if (!entity.Inventory.IsSlotFull((byte) action.SlotId) 
                                    && (_swapItem?.ItemConfigKey == entity.Inventory.GetSlot((byte) action.SlotId)?.ItemConfigKey 
                                        || entity.Inventory.GetSlot((byte) action.SlotId) == null))
                            {
                                IItem item = _swapItem.Clone();
                                item.SetStackSize(1);

                                if (!_swapItem.DecrementStack())
                                    _swapItem = null;

                                //place single item in new slot or merge left over back into original stack
                                entity.Inventory.Merge((byte) action.SlotId, item);
                            }
                        }
                    }

                    entity.Meta.MouseSlot = _swapItem?.GetInventorySlotItem();
                }
            });
            
            
            entity.Socket.On("/inventory/throwHolding", (m) =>
            {
                if (_swapItem == null) 
                    return;

                if (m.Message == "one")
                {
                    //Throw 1 item
                    IItem dropItem = _swapItem?.Clone();
                    dropItem?.SetStackSize(1);

                    if (_swapItem?.DecrementStack() == false)
                    {
                        _swapItem = null;
                        entity.Meta.MouseSlot = null;
                    }

                    _dropItems.Add(dropItem);
                }
                else
                {//Throw whole stack
                    _dropItems.Add(_swapItem);
                    _swapItem = null;
                    entity.Meta.MouseSlot = null;
                }
            });
            
        }
        
        #endregion

        public override void Update(SimulationEntity entity, Simulation simulation)
        {
            lock (_lock)
            {
                if (entity.ControlState != null && entity.Inventory != null && entity.ControlState.DropItem)
                {
                    IItem activeItem = entity.Inventory.GetActiveSlot()?.Clone();

                    if (activeItem != null)
                    {
                        if (entity.Inventory.GetActiveSlot()?.DecrementStack() == false)
                            entity.Inventory.Remove(entity.Inventory.GetActiveSlotId());

                        activeItem.SetStackSize(1);
                        _dropItems.Add(activeItem);
                    }

                }
                
                if (!_dropItems.Any())
                    return;
                    
                foreach (IItem dropItem in _dropItems)
                {
                    
                    InventoryManager inventory = new InventoryManager();
                    inventory.Insert(dropItem);

                    SimulationEntity newItem = SimulationEntity.SimulationEntityFactory("Item");
            
                    newItem.TextureKey = dropItem.Configuration.TextureKey;
                    newItem.Inventory = inventory;

                    if (entity.Facing == DirectionFacing.Left)
                    {
                        newItem.Position = entity.Position.Clone();
                        newItem.Position.X -= 50;
                        newItem.SteeringVector.X = -40;
                    }
                    else
                    {
                        newItem.Position = entity.Position.Clone();
                        newItem.Position.X += entity.Width + 50;
                        newItem.SteeringVector.X = 40;
                    }
                    simulation.AddEntity(newItem);
                }
                
                _dropItems.Clear();
            }
        }
    }
}