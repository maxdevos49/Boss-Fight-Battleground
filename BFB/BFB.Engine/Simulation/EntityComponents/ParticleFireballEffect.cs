using BFB.Engine.Entity;

namespace BFB.Engine.Simulation.EntityComponents
{
    public class ParticleFireballEffect : EntityComponent
    {
        public ParticleFireballEffect() : base(false) { }

        public override void Update(SimulationEntity entity, Simulation simulation)
        {
            for (int i = 0; i < 4; i++)
            {
                SimulationEntity effect = SimulationEntity.SimulationEntityFactory("FireBallParticle");
                effect.Position = entity.Position.Clone();
                simulation.AddEntity(effect);
            }
        }
    }
}