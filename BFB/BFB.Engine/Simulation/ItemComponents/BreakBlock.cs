using BFB.Engine.Entity;

namespace BFB.Engine.Simulation.ItemComponents
{
    public class BreakBlock : IItemComponent
    {

        private int _breakSpeed;
        
        public BreakBlock(int breakSpeed = 1)
        {
            _breakSpeed = breakSpeed;
        }
        
        public void Use(Simulation simulation, SimulationEntity entity)
        {
            
        }
    }
}