using BFB.Engine.Entity;

namespace BFB.Engine.Simulation.SimulationComponents
{
    /// <summary>
    /// Allows for easily removing an entity after a set amount of ticks
    /// </summary>
    public class LifetimeComponent : ISimulationComponent
    {
        private readonly int _lifetime;
        
        /// <summary>
        /// Constructs a Lifetime component for a entity
        /// </summary>
        /// <param name="lifetime">How many ticks the entity should live for</param>
        public LifetimeComponent(int lifetime)
        {
            _lifetime = lifetime;
        }
        
        public void Update(SimulationEntity entity, Simulation simulation)
        {
            if(entity.TicksSinceCreation > _lifetime)
                simulation.RemoveEntity(entity.EntityId);
        }
    }
}