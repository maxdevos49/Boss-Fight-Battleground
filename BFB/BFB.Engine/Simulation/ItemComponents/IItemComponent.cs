using BFB.Engine.Entity;
using BFB.Engine.Inventory;
using BFB.Engine.Inventory.Configuration;

namespace BFB.Engine.Simulation.ItemComponents
{
    
    public interface IItemComponent
    {
        /// <summary>
        /// Used to define 
        /// </summary>
        /// <param name="simulation"></param>
        /// <param name="entity"></param>
        void Use(Simulation simulation, SimulationEntity entity, IItem item);
    }
}