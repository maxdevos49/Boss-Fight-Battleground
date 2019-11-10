using System;
using System.Collections.Generic;
using System.Text;
using BFB.Engine.Entity;

namespace BFB.Engine.Simulation.PhysicsComponents
{
    public class CombatComponent : IPhysicsComponent
    { 
        // Combat
        private int health;
        private int mana;

        public CombatComponent()
        {
            health = 20;
            mana = 1000;
        }

        public void Update(SimulationEntity simulationEntity, Simulation simulation)
        {
            
        }
    }
}
