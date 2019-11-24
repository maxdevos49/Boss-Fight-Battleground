using BFB.Engine.Entity;

namespace BFB.Engine.Simulation.EntityComponents
{
    /// <summary>
    /// Allows for easily removing an entity after a set amount of ticks
    /// </summary>
    public class LifetimeComponent : EntityComponent
    {
        private readonly int _lifetime;

        /// <summary>
        /// Constructs a Lifetime component for a entity
        /// </summary>
        /// <param name="lifetime">How many ticks the entity should live for</param>
        public LifetimeComponent(int lifetime) : base(false)
        {
            _lifetime = lifetime;
        }
        
        public override void Update(SimulationEntity entity, Simulation simulation)
        {
            if(entity.TicksSinceCreation > _lifetime)
                simulation.RemoveEntity(entity.EntityId);
        }
    }
}