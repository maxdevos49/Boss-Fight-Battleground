using BFB.Engine.Collisions;
using BFB.Engine.Entity;

namespace BFB.Engine.Simulation.SimulationComponents.Combat
{
    public class VoidDeathComponent : EntityComponent
    {
        public VoidDeathComponent() : base(false) { }

        public override bool OnWorldBoundaryCollision(Simulation simulation, SimulationEntity entity, CollisionSide side)
        {
            if (side == CollisionSide.BottomBorder && entity.Top > simulation.World.MapPixelHeight())
            {
                if (entity.Meta != null) 
                    entity.Meta.Health -= 2;
            }

            return true;
        }
    }
}