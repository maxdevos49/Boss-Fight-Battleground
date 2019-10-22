using System;
using System.Reflection.PortableExecutable;
using BFB.Engine.Entity.Components.Input;
using BFB.Engine.Math;
using JetBrains.Annotations;
using Microsoft.Xna.Framework;

namespace BFB.Engine.Entity.Components.Physics
{
    public class 
        PlayerPhysicsComponent: IPhysicsComponent
    {
        private readonly float _maxForce;
        private readonly float _accelerationSpeed;
        private readonly float _maxSpeed;
        private readonly BfbVector _gravity;
        private readonly float _friction;
        
        
        public PlayerPhysicsComponent()
        {
            _accelerationSpeed = 6;
            _maxForce = 0.7f;
            _maxSpeed = 50;
            _gravity = new BfbVector(0, 0.98f);
            _friction = 0.5f;

        }

        public void Update(ServerEntity serverEntity)
        {


            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (!serverEntity.Grounded)
            {
                serverEntity.Velocity.Add(_gravity);
            }
            
            
            //Gives us the speed to move left and right
            serverEntity.DesiredVector.Magnitude = _accelerationSpeed;
            
            

            //Creates the new velocity
            serverEntity.Velocity.Add(serverEntity.DesiredVector);
            

            //Caps your speed
            serverEntity.Velocity.Limit(_maxSpeed);

            
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (serverEntity.Grounded && serverEntity.DesiredVector.X == 0)
            {
                //This will affects changing directions back and forth -- so its like smash bros switching directions
                serverEntity.DesiredVector.Limit(_maxForce);
                serverEntity.Velocity.Mult(_friction);
            }
            
            //Updates the position
            serverEntity.Position.Add(serverEntity.Velocity);

            if(serverEntity.Position.Y > 200)
            {
                serverEntity.Position.Y = 200;
                serverEntity.Velocity.Y = 0;
                serverEntity.Grounded = true;
            }
            
        }
    }
}