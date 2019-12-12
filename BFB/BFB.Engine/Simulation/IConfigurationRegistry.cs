using BFB.Engine.Inventory;
using BFB.Engine.Inventory.Configuration;
using BFB.Engine.Simulation.EntityComponents;
using BFB.Engine.Simulation.ItemComponents;
using BFB.Engine.TileMap;

namespace BFB.Engine.Simulation
{
    public interface IConfigurationRegistry
    {

        /// <summary>
        /// Gets a item configuration object
        /// </summary>
        /// <param name="itemKey">The item Key</param>
        /// <returns>The item configuration object</returns>
        ItemConfiguration GetItemConfiguration(string itemKey);

        /// <summary>
        /// Gets a block configuration object
        /// </summary>
        /// <param name="blockKey">The block Key</param>
        /// <returns></returns>
        BlockConfiguration GetBlockConfiguration(WorldTile blockKey);
        
        /// <summary>
        /// Gets a wall configuration object
        /// </summary>
        /// <param name="wallKey">The wall Key</param>
        /// <returns></returns>
        WallConfiguration GetWallConfiguration(WorldTile wallKey);

        /// <summary>
        /// Gets a Item Component for Use
        /// </summary>
        /// <param name="componentKey"></param>
        /// <returns></returns>
        IItemComponent GetItemComponent(string componentKey);

    }
}