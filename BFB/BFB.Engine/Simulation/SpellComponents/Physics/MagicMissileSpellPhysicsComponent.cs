using System;
using System.Collections.Generic;
using System.Text;
using BFB.Engine.Entity;
using BFB.Engine.Math;
using BFB.Engine.Simulation.PhysicsComponents;
using BFB.Engine.Simulation.SimulationComponents;
using Microsoft.Xna.Framework;

namespace BFB.Engine.Simulation.SpellComponents.Physics
{
    public class MagicMissileSpellPhysicsComponent : EntityComponent
    {
        private int _timeToLive;
        private readonly SimulationEntity _owner;
        private readonly BfbVector _acceleration;
        private readonly Random _random;

        public MagicMissileSpellPhysicsComponent(BfbVector direction, SimulationEntity owner) : base(false)
        {
            _owner = owner;
            Vector2 directionNorm = direction.ToVector2();
            directionNorm.Normalize();
            _timeToLive = 30;
            float xAcc = directionNorm.X;
            float yAcc = directionNorm.Y;
            _acceleration = new BfbVector(xAcc, yAcc);
            _random = new Random();
        }

        public override void Update(SimulationEntity simulationEntity, Simulation simulation)
        {
            _timeToLive -= 1;
            if (_timeToLive <= 0)
                simulation.RemoveEntity(simulationEntity.EntityId);

            //Gives us the speed to move left and right
            simulationEntity.SteeringVector.X = _acceleration.X * 30;
            simulationEntity.SteeringVector.Y = _acceleration.Y  * 30;

            //MATH IS HARD!!
            //simulationEntity.DesiredVector.Y *= MathF.Sin(_random.Next(1, 10));

            //Creates the new velocity
            simulationEntity.Velocity.X = simulationEntity.SteeringVector.X;
            simulationEntity.Velocity.Y = simulationEntity.SteeringVector.Y;

            //Updates the position
            simulationEntity.Position.Add(simulationEntity.Velocity);

            CheckCollisions(simulationEntity, simulation);

            //Spawn Effects!
            if (simulation.World.ChunkFromPixelLocation((int)simulationEntity.Position.X, (int)simulationEntity.Position.Y) == null)
                return;

            SimulationEntity effect = new SimulationEntity(
                Guid.NewGuid().ToString(),
                new EntityOptions()
                {
                    TextureKey = "EffectMiniStar",
                    Position = new BfbVector(simulationEntity.Position.X, simulationEntity.Position.Y),
                    Dimensions = new BfbVector(_random.Next(1, 100), _random.Next(1,100)),
                    Rotation = 0,
                    Origin = new BfbVector(0, 0),
                }, new List<EntityComponent>()
                {
                    new SpellEffects1PhysicsComponent()
                });
            simulation.AddEntity(effect);
            effect = new SimulationEntity(
                Guid.NewGuid().ToString(),
                new EntityOptions()
                {
                    TextureKey = "EffectStar",
                    Position = new BfbVector(simulationEntity.Position.X, simulationEntity.Position.Y),
                    Dimensions = new BfbVector(_random.Next(1, 100), _random.Next(1, 100)),
                    Rotation = 0,
                    Origin = new BfbVector(0, 0),
                }, new List<EntityComponent>()
                {
                    new SpellEffects1PhysicsComponent()
                });
            simulation.AddEntity(effect);
            effect = new SimulationEntity(
                Guid.NewGuid().ToString(),
                new EntityOptions()
                {
                    TextureKey = "EffectX",
                    Position = new BfbVector(simulationEntity.Position.X, simulationEntity.Position.Y),
                    Dimensions = new BfbVector(_random.Next(1, 100), _random.Next(1, 100)),
                    Rotation = 0,
                    Origin = new BfbVector(0, 0),
                }, new List<EntityComponent>()
                {
                    new SpellEffects1PhysicsComponent()
                });
            simulation.AddEntity(effect);

            effect = new SimulationEntity(
                Guid.NewGuid().ToString(),
                new EntityOptions()
                {
                    TextureKey = "EffectMiniStar",
                    Position = new BfbVector(simulationEntity.Position.X, simulationEntity.Position.Y),
                    Dimensions = new BfbVector(_random.Next(1, 100), _random.Next(1, 100)),
                    Rotation = 0,
                    Origin = new BfbVector(0, 0),
                }, new List<EntityComponent>()
                {
                    new SpellEffects1PhysicsComponent()
                });
            simulation.AddEntity(effect);
            effect = new SimulationEntity(
                Guid.NewGuid().ToString(),
                new EntityOptions()
                {
                    TextureKey = "EffectStar",
                    Position = new BfbVector(simulationEntity.Position.X, simulationEntity.Position.Y),
                    Dimensions = new BfbVector(_random.Next(1, 100), _random.Next(1, 100)),
                    Rotation = 0,
                    Origin = new BfbVector(0, 0),
                }, new List<EntityComponent>()
                {
                    new SpellEffects1PhysicsComponent()
                });
            simulation.AddEntity(effect);
            effect = new SimulationEntity(
                Guid.NewGuid().ToString(),
                new EntityOptions()
                {
                    TextureKey = "EffectX",
                    Position = new BfbVector(simulationEntity.Position.X, simulationEntity.Position.Y),
                    Dimensions = new BfbVector(_random.Next(1, 100), _random.Next(1, 100)),
                    Rotation = 0,
                    Origin = new BfbVector(0, 0),
                }, new List<EntityComponent>()
                {
                    new SpellEffects1PhysicsComponent()
                });
            
            simulation.AddEntity(effect);

        }

        private void CheckCollisions(SimulationEntity simulationEntity, Simulation simulation)
        {
            // Collisions with monsters
            List<SimulationEntity> targets = new List<SimulationEntity>();
            for (int i = 0; i < 10; i++)
            {
                int xPos = (int)simulationEntity.Position.X;
                SimulationEntity target = simulation.GetEntityAtPosition(xPos, (int)simulationEntity.Position.Y);
                if (target != null && target != simulationEntity && !targets.Contains(target))
                    targets.Add(target);
            }

            targets.Remove(_owner);
            if (targets.Count >= 1)
            {
                DamageTargets(targets);
                simulation.RemoveEntity(simulationEntity.EntityId);
            }
        }

        private void DamageTargets(List<SimulationEntity> targets)
        {
            foreach (SimulationEntity target in targets)
            {
                // Instead of a hard coded value here, you could call a weapon stored on the simulationEntity, and use its damage value.
                if (target.Meta != null) 
                    target.Meta.Health -= 10;
            }
        }
    }
}
