using BFB.Engine.Entity;

namespace BFB.Engine.Simulation.EntityComponents
{
    public class ManaRegen : EntityComponent
    {
        private const int RegenTickCooldown = 2;
        
        public ManaRegen() : base(false) { }

        public override void Update(SimulationEntity entity, Simulation simulation)
        {
            if (entity.Meta == null)
                return;

            if (entity.TicksSinceCreation % RegenTickCooldown == 0) 
                return;
            
            entity.Meta.Mana += 5;

            if (entity.Meta.MaxMana < entity.Meta.Mana)
                entity.Meta.Mana = entity.Meta.MaxMana;
        }
    }
}