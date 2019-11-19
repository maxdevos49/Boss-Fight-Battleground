using System;
using System.Collections.Generic;
using BFB.Engine.Entity;
using BFB.Engine.Helpers;

namespace BFB.Engine.Simulation.SimulationComponents.Combat
{
    public class CombatComponent : SimulationComponent
    { 
        public CombatComponent() : base(false) { }

        public override void Init(SimulationEntity entity)
        {
            if(entity.Meta == null)
                entity.Meta = new EntityMeta();
            
            entity.Meta.Health = 20;
            entity.Meta.Mana = 1000;
        }

        public override void Update(SimulationEntity entity, Simulation simulation)
        {
            //TODO temp Combat
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
                }

                //Apply actual damage
                CombatService.FightPeople(entity, targets, simulation);
                
            }
            
            if (entity.Meta?.Health <= 0)
                OnDeath(entity, simulation);
        }

        private void OnDeath(SimulationEntity entity, Simulation simulation)
        {
            Console.WriteLine("YOU'VE DIED!");
            simulation.RemoveEntity(entity.EntityId);
        }

    }
}
