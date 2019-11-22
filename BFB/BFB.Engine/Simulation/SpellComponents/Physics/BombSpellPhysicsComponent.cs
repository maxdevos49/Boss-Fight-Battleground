using System;
using System.Collections.Generic;
using System.Text;
using BFB.Engine.Collisions;
using BFB.Engine.Entity;
using BFB.Engine.Math;
using BFB.Engine.Simulation.PhysicsComponents;
using BFB.Engine.Simulation.SimulationComponents;
using Microsoft.Xna.Framework;

namespace BFB.Engine.Simulation.SpellComponents.Physics
{
    public class BombSpellPhysicsComponent : EntityComponent
    {
        private int _timeToLive;
        private SimulationEntity _owner;
        private BfbVector _acceleration;
        private BfbVector _gravity;
        private bool _setInitialVelocity;
        private Random _random;

        public BombSpellPhysicsComponent(BfbVector direction, SimulationEntity owner) : base(false)
        {
            //assign direction
            //assign entity parent
        }

        #region Init

        public override void Init(SimulationEntity entity)
        {
            _timeToLive = 10;
            
            _acceleration = new BfbVector(10 * 25, -20);
            _gravity = new BfbVector(0, 4f);
            _owner = entity;//TODO
            _setInitialVelocity = false;
            _random = new Random();
        }

        #endregion
        
        public override void Update(SimulationEntity simulationEntity, Simulation simulation)
        {
            if (!_setInitialVelocity)
            {
                _setInitialVelocity = true;
                simulationEntity.Velocity.Y = _acceleration.Y;
            }

            _timeToLive -= 1;
            if (_timeToLive <= 0)
            {
                Explode(simulationEntity, simulation);
                simulation.RemoveEntity(simulationEntity.EntityId);
            }

            simulationEntity.Velocity.X = _acceleration.X;
            simulationEntity.Velocity.Y += _gravity.Y;

            //Updates the position
            simulationEntity.Position.Add(simulationEntity.Velocity);

//            CheckCollisions(_owner, simulation);
        }

        #region Explode 
        
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
                        TextureKey = "EffectMiniStar",
                        Position = new BfbVector(simulationEntity.Position.X + 50, simulationEntity.Position.Y + 50),
                        Dimensions = new BfbVector(_random.Next(1, 100), _random.Next(1, 100)),
                        Rotation = 0,
                        Origin = new BfbVector(0, 0),
                    }, new List<EntityComponent>
                    {
                        new SpellEffects3PhysicsComponent()
                    });
                
                simulation.AddEntity(effect);
                
                effect = new SimulationEntity(
                    Guid.NewGuid().ToString(),
                    new EntityOptions()
                    {
                        TextureKey = "EffectStar",
                        Position = new BfbVector(simulationEntity.Position.X + 25, simulationEntity.Position.Y + 25),
                        Dimensions = new BfbVector(_random.Next(1, 100), _random.Next(1, 100)),
                        Rotation = 0,
                        Origin = new BfbVector(0, 0),
                    }, new List<EntityComponent>
                    {
                        new SpellEffects3PhysicsComponent()
                    });
                simulation.AddEntity(effect);
                
                effect = new SimulationEntity(
                    Guid.NewGuid().ToString(),
                    new EntityOptions()
                    {
                        TextureKey = "EffectX",
                        Position = new BfbVector(simulationEntity.Position.X + 25, simulationEntity.Position.Y + 25),
                        Dimensions = new BfbVector(_random.Next(1, 100), _random.Next(1, 100)),
                        Rotation = 0,
                        Origin = new BfbVector(0, 0),
                    }, new List<EntityComponent>
                    {
                        new SpellEffects3PhysicsComponent()
                    });
                simulation.AddEntity(effect);
            }
        }
        
        #endregion


//        private void CheckCollisions(SimulationEntity simulationEntity, Simulation simulation)
//        {
//            // Collisions with monsters
//            List<SimulationEntity> targets = new List<SimulationEntity>();
//            for (int i = 0; i < 10; i++)
//            {
//                int xPos = (int) simulationEntity.Position.X;
//                SimulationEntity target = simulation.GetEntityAtPosition(xPos, (int) simulationEntity.Position.Y);
//                if (target != null && target != simulationEntity && !targets.Contains(target))
//                    targets.Add(target);
//            }
//
//            targets.Remove(_owner);
//            if (targets.Count >= 1)
//            {
////                DamageTargets(targets);
//                simulation.RemoveEntity(simulationEntity.EntityId);
//            }
//        }

//        private void DamageTargets(List<SimulationEntity> targets)
//        {
//            foreach (SimulationEntity target in targets)
//            {
//                // Instead of a hard coded value here, you could call a weapon stored on the simulationEntity, and use its damage value.
//                ((CombatComponent)target.Combat).Health -= 15;
//            }
//        }
    }
}
