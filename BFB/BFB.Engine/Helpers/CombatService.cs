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
            ushort knockback = entity.Inventory?.GetActiveSlot()?.Configuration?.KnockBack ?? 20;

            
            foreach (SimulationEntity target in targets)
            {
                if (target.Meta == null || (entity.EntityConfiguration.EntityKey == target.EntityConfiguration.EntityKey)) continue;
                
                target.Meta.Health -= damage;

                if (entity.Facing == DirectionFacing.Left)
                {
                    target.Velocity.Y = -knockback/2f;
                    target.Velocity.X = -knockback;
                }
                else
                {
                    target.Velocity.Y = -knockback/2f;
                    target.Velocity.X = knockback;
                }
            }
        }
    }
}
