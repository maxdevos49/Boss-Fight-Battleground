using BFB.Engine.Entity;

namespace BFB.Engine.Simulation.SimulationComponents
{
    public interface ISimulationComponent
    {
        void Update(SimulationEntity simulationEntity, Simulation simulation);
    }
}