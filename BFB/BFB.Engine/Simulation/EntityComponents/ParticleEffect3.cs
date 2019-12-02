using System;
using BFB.Engine.Entity;

namespace BFB.Engine.Simulation.EntityComponents
{
    class ParticleEffect3 : EntityComponent
    {
        private readonly Random _random;

        public ParticleEffect3() : base(false)
        {
            _random = new Random();
        }
        public override void Update(SimulationEntity simulationEntity, Simulation simulation)
        {
            //Creates the new velocity
            simulationEntity.Velocity.X = _random.Next(-10, 10);
            simulationEntity.Velocity.Y = _random.Next(-10, 10);

            //Updates the position
            simulationEntity.Position.Add(simulationEntity.Velocity);
        }
    }
}
