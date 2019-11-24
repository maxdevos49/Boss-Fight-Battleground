using System;
using BFB.Engine.Collisions;
using BFB.Engine.Entity;
using BFB.Engine.Math;

namespace BFB.Engine.Simulation.EntityComponents.Physics
{
    public class SpellHeadingPhysicsComponent : EntityComponent
    {
        private BfbVector _acceleration;
        private bool _hitTarget;

        public SpellHeadingPhysicsComponent() : base(false) { }

        public override void Init(SimulationEntity entity)
        {
            _hitTarget = false;
            entity.SteeringVector.Normalize();
            _acceleration = new BfbVector(entity.SteeringVector.X, entity.SteeringVector.Y);
        }

        public override void Update(SimulationEntity entity, Simulation simulation)
        {
            //Creates the new velocity
            entity.Velocity.X = _acceleration.X * 30;
            entity.Velocity.Y = _acceleration.Y  * 30;
            
            //Updates the position
            entity.Position.Add(entity.Velocity);

            //Detect collisions
            Collision.DetectCollision(simulation, entity);
            
            if(_hitTarget)
                simulation.RemoveEntity(entity.EntityId);
        }

        public override bool OnEntityCollision(Simulation simulation, SimulationEntity primaryEntity, SimulationEntity secondaryEntity)
        {
            if (secondaryEntity.EntityType != EntityType.Player &&
                secondaryEntity.EntityType != EntityType.Mob)
                return true;
            
            DamageTarget(primaryEntity,secondaryEntity);

            return true;
        }

        public override bool OnTileCollision(Simulation simulation, SimulationEntity entity, TileCollision tc)
        {
            simulation.RemoveEntity(entity.EntityId);
            return true;
        }

        public override bool OnWorldBoundaryCollision(Simulation simulation, SimulationEntity entity, CollisionSide side)
        {
            simulation.RemoveEntity(entity.EntityId);
            return false;
        }


        private void DamageTarget(SimulationEntity attacker ,SimulationEntity target)
        {
            //TODO get damage
            
            // Instead of a hard coded value here, you could call a weapon stored on the simulationEntity, and use its damage value.
            if (target.Meta != null && attacker.EntityId != target.EntityId)
            {
                target.Meta.Health -= 10;
                _hitTarget = true;//Dont remove entity here until next frame because we want to make sure we damage all entities
            }
        }
    }
}
