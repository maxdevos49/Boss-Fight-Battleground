using System;
using BFB.Engine.Math;
using JetBrains.Annotations;
using Microsoft.Xna.Framework;

namespace BFB.Engine.Entity.Components.Physics
{
    public class AccelerateComponent: IPhysicsComponent
    {
        
        private readonly float _maxForce;

        private readonly float _maxSpeed;

        public AccelerateComponent()
        {
            _maxForce = 10;
            _maxSpeed = 10;
        }

        public void Update(ServerEntity serverEntity)
        {
            BfbVector desiredVector = serverEntity.DesiredVector;

            //Enforce max speed
            desiredVector.Limit(_maxSpeed);
            
            BfbVector steering = BfbVector.Sub(desiredVector, serverEntity.Velocity);

            //enforce max force
            desiredVector.Limit(_maxForce);

            //Apply steering to velocity
            serverEntity.Velocity = BfbVector.Add(steering, serverEntity.Velocity);

            //update position
            serverEntity.Position = BfbVector.Add(serverEntity.Velocity, serverEntity.Position);

            //update Rotation with degrees
            serverEntity.Rotation = Convert.ToSingle(System.Math.Atan2(serverEntity.Velocity.Y, serverEntity.Velocity.X) - System.Math.PI / 2);
            
        }
        
    }
}