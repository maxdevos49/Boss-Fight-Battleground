using BFB.Engine.Entity;

namespace BFB.Engine.Simulation.EntityComponents
{
    public class ParticleTrailEffect : EntityComponent
    {
        public ParticleTrailEffect() : base(false) {  }

        public override void Update(SimulationEntity entity, Simulation simulation)
        {
            
            SimulationEntity effect = SimulationEntity.SimulationEntityFactory("MiniStarParticle");
            effect.Position = entity.Position.Clone();
            simulation.AddEntity(effect);

            effect = SimulationEntity.SimulationEntityFactory("StarParticle");
            effect.Position = entity.Position.Clone();
            simulation.AddEntity(effect);
            
            effect = SimulationEntity.SimulationEntityFactory("XParticle");
            effect.Position = entity.Position.Clone();
            simulation.AddEntity(effect);
  
        }
    }
}