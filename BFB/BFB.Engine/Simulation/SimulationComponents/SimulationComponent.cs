using BFB.Engine.Collisions;
using BFB.Engine.Entity;

namespace BFB.Engine.Simulation.SimulationComponents
{
    public abstract class SimulationComponent
    {
        /// <summary>
        /// If this is true then this component can only be used by players
        /// </summary>
        public bool RequiresPlayer { get; private set; }
        
        /// <summary>
        /// Constructs a Simulation Component
        /// </summary>
        /// <param name="requiresPlayer">A boolean indicating if it requires a player</param>
        protected SimulationComponent(bool requiresPlayer)
        {
            RequiresPlayer = requiresPlayer;
        }
        
        /// <summary>
        /// Used to init the component when added to the entity
        /// </summary>
        /// <param name="entity">The entity that the component belongs too </param>
        public virtual void Init(SimulationEntity entity) { }

        /// <summary>
        /// called each tick of the simulation for the component
        /// </summary>
        /// <param name="entity">The entity who owns the component</param>
        /// <param name="simulation">The simulation that the entity own</param>
        public virtual void Update(SimulationEntity entity, Simulation simulation) { }

        public virtual bool OnEntityCollision(Simulation simulation, SimulationEntity primaryEntity, SimulationEntity secondaryEntity)
        {
            return true;
        }

        public virtual bool OnTileCollision(Simulation simulation, SimulationEntity entity, TileCollision tc)
        {
            return true;
        }

        public virtual bool OnWorldBoundaryCollision(Simulation simulation, SimulationEntity entity, CollisionSide side)
        {
            return true;
        }
    }
}