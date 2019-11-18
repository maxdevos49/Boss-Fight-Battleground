using System;
using BFB.Engine.Entity;

namespace BFB.Engine.Simulation.SimulationComponents
{
    public class CombatComponent : ISimulationComponent
    { 
        // Combat
        public int Health;
        public int Mana;

        public CombatComponent()
        {
            Health = 20;
            Mana = 1000;
        }

        public void Update(SimulationEntity simulationEntity, Simulation simulation)
        {
            if (Health <= 0)
                OnDeath(simulationEntity, simulation);
        }

        private void OnDeath(SimulationEntity simulationEntity, Simulation simulation)
        {
            Console.WriteLine("YOU'VE DIED!");
            simulation.RemoveEntity(simulationEntity.EntityId);
        }

    }
}
