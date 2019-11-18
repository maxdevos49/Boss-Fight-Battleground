using System.Collections.Generic;
using BFB.Engine.Simulation.InputComponents;
using BFB.Engine.Simulation.PhysicsComponents;
using BFB.Engine.Simulation.SimulationComponents;

namespace BFB.Engine.Entity
{
    public class ComponentOptions
    {
        public IInputComponent Input { get; set; }
        
        public IPhysicsComponent Physics { get; set; }
        
        public List<ISimulationComponent> GameComponents { get; set; }
    }

}