using BFB.Engine.Entity;
using BFB.Engine.Inventory;
using BFB.Engine.Server.Communication;

namespace BFB.Engine.Simulation.EntityComponents
{
    public class InventoryConnection : EntityComponent
    {
        public InventoryConnection() : base(true) { }

        public override void Init(SimulationEntity entity)
        {
            
            if(entity.Socket == null || entity.Inventory == null || entity.ControlState == null)
                return;

            IInventoryManager inventory = entity.Inventory;
            
            //right click select
            entity.Socket.On("/inventory/select", (m) =>
            {
                InventoryActionMessage action = (InventoryActionMessage) m;

                if (action.LeftClick)
                {
                    
                }
                else if(action.RightClick)
                {
                    
                }

            });
            
            //leave inventory
            entity.Socket.On("/inventory/leaveInventory", (m) =>
            {
                InventoryActionMessage action = (InventoryActionMessage) m;
                
            });
            
        }
    }
}