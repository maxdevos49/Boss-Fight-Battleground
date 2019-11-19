using System;
using System.Collections.Generic;
using BFB.Engine.Entity;
using BFB.Engine.Helpers;
using BFB.Engine.Inventory;

namespace BFB.Engine.Simulation.ItemComponents
{
    public class HitComponent : IItemComponent
    {
        public void Use(Simulation simulation, SimulationEntity entity, IItem item)
        {
            
            
            if (entity.ControlState != null && entity.ControlState.LeftClick)
            {

                List<SimulationEntity> targets = new List<SimulationEntity>();
                //check each pixel 100 pixels in front of the player
                for (int i = 0; i < 100; i += simulation.World.WorldOptions.WorldScale/2)//We only need to check every half tile size to be accurate
                {
                    int xPos = (int) entity.Position.X + i;
                    if (entity.Facing == DirectionFacing.Left)
                        xPos = (int) entity.Position.X - i;

                    //get possible entity at location
                    SimulationEntity target = simulation.GetEntityAtPosition(xPos, (int) entity.Position.Y);
                    
                    //determine if to add the entities
                    if (target != null && target != entity && !targets.Contains(target))
                        targets.Add(target);
                    
                    Console.WriteLine(targets.Count);

                }

                //Apply actual damage
                CombatService.FightPeople(entity, targets, simulation);
                
            }
            
        }
    }
}