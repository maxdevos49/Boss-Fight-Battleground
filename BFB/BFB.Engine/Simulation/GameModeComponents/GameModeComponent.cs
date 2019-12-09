using System;
using System.Collections.Generic;
using System.Text;
using BFB.Engine.Entity;

namespace BFB.Engine.Simulation.GameModeComponents
{
    public class GameModeComponent
    {
        public GameModeComponent() { }

        public virtual void Init(Simulation simulation) { }

        public virtual void Update(Simulation simulation) { }

        public virtual void OnEntityRemove(Simulation simulation, SimulationEntity entity, EntityRemovalReason? reason) { }
    }
}
