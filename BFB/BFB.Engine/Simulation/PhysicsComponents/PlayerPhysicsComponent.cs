using System;
using BFB.Engine.Content;
using BFB.Engine.Entity;
using BFB.Engine.Math;

namespace BFB.Engine.Simulation.PhysicsComponents
{
    /// <summary>
    /// A player physics component that is used for normal walking like physics
    /// </summary>
    public class  PlayerPhysicsComponent: IPhysicsComponent
    {
        private readonly BfbVector _acceleration;
        private readonly BfbVector _maxSpeed;
        private readonly BfbVector _gravity;
        private readonly float _friction;
        private AnimationState _previousAnimationState;

        /// <summary>
        /// Constructs a player physics component.
        /// </summary>
        public PlayerPhysicsComponent()
        {
            _acceleration = new BfbVector(5, 25);
            _maxSpeed = new BfbVector(20, 40);
            _gravity = new BfbVector(0, 4f);
            _friction = 0.8f;

        }

        public void Update(SimulationEntity entity, Simulation simulation)
        {
            
            //Gives us the speed to move left and right
            entity.DesiredVector.X *= _acceleration.X;
            entity.DesiredVector.Y *= _acceleration.Y;
            
            entity.DesiredVector.Add(_gravity);
            
            //Creates the new velocity
            entity.Velocity.Add(entity.DesiredVector);
            
            entity.Velocity.X *= _friction;

            //Caps your speed
            if(System.Math.Abs(entity.Velocity.X) > _maxSpeed.X)
                entity.Velocity.X = entity.Velocity.X > 0 ? _maxSpeed.X : -_maxSpeed.X;
            
            if(System.Math.Abs(entity.Velocity.Y) > _maxSpeed.Y)
                entity.Velocity.Y = entity.Velocity.Y > 0 ? _maxSpeed.Y : -_maxSpeed.Y;
            
            //Updates the position
            entity.Position.Add(entity.Velocity);

            Collision.DetectCollision(entity,simulation);
            
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (System.Math.Abs(entity.Velocity.X) < 2 && entity.Velocity.X != 0)
            {
                entity.Velocity.X = 0;
                entity.AnimationState = _previousAnimationState == AnimationState.MoveLeft ? AnimationState.IdleLeft : AnimationState.IdleRight;
            }

            //Animation states
            if (entity.Velocity.X > 1)
            {
                entity.AnimationState = AnimationState.MoveRight;
                entity.isFacingRight = true;
            }
            else if (entity.Velocity.X < -1)
            {
                entity.AnimationState = AnimationState.MoveLeft;
                entity.isFacingRight = false;
            }

            _previousAnimationState = entity.AnimationState;

        }
    }
}