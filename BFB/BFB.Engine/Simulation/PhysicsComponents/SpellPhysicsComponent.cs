using System;
using System.Collections.Generic;
using System.Text;
using BFB.Engine.Entity;
using BFB.Engine.Math;

namespace BFB.Engine.Simulation.PhysicsComponents
{
    class SpellPhysicsComponent : IPhysicsComponent
    {
        private int timeToLive;
        private readonly BfbVector _acceleration;
        private readonly BfbVector _maxSpeed;

        public SpellPhysicsComponent()
        {
            timeToLive = 100;
            _acceleration = new BfbVector(5, 25);
            _maxSpeed = new BfbVector(20, 40);
        }

        public void Update(SimulationEntity simulationEntity, Simulation simulation)
        {
            timeToLive -= 1;

            //Gives us the speed to move left and right
            simulationEntity.DesiredVector.X *= _acceleration.X;
            simulationEntity.DesiredVector.Y *= _acceleration.Y;


            //Creates the new velocity
            simulationEntity.Velocity.Add(simulationEntity.DesiredVector);

            //Caps your speed
            if (System.Math.Abs(simulationEntity.Velocity.X) > _maxSpeed.X)
                simulationEntity.Velocity.X = simulationEntity.Velocity.X > 0 ? _maxSpeed.X : -_maxSpeed.X;

            if (System.Math.Abs(simulationEntity.Velocity.Y) > _maxSpeed.Y)
                simulationEntity.Velocity.Y = simulationEntity.Velocity.Y > 0 ? _maxSpeed.Y : -_maxSpeed.Y;

            //Updates the position
            simulationEntity.Position.Add(simulationEntity.Velocity);

        }
    }
}
