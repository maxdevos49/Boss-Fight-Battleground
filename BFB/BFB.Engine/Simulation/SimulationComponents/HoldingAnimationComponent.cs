using BFB.Engine.Entity;

namespace BFB.Engine.Simulation.SimulationComponents
{
    public class HoldingAnimationComponent : SimulationComponent
    {
        /// <summary>
        /// Assigns the correct texture key for use by the client to draw the item you are holding
        /// </summary>
        public HoldingAnimationComponent() : base(false) { }

        public override void Update(SimulationEntity entity, Simulation simulation)
        {
            entity.HoldingTexture = entity.Inventory?.GetActiveSlot()?.Configuration?.TextureKey;
        }
    }
}