using System;
using System.Collections.Generic;
using System.Text;
using BFB.Engine.Entity;
using BFB.Engine.Math;
using Microsoft.Xna.Framework;

namespace BFB.Engine.Simulation.PhysicsComponents
{
    class SpellPhysicsComponent : IPhysicsComponent
    {
        private int timeToLive;
        private readonly BfbVector _acceleration;
        private readonly BfbVector _maxSpeed;

        public SpellPhysicsComponent(BfbVector direction)
        {
            Vector2 directionNorm = direction.ToVector2();
            directionNorm.Normalize();
            timeToLive = 30;
            float xAcc = directionNorm.X;
            float yAcc = directionNorm.Y;
            _acceleration = new BfbVector(xAcc, yAcc);
            _maxSpeed = new BfbVector(15, 15);
        }

        public void Update(SimulationEntity simulationEntity, Simulation simulation)
        {
            timeToLive -= 1;
            if (timeToLive <= 0)
                simulation.RemoveEntity(simulationEntity.EntityId);

            //Gives us the speed to move left and right
            simulationEntity.DesiredVector.X = _acceleration.X * 30;
            simulationEntity.DesiredVector.Y = _acceleration.Y * 30;

            //Creates the new velocity
            simulationEntity.Velocity.X = simulationEntity.DesiredVector.X;
            simulationEntity.Velocity.Y = simulationEntity.DesiredVector.Y;

            //Updates the position
            simulationEntity.Position.Add(simulationEntity.Velocity);

        }
    }
}
