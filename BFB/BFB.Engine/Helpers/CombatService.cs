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
                if (target.Meta != null)
                {
                    target.Meta.Health -= 4;
                    if (simulationEntity.Facing == DirectionFacing.Left)
                    {
                        target.Velocity.Y = -10;
                        target.Velocity.X = -10;
                    }
                    else
                    {
                        target.Velocity.Y = -10;
                        target.Velocity.X = 10;
                    }
                }

//                ((CombatComponent) target.Combat).Health -= 4;//TODO
            }
        }
    }
}
