using System;
using System.Collections.Generic;
using BFB.Engine.Entity;
using BFB.Engine.Math;
using BFB.Engine.Simulation.EntityComponents.Physics;

namespace BFB.Engine.Simulation.EntityComponents.Effects
{
    public class MagicMissileEffectComponent : EntityComponent
    {
        private Random _random;
        
        public MagicMissileEffectComponent() : base(false) {  }

        public override void Init(SimulationEntity entity)
        {
            _random = new Random();
        }

        public override void Update(SimulationEntity entity, Simulation simulation)
        {
            
            SimulationEntity effect = new SimulationEntity(//TODO simulationEntity Factory
                Guid.NewGuid().ToString(),
                new EntityOptions()
                {
                    TextureKey = "EffectMiniStar",
                    Position = new BfbVector(entity.Position.X, entity.Position.Y),
                    Dimensions = new BfbVector(_random.Next(1, 100), _random.Next(1,100)),
                    Rotation = 0,
                    Origin = new BfbVector(0, 0),
                    EntityType = EntityType.Effect
                }, new List<EntityComponent>()
                {
                    new LifetimeComponent(10),
                    new SpellEffects1PhysicsComponent()
                });
            simulation.AddEntity(effect);
            effect = new SimulationEntity(
                Guid.NewGuid().ToString(),
                new EntityOptions()
                {
                    TextureKey = "EffectStar",
                    Position = new BfbVector(entity.Position.X, entity.Position.Y),
                    Dimensions = new BfbVector(_random.Next(1, 100), _random.Next(1, 100)),
                    Rotation = 0,
                    Origin = new BfbVector(0, 0),
                    EntityType = EntityType.Effect
                }, new List<EntityComponent>
                {
                    new LifetimeComponent(10),
                    new SpellEffects1PhysicsComponent()
                });
            simulation.AddEntity(effect);
            effect = new SimulationEntity(
                Guid.NewGuid().ToString(),
                new EntityOptions()
                {
                    TextureKey = "EffectX",
                    Position = new BfbVector(entity.Position.X, entity.Position.Y),
                    Dimensions = new BfbVector(_random.Next(1, 100), _random.Next(1, 100)),
                    Rotation = 0,
                    Origin = new BfbVector(0, 0),
                    EntityType = EntityType.Effect
                }, new List<EntityComponent>
                {
                    new LifetimeComponent(10),
                    new SpellEffects1PhysicsComponent()
                });
            simulation.AddEntity(effect);

            effect = new SimulationEntity(
                Guid.NewGuid().ToString(),
                new EntityOptions()
                {
                    TextureKey = "EffectMiniStar",
                    Position = new BfbVector(entity.Position.X, entity.Position.Y),
                    Dimensions = new BfbVector(_random.Next(1, 100), _random.Next(1, 100)),
                    Rotation = 0,
                    Origin = new BfbVector(0, 0),
                    EntityType = EntityType.Effect
                }, new List<EntityComponent>
                {
                    new LifetimeComponent(10),
                    new SpellEffects1PhysicsComponent()
                });
            simulation.AddEntity(effect);
            effect = new SimulationEntity(
                Guid.NewGuid().ToString(),
                new EntityOptions()
                {
                    TextureKey = "EffectStar",
                    Position = new BfbVector(entity.Position.X, entity.Position.Y),
                    Dimensions = new BfbVector(_random.Next(1, 100), _random.Next(1, 100)),
                    Rotation = 0,
                    Origin = new BfbVector(0, 0),
                    EntityType = EntityType.Effect
                }, new List<EntityComponent>
                {
                    new LifetimeComponent(10),
                    new SpellEffects1PhysicsComponent()
                });
            simulation.AddEntity(effect);
            effect = new SimulationEntity(
                Guid.NewGuid().ToString(),
                new EntityOptions()
                {
                    TextureKey = "EffectX",
                    Position = new BfbVector(entity.Position.X, entity.Position.Y),
                    Dimensions = new BfbVector(_random.Next(1, 100), _random.Next(1, 100)),
                    Rotation = 0,
                    Origin = new BfbVector(0, 0),
                    EntityType = EntityType.Effect
                }, new List<EntityComponent>
                {
                    new LifetimeComponent(10),
                    new SpellEffects1PhysicsComponent()
                });
            
            simulation.AddEntity(effect);
            
        }
    }
}