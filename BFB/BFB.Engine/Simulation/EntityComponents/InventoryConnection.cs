using BFB.Engine.Entity;
using BFB.Engine.Inventory;
using BFB.Engine.Server.Communication;

namespace BFB.Engine.Simulation.EntityComponents
{
    public class InventoryConnection : EntityComponent
    {
        private IItem _swapItem;

        public InventoryConnection() : base(true)
        {
            _swapItem = null;
        }

        public override void Init(SimulationEntity entity)
        {
            
            if(entity.Socket == null || entity.Inventory == null)
                return;
            
            if(entity.Meta == null)
                entity.Meta = new EntityMeta();
            
            //right click select
            entity.Socket.On("/inventory/select", (m) =>
            {
                
                InventoryActionMessage action = (InventoryActionMessage) m;

                if (action.SlotId == null)
                {
                    //TODO Throw item

                    _swapItem = null;
                    entity.Meta.MouseSlot = null;
                    return;
                }
                
                if (action.LeftClick)
                {
                    _swapItem = entity.Inventory.Swap((byte)action.SlotId, _swapItem);
                    if (_swapItem != null)
                    {
                        entity.Meta.MouseSlot = new InventorySlot
                        {
                            Name = _swapItem.ItemConfigKey,
                            Count = _swapItem.StackSize(),
                            ItemType = _swapItem.Configuration.ItemType,
                            TextureKey = _swapItem.Configuration.TextureKey,
                        };
                    }
                    else
                    {
                        entity.Meta.MouseSlot = null;
                    }
                    
                }
                else if(action.RightClick)
                {
                    if (_swapItem == null)
                    {
                        //TODO Split stack if exist
                    }
                    else
                    {
                        //TODO place a single 
                    }
                }

            });
            
            //leave inventory
            entity.Socket.On("/inventory/leaveInventory", (m) =>
            {
                InventoryActionMessage action = (InventoryActionMessage) m;

                if (_swapItem == null)
                    return;
                
                
                //TODO throw item if still holding it

            });
            
        }
    }
}