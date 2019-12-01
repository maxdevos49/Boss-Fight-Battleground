using System;
using BFB.Engine.Collisions;
using BFB.Engine.Entity;
using BFB.Engine.Math;

namespace BFB.Engine.Simulation.EntityComponents
{
    public class BombSpellPhysicsComponent : EntityComponent
    {
        private BfbVector _acceleration;
        private BfbVector _gravity;
        private Random _random;
        private bool _hitTarget;

        public BombSpellPhysicsComponent() : base(false) {}

        #region Init

        public override void Init(SimulationEntity entity)
        {
            _acceleration = new BfbVector(10 * 25, -20);
            _gravity = new BfbVector(0, 4f);
            _random = new Random();

            _hitTarget = false;
            
            entity.Velocity.Y = _acceleration.Y;
        }

        #endregion
        
        public override void Update(SimulationEntity entity, Simulation simulation)
        {
            entity.Velocity.X = _acceleration.X;
            entity.Velocity.Y += _gravity.Y;
            
            //Updates the position
            entity.Position.Add(entity.Velocity);

            //Detect collisions
            Collision.DetectCollision(simulation,entity);
            
            
            if(_hitTarget)
                simulation.RemoveEntity(entity.EntityId, EntityRemovalReason.Other);
           
        }

        public override void OnSimulationRemove(Simulation simulation, SimulationEntity entity, EntityRemovalReason? reason)
        {
            Explode(simulation,entity);
        }

        public override bool OnEntityCollision(Simulation simulation, SimulationEntity primaryEntity, SimulationEntity secondaryEntity)
        {
            if (secondaryEntity.EntityId != primaryEntity.EntityId)
            {
                DamageTarget(secondaryEntity);
                _hitTarget = true;
            }

            return true;
        }


        private void DamageTarget(SimulationEntity target)
        {
                // Instead of a hard coded value here, you could call a weapon stored on the simulationEntity, and use its damage value.
                if (target.Meta != null) 
                    target.Meta.Health -= 15;
        }
        
        private void Explode(Simulation simulation,SimulationEntity entity)
        {
            for (int i = 0; i < 50; i++)
            {
                if (simulation.World.ChunkFromPixelLocation((int)entity.Position.X, (int)entity.Position.Y) == null)
                    return;

                SimulationEntity effect = new SimulationEntity(
                    Guid.NewGuid().ToString(),
                    new EntityOptions()
                    {
                        TextureKey = "EffectMiniStar",
                        Position = new BfbVector(entity.Position.X + 50, entity.Position.Y + 50),
                        Dimensions = new BfbVector(_random.Next(1, 100), _random.Next(1, 100)),
                        Rotation = 0,
                        Origin = new BfbVector(0, 0),
                        EntityType = EntityType.Projectile
                    }//, new List<EntityComponent>
                    //{
                     //  new LifetimeComponent(10),
                     //   new SpellEffects3PhysicsComponent()
                    //}
                    );
                
                simulation.AddEntity(effect);
                
                effect = new SimulationEntity(
                    Guid.NewGuid().ToString(),
                    new EntityOptions()
                    {
                        TextureKey = "EffectStar",
                        Position = new BfbVector(entity.Position.X + 25, entity.Position.Y + 25),
                        Dimensions = new BfbVector(_random.Next(1, 100), _random.Next(1, 100)),
                        Rotation = 0,
                        Origin = new BfbVector(0, 0),
                        EntityType = EntityType.Projectile
                    }//, new List<EntityComponent>
                    //{
                      //  new LifetimeComponent(10),
                       // new SpellEffects3PhysicsComponent()
                    //}
                    );
                simulation.AddEntity(effect);
                
                effect = new SimulationEntity(
                    Guid.NewGuid().ToString(),
                    new EntityOptions()
                    {
                        TextureKey = "EffectX",
                        Position = new BfbVector(entity.Position.X + 25, entity.Position.Y + 25),
                        Dimensions = new BfbVector(_random.Next(1, 100), _random.Next(1, 100)),
                        Rotation = 0,
                        Origin = new BfbVector(0, 0),
                        EntityType = EntityType.Projectile
                    }//, new List<EntityComponent>
                    //{
                     //   new LifetimeComponent(10),
                      //  new SpellEffects3PhysicsComponent()
                    //}
                    );
                simulation.AddEntity(effect);
            }
        }
        
    }
}
