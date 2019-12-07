using System;
using System.Collections.Generic;
using System.Text;
using BFB.Engine.Entity;

namespace BFB.Engine.Simulation.GameModeComponents
{
    public class ManaRegenComponent : GameComponent
    {
        private int timeToRegen;
        public ManaRegenComponent() : base()
        {
            timeToRegen = 40;
        }

        public override void Update(Simulation simulation)
        {
            timeToRegen -= 1;
            if (timeToRegen <= 0)
            {
                foreach (SimulationEntity entity in simulation.GetPlayerEntities())
                {
                    if (entity.EntityConfiguration.EntityKey != "Human") return;

                    entity.Meta.Mana += 5;
                }

                timeToRegen = 40;
            }
        }
    }
}
