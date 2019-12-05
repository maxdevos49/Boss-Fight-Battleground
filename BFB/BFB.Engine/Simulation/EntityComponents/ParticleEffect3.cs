using System;
using BFB.Engine.Entity;

namespace BFB.Engine.Simulation.EntityComponents
{
    class ParticleEffect3 : EntityComponent
    {
        private readonly Random _random;
        private readonly int _range;

        public ParticleEffect3() : base(false)
        {
            _random = new Random();
            _range = 30;
        }
        public override void Update(SimulationEntity simulationEntity, Simulation simulation)
        {
            //Creates the new velocity
            simulationEntity.Velocity.X = _random.Next(-_range, _range);
            simulationEntity.Velocity.Y = _random.Next(-_range, _range);

            //Updates the position
            simulationEntity.Position.Add(simulationEntity.Velocity);
        }
    }
}
