using System;
using System.Collections.Generic;
using System.Text;
using BFB.Engine.Entity;
using BFB.Engine.Math;
using BFB.Engine.Simulation.PhysicsComponents;
using Microsoft.Xna.Framework;

namespace BFB.Engine.Simulation.SpellComponents.Physics
{
    public class BombSpellPhysicsComponent : IPhysicsComponent
    {
        private int timeToLive;
        private readonly SimulationEntity _owner;
        private readonly BfbVector _acceleration;
        private readonly BfbVector _gravity;
        private Boolean _setInitialVelocity;
        private Random _random;

        public BombSpellPhysicsComponent(BfbVector direction, SimulationEntity owner)
        {
            timeToLive = 10;
            Vector2 direction2 = direction.ToVector2();
            direction2.Normalize();
            _acceleration = new BfbVector(direction2.X * 25, -20);
            _gravity = new BfbVector(0, 4f);
            _owner = owner;
            _setInitialVelocity = false;
            _random = new Random();
        }

        public void Update(SimulationEntity simulationEntity, Simulation simulation)
        {
            if (!_setInitialVelocity)
            {
                _setInitialVelocity = true;
                simulationEntity.Velocity.Y = _acceleration.Y;
            }

            timeToLive -= 1;
            if (timeToLive <= 0)
            {
                Explode(simulationEntity, simulation);
                simulation.RemoveEntity(simulationEntity.EntityId);
            }

            simulationEntity.Velocity.X = _acceleration.X;
            simulationEntity.Velocity.Y += _gravity.Y;

            //Updates the position
            simulationEntity.Position.Add(simulationEntity.Velocity);

            CheckCollisions(_owner, simulation);
        }

        private void Explode(SimulationEntity simulationEntity, Simulation simulation)
        {
            for (int i = 0; i < 50; i++)
            {
                if (simulation.World.ChunkFromPixelLocation((int)simulationEntity.Position.X, (int)simulationEntity.Position.Y) == null)
                    return;

                SimulationEntity effect = new SimulationEntity(
                    Guid.NewGuid().ToString(),
                    new EntityOptions()
                    {
                        AnimatedTextureKey = "EffectMiniStar",
                        Position = new BfbVector(simulationEntity.Position.X + 50, simulationEntity.Position.Y + 50),
                        Dimensions = new BfbVector(_random.Next(1, 100), _random.Next(1, 100)),
                        Rotation = 0,
                        Origin = new BfbVector(0, 0),
                    }, new ComponentOptions
                    {
                        Physics = new SpellEffects3PhysicsComponent()
                    });
                simulation.AddEntity(effect);
                effect = new SimulationEntity(
                    Guid.NewGuid().ToString(),
                    new EntityOptions()
                    {
                        AnimatedTextureKey = "EffectStar",
                        Position = new BfbVector(simulationEntity.Position.X + 25, simulationEntity.Position.Y + 25),
                        Dimensions = new BfbVector(_random.Next(1, 100), _random.Next(1, 100)),
                        Rotation = 0,
                        Origin = new BfbVector(0, 0),
                    }, new ComponentOptions
                    {
                        Physics = new SpellEffects3PhysicsComponent()
                    });
                simulation.AddEntity(effect);
                effect = new SimulationEntity(
                    Guid.NewGuid().ToString(),
                    new EntityOptions()
                    {
                        AnimatedTextureKey = "EffectX",
                        Position = new BfbVector(simulationEntity.Position.X + 25, simulationEntity.Position.Y + 25),
                        Dimensions = new BfbVector(_random.Next(1, 100), _random.Next(1, 100)),
                        Rotation = 0,
                        Origin = new BfbVector(0, 0),
                    }, new ComponentOptions
                    {
                        Physics = new SpellEffects3PhysicsComponent()
                    });
                simulation.AddEntity(effect);
            }
        }

        private void CheckCollisions(SimulationEntity simulationEntity, Simulation simulation)
        {
            // Collisions with monsters
            List<SimulationEntity> targets = new List<SimulationEntity>();
            for (int i = 0; i < 10; i++)
            {
                int xPos = (int) simulationEntity.Position.X;
                SimulationEntity target = simulation.GetEntityAtPosition(xPos, (int) simulationEntity.Position.Y);
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
                ((CombatComponent)target.Combat).Health -= 15;
            }
        }
    }
}
