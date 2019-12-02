using BFB.Engine.Entity;

namespace BFB.Engine.Simulation.EntityComponents
{
    public class PhysicsFloat : EntityComponent
    {
        public PhysicsFloat() : base(false) { }

        public override void Init(SimulationEntity entity)
        {
            entity.Rotation = 0;
        }

        public override void Update(SimulationEntity entity, Simulation simulation)
        {
            entity.Position.Y -= 1;
        }
    }
}