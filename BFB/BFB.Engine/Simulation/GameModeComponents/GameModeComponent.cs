using System;
using System.Collections.Generic;
using System.Text;
using BFB.Engine.Entity;

namespace BFB.Engine.Simulation.GameModeComponents
{
    public abstract class GameModeComponent
    {
        
        protected GameModeComponent() { }

        public virtual void Init(Simulation simulation) { }
        public virtual void Update(Simulation simulation) { }
        public virtual void OnEntityAdd(Simulation simulation, SimulationEntity entity) { }
        public virtual void OnEntityRemove(Simulation simulation, SimulationEntity entity, EntityRemovalReason? reason) { }
        public virtual void OnSimulationStart(Simulation simulation) { }
        public virtual void OnSimulationStop(Simulation simulation) { }
        public virtual void Reset(Simulation simulation) {}
        public virtual void OnGameMessage(string message){}
    }
}
