using System;
using BFB.Engine.Entity;
using BFB.Engine.Math;

namespace BFB.Engine.Simulation.EntityComponents
{
    public class FireballEffectComponent : EntityComponent
    {
        private Random _random;
        
        public FireballEffectComponent() : base(false) { }

        public override void Init(SimulationEntity entity)
        {
            _random = new Random();
        }

        public override void Update(SimulationEntity entity, Simulation simulation)
        {
            for (int i = 0; i < 5; i++)
            {
                SimulationEntity effect = new SimulationEntity(//TODO simulationEntity Factory
                    Guid.NewGuid().ToString(),
                    new EntityOptions()
                    {
                        TextureKey = "EffectRedBall",
                        Position = new BfbVector(entity.Position.X, entity.Position.Y + 3),
                        Dimensions = new BfbVector(_random.Next(1, 100), _random.Next(1, 100)),
                        Rotation = 0,
                        Origin = new BfbVector(0, 0),
                        EntityType = EntityType.Particle
                    }//, new List<EntityComponent>()
                    //{
                    //    new LifetimeComponent(5),
                    //    new SpellEffects2PhysicsComponent()
                    //}
                    );
                simulation.AddEntity(effect);
            }
            
        }
    }
}