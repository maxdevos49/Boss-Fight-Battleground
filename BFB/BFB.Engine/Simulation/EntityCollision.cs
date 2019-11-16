using BFB.Engine.Entity;

namespace BFB.Engine.Simulation
{
    public class EntityCollision
    {
        public string Filter { get; set; }
        
        public SimulationEntity PrimaryEntity { get; set; }
        
        public SimulationEntity SecondaryEntity { get; set; }
    }
}