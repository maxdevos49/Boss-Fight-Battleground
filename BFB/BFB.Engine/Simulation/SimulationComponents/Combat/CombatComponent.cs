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
            
            if (entity.Meta?.Health <= 0)
                OnDeath(entity, simulation);
        }

        private void OnDeath(SimulationEntity entity, Simulation simulation)
        {
            if (entity.EntityType != EntityType.Player)
            {
                simulation.RemoveEntity(entity.EntityId);
                return;
            }

            entity.Position.X = 100;
            entity.Position.Y = 100;
            
            if (entity.Meta != null) 
                entity.Meta.Health = 20;
            Console.WriteLine("YOU'VE DIED!");
//            simulation.RemoveEntity(entity.EntityId);
        }

    }
}
