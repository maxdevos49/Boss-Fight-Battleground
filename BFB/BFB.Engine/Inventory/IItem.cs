namespace BFB.Engine.Inventory
{
    
    public interface IItem
    {
        
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
        
//        /// <summary>
//        /// //TODO
//        /// </summary>
//        /// <param name="simulation"></param>
//        /// <param name="itemEntityOwner"></param>
//        void RightClickAction(Simulation.Simulation simulation,SimulationEntity itemEntityOwner);
//
//        /// <summary>
//        /// //TODO
//        /// </summary>
//        /// <param name="simulation"></param>
//        /// <param name="itemEntityOwner"></param>
//        void LeftClickAction(Simulation.Simulation simulation, SimulationEntity itemEntityOwner);
    }
}