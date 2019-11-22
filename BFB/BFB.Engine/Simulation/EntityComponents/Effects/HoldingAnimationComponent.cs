using BFB.Engine.Entity;
using BFB.Engine.Inventory;

namespace BFB.Engine.Simulation.SimulationComponents
{
    public class HoldingAnimationComponent : EntityComponent
    {
        /// <summary>
        /// Assigns the correct texture key for use by the client to draw the item you are holding
        /// </summary>
        public HoldingAnimationComponent() : base(false) { }

        public override void Update(SimulationEntity entity, Simulation simulation)
        {
            if (entity.Meta != null)
            {
                IItem activeItem = entity.Inventory?.GetActiveSlot();
                entity.Meta.Holding.AtlasKey = activeItem?.Configuration.TextureKey;
                entity.Meta.Holding.ItemType = activeItem?.Configuration.ItemType ?? ItemType.Unknown;
                entity.Meta.Holding.Reach = activeItem?.Configuration.Reach ?? 0;
                entity.Meta.Holding.Progress = activeItem?.TileTarget.ProgressPercent ?? 0f;

                if (entity.Meta.Holding.Progress > 1)
                    entity.Meta.Holding.Progress = 1;
            }
        }
    }
}