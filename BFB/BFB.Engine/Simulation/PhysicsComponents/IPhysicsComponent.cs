using BFB.Engine.Entity;

namespace BFB.Engine.Simulation.PhysicsComponents
{
    public interface IPhysicsComponent
    {
        void Update(SimulationEntity entity, Simulation simulation);
    }
}