using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using BFB.Engine.Entity;
using BFB.Engine.Helpers;
using BFB.Engine.Inventory;
using JetBrains.Annotations;

namespace BFB.Engine.Simulation.ItemComponents
{
    public class HitComponent : IItemComponent
    {
        public void Use(Simulation simulation, SimulationEntity entity, IItem item)
        {
            if (entity.ControlState != null && entity.ControlState.LeftClick)
            {
                int reach = simulation.World.WorldOptions.WorldScale * item.Configuration.Reach;
                List<SimulationEntity> targets;

                if (entity.Facing == DirectionFacing.Left)
                    targets = simulation.World.QueryEntities(new Rectangle(entity.Left - reach, entity.Top + entity.Height/2,reach,2), new List<string> {"melee"}, entity.EntityId);
                else
                    targets = simulation.World.QueryEntities(new Rectangle(entity.Right, entity.Top + entity.Height/2,reach,2), new List<string> {"melee"}, entity.EntityId);

                CombatService.FightPeople(entity, targets, simulation);
            }
            
        }
    }
}