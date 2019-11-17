using BFB.Engine.Entity;

namespace BFB.Engine.Simulation.GameComponents
{
    public interface IGameComponent
    {
        void Update(SimulationEntity simulationEntity, Simulation simulation);
    }
}