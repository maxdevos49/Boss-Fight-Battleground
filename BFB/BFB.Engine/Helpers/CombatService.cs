using System;
using System.Collections.Generic;
using System.Text;
using BFB.Engine.Entity;
using BFB.Engine.Simulation.PhysicsComponents;

namespace BFB.Engine.Helpers
{
    public static class CombatService
    {

        public static void FightPeople(SimulationEntity simulationEntity, List<SimulationEntity> targets, Simulation.Simulation simulation)
        {
            if (targets.Count <= 0) return;
            foreach (SimulationEntity target in targets)
            {
                // Instead of a hard coded value here, you could call a weapon stored on the simulationEntity, and use its damage value.
                ((CombatComponent) target.Combat).Health -= 4;
            }
        }
    }
}
