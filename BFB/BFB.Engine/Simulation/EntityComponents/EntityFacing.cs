using BFB.Engine.Entity;

namespace BFB.Engine.Simulation.EntityComponents
{
    public class EntityFacing : EntityComponent
    {
        public EntityFacing() : base(false) { }

        public override void Update(SimulationEntity entity, Simulation simulation)
        {
            if (entity.Velocity.X > 2)
                entity.Facing = DirectionFacing.Right;
            else if (entity.Velocity.X < -2)
                entity.Facing = DirectionFacing.Left;
        }
    }
}