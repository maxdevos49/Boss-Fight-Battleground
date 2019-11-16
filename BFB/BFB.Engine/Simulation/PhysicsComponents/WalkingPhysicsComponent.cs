using System;
using System.Collections.Generic;
using BFB.Engine.Content;
using BFB.Engine.Entity;
using BFB.Engine.Math;
using BFB.Engine.TileMap;

namespace BFB.Engine.Simulation.PhysicsComponents
{
    /// <summary>
    /// A player physics component that is used for normal walking like physics
    /// </summary>
    public class  WalkingPhysicsComponent: IPhysicsComponent
    {
        private readonly BfbVector _acceleration;
        private readonly BfbVector _maxSpeed;
        private readonly float _gravity;
        private readonly float _friction;
        
        public string CollideFilter { get; set; }
        public List<string> CollideWithFilters { get; set; }
        
        
        /// <summary>
        /// Constructs a Walking Physics Component
        /// </summary>
        public WalkingPhysicsComponent(string collideFilter, List<string> collideWithFilters, WalkingConfiguration options)
        {
            
            CollideFilter = collideFilter ?? "entity";
            CollideWithFilters = collideWithFilters ?? new List<string> {"tile"};
            
            _acceleration = options?.Acceleration ?? new BfbVector(5,25);
            _maxSpeed = options?.MaxSpeed ?? new BfbVector(20,40);
            _gravity = options?.Gravity ?? 4f;
            _friction = options?.Friction ?? 0.8f;

        }

        public void Update(SimulationEntity entity, Simulation simulation)
        {
            
            entity.SteeringVector.X *= _acceleration.X;
            entity.SteeringVector.Y *= _acceleration.Y;
            
            entity.Velocity.Add(entity.SteeringVector);
            
            entity.Velocity.Y += _gravity;
            entity.Velocity.X *= _friction;

            //Caps your speed in specific axis
            if(System.Math.Abs(entity.Velocity.X) > _maxSpeed.X)
                entity.Velocity.X = entity.Velocity.X > 0 ? _maxSpeed.X : -_maxSpeed.X;
            
            if(System.Math.Abs(entity.Velocity.Y) > _maxSpeed.Y)
                entity.Velocity.Y = entity.Velocity.Y > 0 ? _maxSpeed.Y : -_maxSpeed.Y;
            
            entity.Position.Add(entity.Velocity);

            //Animation states
            if (entity.Velocity.X > 2)
            {
                entity.Facing = DirectionFacing.Right;
                entity.AnimationState = AnimationState.MoveRight;
            }
            else if (entity.Velocity.X < -2)
            {
                entity.Facing = DirectionFacing.Left;
                entity.AnimationState = AnimationState.MoveLeft;
            }
            else
            {
                entity.Velocity.X = 0;
                entity.AnimationState = (entity.Facing == DirectionFacing.Left)
                    ? AnimationState.IdleLeft
                    : AnimationState.IdleRight;
            }
            
            //Collision Test
            this.DetectCollision(simulation, entity);
        }

        public bool OnEntityCollision(Simulation simulation, EntityCollision entityCollision)
        {
            return true;//TODO
        }

        public bool OnTileCollision(Simulation simulation, SimulationEntity entity, TileCollision tc)
        {
            
            //Auto Jump
            if (tc.Side == CollisionSide.RightBorder || tc.Side == CollisionSide.LeftBorder)
            {
                if (tc.BottomBlockY != (int) tc.TilePosition.Y + 1 || simulation.World.GetBlock(tc.LeftBlockX, (int) tc.TilePosition.Y - 1) != WorldTile.Air)
                    return true;
                            
                entity.Position.Y -= simulation.World.WorldOptions.WorldScale + 4;
                return false;
            }

            return true;
        }

        public bool OnWorldBoundaryCollision(Simulation simulation, SimulationEntity entity, CollisionSide side)
        {
            return true;
        }
    }

    public class WalkingConfiguration
    {
        public BfbVector Acceleration { get; set; }
        
        public BfbVector MaxSpeed { get; set; }
        
        public float Gravity { get; set; }
        
        public float Friction { get; set; }
        
    }
}