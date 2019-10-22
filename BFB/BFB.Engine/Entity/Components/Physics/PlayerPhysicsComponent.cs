using System;
using System.Reflection.PortableExecutable;
using BFB.Engine.Content;
using BFB.Engine.Entity.Components.Input;
using BFB.Engine.Math;
using JetBrains.Annotations;
using Microsoft.Xna.Framework;

namespace BFB.Engine.Entity.Components.Physics
{
    public class 
        PlayerPhysicsComponent: IPhysicsComponent
    {
        private readonly BfbVector _acceleration;
        private readonly BfbVector _maxSpeed;
        private readonly BfbVector _gravity;
        private readonly float _friction;
        private AnimationState _previousAnimationState;
        
        
        public PlayerPhysicsComponent()
        {
            _acceleration = new BfbVector(1,15);
            _maxSpeed = new BfbVector(10,20);
            _gravity = new BfbVector(0, 0.98f);
            _friction = 0.2f;

        }

        public void Update(ServerEntity serverEntity)
        {
            
            //Gives us the speed to move left and right
            serverEntity.DesiredVector.X *= _acceleration.X;
            serverEntity.DesiredVector.Y *= _acceleration.Y;
            
            if (!serverEntity.Grounded)
                serverEntity.DesiredVector.Add(_gravity);
            
            
            //Creates the new velocity
            serverEntity.Velocity.Add(serverEntity.DesiredVector);


            //Caps your speed
            if(System.Math.Abs(serverEntity.Velocity.X) > _maxSpeed.X)
                serverEntity.Velocity.X = serverEntity.Velocity.X > 0 ? _maxSpeed.X : -_maxSpeed.X;
            
            if(System.Math.Abs(serverEntity.Velocity.Y) > _maxSpeed.Y)
                serverEntity.Velocity.Y = serverEntity.Velocity.Y > 0 ? _maxSpeed.Y : -_maxSpeed.Y;
            
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (serverEntity.Grounded && serverEntity.DesiredVector.X == 0)
            {
                //This will affects changing directions back and forth -- so its like smash bros switching directions
                serverEntity.Velocity.Mult(_friction);
            }

            if (System.Math.Abs(serverEntity.Velocity.X) < 1)
                serverEntity.Velocity.X = 0;

            //Updates the position
            serverEntity.Position.Add(serverEntity.Velocity);

            if(serverEntity.Position.Y > 200)
            {
                serverEntity.Position.Y = 200;
                serverEntity.Velocity.Y = 0;
                serverEntity.Grounded = true;
            }

            //Animation states
            if (serverEntity.Velocity.X > 1)
            {
                serverEntity.AnimationState = AnimationState.MoveRight;
            }
            else if (serverEntity.Velocity.X < 1)
            {

                serverEntity.AnimationState = AnimationState.MoveLeft;
            }
            else if (_previousAnimationState == AnimationState.MoveLeft &&  _previousAnimationState != AnimationState.IdleRight)
            {
                serverEntity.AnimationState = AnimationState.IdleLeft;
            }
            else if(_previousAnimationState != AnimationState.IdleLeft)
            {
                serverEntity.AnimationState = AnimationState.IdleRight;
            }


            _previousAnimationState = serverEntity.AnimationState;

        }
    }
}