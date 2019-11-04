using BFB.Engine.Entity;

namespace BFB.Engine.Simulation.PhysicsComponents
{
    /// <summary>
    /// The interface to outline how to implement a Physics component
    /// </summary>
    public interface IPhysicsComponent
    {
        /// <summary>
        /// The update method that is called each frame of the simulation
        /// </summary>
        /// <param name="simulationEntity">The entity that the physics will apply too</param>
        /// <param name="simulation">An instance of the current simulation</param>
        void Update(SimulationEntity simulationEntity, Simulation simulation);
    }
}