using BFB.Engine.Simulation.InputComponents;
using BFB.Engine.Simulation.PhysicsComponents;

namespace BFB.Engine.Entity
{
    public class ComponentOptions
    {
        public IInputComponent Input { get; set; }
        public IPhysicsComponent Physics { get; set; }
        public IPhysicsComponent Combat { get; set; }
    }

}