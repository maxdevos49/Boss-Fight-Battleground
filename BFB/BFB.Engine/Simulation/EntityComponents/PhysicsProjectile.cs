using System;
using System.Collections.Generic;
using BFB.Engine.Collisions;
using BFB.Engine.Entity;
using BFB.Engine.Helpers;
using BFB.Engine.Math;

namespace BFB.Engine.Simulation.EntityComponents
{
    public class PhysicsProjectile : EntityComponent, IPhysics
    {
        
        public BfbVector Acceleration { get; set; }
        public BfbVector MaxSpeed { get; set; }
        public float Gravity { get; set; }
        public float Friction { get; set; }

        private readonly bool _useGravity;
        
        private List<SimulationEntity> _targetsHit;
        private bool _hitTarget;

        public PhysicsProjectile(int maxSpeed = 40, bool useGravity = false) : base(false)
        {
            
            Acceleration = new BfbVector(maxSpeed,maxSpeed);
            MaxSpeed = new BfbVector(maxSpeed,maxSpeed);
            
            Gravity = 2f;
            Friction = 0.99f;

            _useGravity = useGravity;
            
            _targetsHit = new List<SimulationEntity>();
            _hitTarget = false;

        }

        public override void Init(SimulationEntity entity)
        {
            entity.SteeringVector.Normalize();
            entity.SteeringVector.X *= Acceleration.X;
            entity.SteeringVector.Y *= Acceleration.Y;
            entity.Velocity.Add(entity.SteeringVector);

        }

        public override void Update(SimulationEntity entity, Simulation simulation)
        {
            if (_useGravity)
            {
                entity.Velocity.Y += Gravity;
                entity.Velocity.X *= Friction;

                //Caps your speed in specific axis
                if(System.Math.Abs(entity.Velocity.X) > MaxSpeed.X)
                    entity.Velocity.X = entity.Velocity.X > 0 ? MaxSpeed.X : -MaxSpeed.X;
                
                if(System.Math.Abs(entity.Velocity.Y) > MaxSpeed.Y)
                    entity.Velocity.Y = entity.Velocity.Y > 0 ? MaxSpeed.Y : -MaxSpeed.Y;
                
                entity.Rotation = (float)System.Math.Atan2(entity.Velocity.Y, entity.Velocity.X) + (float)(System.Math.PI / 2);

            }

            entity.Position.Add(entity.Velocity);

            //Collision Test
            Collision.DetectCollision(simulation, entity);

            if (!_hitTarget) return;
            
            CombatService.FightPeople(entity,_targetsHit,simulation);
            simulation.RemoveEntity(entity.EntityId);
        }

        public override bool OnEntityCollision(Simulation simulation, SimulationEntity primaryEntity, SimulationEntity secondaryEntity)
        {
            if (secondaryEntity.EntityType != EntityType.Player &&
                secondaryEntity.EntityType != EntityType.Mob)
                return true;
            
            CollectTargets(simulation,primaryEntity,secondaryEntity);

            return true;
        }

        public override bool OnTileCollision(Simulation simulation, SimulationEntity entity, TileCollision tc)
        {
            simulation.RemoveEntity(entity.EntityId);
            return true;
        }

        public override bool OnWorldBoundaryCollision(Simulation simulation, SimulationEntity entity, CollisionSide side)
        {
            if(side != CollisionSide.TopBorder || !_useGravity)
                simulation.RemoveEntity(entity.EntityId);
            return false;
        }


        private void CollectTargets(Simulation simulation, SimulationEntity spell ,SimulationEntity target)
        {
            
            if (target.Meta != null && spell.EntityId != target.EntityId && target.EntityId != spell.ParentEntityId)
            {
                _targetsHit.Add(target);
                _hitTarget = true;//Dont remove entity here until next frame because we want to make sure we damage all entities
            }
        }

    }
}
