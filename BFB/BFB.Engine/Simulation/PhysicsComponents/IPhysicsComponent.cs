using BFB.Engine.Entity;

namespace BFB.Engine.Simulation.PhysicsComponents
{
    public interface IPhysicsComponent
    {
        void Update(SimulationEntity simulationEntity, Simulation simulation);
    }
}