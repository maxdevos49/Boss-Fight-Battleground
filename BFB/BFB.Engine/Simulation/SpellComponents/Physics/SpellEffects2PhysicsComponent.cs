using System;
using System.Collections.Generic;
using System.Text;
using BFB.Engine.Entity;
using BFB.Engine.Math;
using BFB.Engine.Simulation.PhysicsComponents;

namespace BFB.Engine.Simulation.SpellComponents.Physics
{
    class SpellEffects2PhysicsComponent : IPhysicsComponent
    {
        private int timeToLive;
        private readonly BfbVector _acceleration;
        private Random _random;

        public SpellEffects2PhysicsComponent()
        {
            timeToLive = 15;
            _acceleration = new BfbVector(1,1);
            _random = new Random();
        }
        public void Update(SimulationEntity simulationEntity, Simulation simulation)
        {
            if (_random.Next(10) % 2 == 0)
                timeToLive -= 1;
            if (timeToLive <= 0)
                simulation.RemoveEntity(simulationEntity.EntityId);

            //Gives us the speed to move left and right
            simulationEntity.DesiredVector.X = System.Math.Abs(_acceleration.X * _random.Next(1, 10));
            simulationEntity.DesiredVector.Y = System.Math.Abs(_acceleration.Y * _random.Next(1, 2) * 0);

            //Creates the new velocity
            simulationEntity.Velocity.X = simulationEntity.DesiredVector.X;
            simulationEntity.Velocity.Y = simulationEntity.DesiredVector.Y;

            //Updates the position
            simulationEntity.Position.Add(simulationEntity.Velocity);
        }
    }
}
