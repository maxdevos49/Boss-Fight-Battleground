using BFB.Engine.Entity;

namespace BFB.Engine.Simulation.EntityComponents
{
    public class ParticleExplosionEffect : EntityComponent
    {
        public ParticleExplosionEffect() : base(false) {}

        public override void OnSimulationRemove(Simulation simulation, SimulationEntity entity, EntityRemovalReason? reason)
        {
            Explode(simulation,entity);
        }
        
        private void Explode(Simulation simulation, SimulationEntity entity)
        {
            for (int i = 0; i < 50; i++)
            {
                if (simulation.World.ChunkFromPixelLocation((int) entity.Position.X, (int) entity.Position.Y) == null)
                    return;

                SimulationEntity effect = SimulationEntity.SimulationEntityFactory("rMiniStarParticle");
                effect.Position = entity.OriginPosition;
                simulation.AddEntity(effect);

                effect = SimulationEntity.SimulationEntityFactory("rStarParticle");
                effect.Position = entity.OriginPosition;
                simulation.AddEntity(effect);
            
                effect = SimulationEntity.SimulationEntityFactory("rXParticle");
                effect.Position = entity.OriginPosition;
                simulation.AddEntity(effect);
            }
        }
        
    }
}
