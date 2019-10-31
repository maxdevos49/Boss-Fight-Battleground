using BFB.Engine.Entity;

namespace BFB.Engine.Simulation.InputComponents
{
    public interface IInputComponent
    {
        void Update(SimulationEntity simulationEntity, Simulation simulation);
    }
}