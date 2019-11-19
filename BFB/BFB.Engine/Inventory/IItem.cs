using BFB.Engine.Entity;
using BFB.Engine.Inventory.Configuration;

namespace BFB.Engine.Inventory
{
    
    public interface IItem
    {
        string ItemConfigKey { get; set; }
        
        /// <summary>
        /// The items configuration data
        /// </summary>
        ItemConfiguration Configuration { get; set; }
        
        /// <summary>
        /// Meta data about a focused tile that a item component might need
        /// </summary>
        TileTarget TileTarget { get; set; }
        
        /// <summary>
        /// Increments the item stack by one if the item configuration allows
        /// </summary>
        /// <returns>A boolean if the action succeeded</returns>
        bool IncrementStack();
        
        /// <summary>
        /// Decrements the item stack by one if the item configuration allows
        /// </summary>
        /// <returns>A boolean of the action succeeded</returns>
        bool DecrementStack();
            
        /// <summary>
        /// Sets the current stack size
        /// </summary>
        /// <param name="stackSize">The stack size value to set</param>
        /// <returns>A boolean to indicate if the action succeeded</returns>
        bool SetStackSize(int stackSize);
        
        /// <summary>
        /// How many items are currently in the stack
        /// </summary>
        /// <returns>The number items in the stack</returns>
        int StackSize();

        /// <summary>
        /// Returns the maximum stack size
        /// </summary>
        /// <returns>The maximum stack size as a integer</returns>
        int MaxStackSize();
        
        /// <summary>
        /// Indicates if the stack is full or not 
        /// </summary>
        /// <returns>A boolean determining if the stack is full</returns>
        bool IsStackFull();

        /// <summary>
        /// Clones the item in scenarios like when splitting a stack of items
        /// </summary>
        /// <returns>A clone of the Item</returns>
        IItem Clone();

        void UseItemLeftClick(Simulation.Simulation simulation, SimulationEntity entity);
        
        void UseItemRightClick(Simulation.Simulation simulation, SimulationEntity entity);
        
        void UseItemLeftHold(Simulation.Simulation simulation, SimulationEntity entity, int holdTicks);
        
        void UseItemRightHold(Simulation.Simulation simulation, SimulationEntity entity, int holdTicks);

    }
}