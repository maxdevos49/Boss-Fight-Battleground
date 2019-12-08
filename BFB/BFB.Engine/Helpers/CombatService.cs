using System.Collections.Generic;
using BFB.Engine.Entity;

namespace BFB.Engine.Helpers
{
    public static class CombatService
    {

        public static void FightPeople(SimulationEntity entity, List<SimulationEntity> targets, Simulation.Simulation simulation)
        {
            if (targets.Count <= 0) return;
            
            ushort damage = entity.Inventory?.GetActiveSlot()?.Configuration?.Damage ?? 0;

            
            foreach (SimulationEntity target in targets)
            {
                if (target.Meta == null) continue;
                
                target.Meta.Health -= damage;

                float knockBack = (float)System.Math.Pow(2, damage);
                if (entity.Facing == DirectionFacing.Left)
                {
                    target.Velocity.Y = -knockBack/2;
                    target.Velocity.X = -knockBack;
                }
                else
                {
                    target.Velocity.Y = -knockBack/2;
                    target.Velocity.X = knockBack;
                }
            }
        }
    }
}
