using BFB.Engine.Entity;

namespace BFB.Engine.Simulation.InputComponents
{
    /// <summary>
    /// THe interface to outline how to implement a Input component
    /// </summary>
    public interface IInputComponent
    {
        /// <summary>
        /// The update method that is called by the entity every time it is ticked.
        /// </summary>
        /// <param name="simulationEntity">The entity who called update</param>
        /// <param name="simulation">The simulation the entity is in</param>
        void Update(SimulationEntity simulationEntity, Simulation simulation);
    }
}