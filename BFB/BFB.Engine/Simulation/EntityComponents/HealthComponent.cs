using System;
using BFB.Engine.Entity;

namespace BFB.Engine.Simulation.EntityComponents
{
    public class HealthComponent : EntityComponent
    { 
        public HealthComponent() : base(false) { }

        public override void Init(SimulationEntity entity)
        {
            if(entity.Meta == null)
                entity.Meta = new EntityMeta();
            
            //Human defaults
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
            
            Console.WriteLine("YOU'VE DIED!");


            Respawn(simulation,entity);
        }

        private void Respawn(Simulation simulation, SimulationEntity entity)
        {
            //Respawn
            entity.Position.X = 100;
            entity.Position.Y = 100;
            
            if (entity.Meta != null) 
                entity.Meta.Health = 20;
        }

    }
}
