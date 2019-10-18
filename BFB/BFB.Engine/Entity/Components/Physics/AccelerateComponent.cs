using System;
using BFB.Engine.Math;
using JetBrains.Annotations;
using Microsoft.Xna.Framework;

namespace BFB.Engine.Entity.Components.Physics
{
    public class 
        
        AccelerateComponent: IPhysicsComponent
    {
        
        private readonly float _maxForce;

        private readonly float _maxSpeed;

        public AccelerateComponent()
        {
            _maxForce = 0.7f;
            _maxSpeed = 10;
        }

        public void Update(ServerEntity serverEntity)
        {

            //Enforce max speed
            serverEntity.DesiredVector.Magnitude = _maxSpeed;
            
            BfbVector steering = BfbVector.Sub(serverEntity.DesiredVector, serverEntity.Velocity);

            //enforce max force
            steering.Limit(_maxForce);

            //Apply steering to velocity
            serverEntity.Velocity.Add(steering);

            //update position
            serverEntity.Position.Add(serverEntity.Velocity);

            //update Rotation with degrees
            serverEntity.Rotation = Convert.ToSingle(System.Math.Atan2(serverEntity.Velocity.Y, serverEntity.Velocity.X));
            
        }
        
    }
}