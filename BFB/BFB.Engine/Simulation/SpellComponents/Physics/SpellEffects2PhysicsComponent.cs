using System;
using System.Collections.Generic;
using System.Text;
using BFB.Engine.Entity;
using BFB.Engine.Math;
using BFB.Engine.Simulation.PhysicsComponents;
using BFB.Engine.Simulation.SimulationComponents;

namespace BFB.Engine.Simulation.SpellComponents.Physics
{
    class SpellEffects2PhysicsComponent : EntityComponent
    {
        private int _timeToLive;
        private readonly BfbVector _acceleration;
        private readonly Random _random;

        public SpellEffects2PhysicsComponent() : base(false)
        {
            _timeToLive = 15;
            _acceleration = new BfbVector(1,1);
            _random = new Random();
        }
        public override void Update(SimulationEntity simulationEntity, Simulation simulation)
        {
            if (_random.Next(10) % 2 == 0)
                _timeToLive -= 1;
            if (_timeToLive <= 0)
                simulation.RemoveEntity(simulationEntity.EntityId);

            //Gives us the speed to move left and right
            simulationEntity.SteeringVector.X = System.Math.Abs(_acceleration.X * _random.Next(1, 10));
            simulationEntity.SteeringVector.Y = System.Math.Abs(_acceleration.Y * _random.Next(1, 2) * 0);

            //Creates the new velocity
            simulationEntity.Velocity.X = simulationEntity.SteeringVector.X;
            simulationEntity.Velocity.Y = simulationEntity.SteeringVector.Y;

            //Updates the position
            simulationEntity.Position.Add(simulationEntity.Velocity);
        }
    }
}
