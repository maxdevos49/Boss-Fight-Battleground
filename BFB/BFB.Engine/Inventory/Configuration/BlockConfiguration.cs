using System.Collections.Generic;

namespace BFB.Engine.Inventory.Configuration
{
    /// <summary>
    /// A class to represent a blocks configuration and should be treated as readonly
    /// </summary>
    public class BlockConfiguration
    {
        /// <summary>
        /// The Texture to use for rendering
        /// </summary>
        public string TextureKey { get; set; }
        
        /// <summary>
        /// The key to the item of this block
        /// </summary>
        public string ItemKey { get; set; }
        
        /// <summary>
        /// Used to indicate what kind of collisions should be used for that block
        /// </summary>
        public int CollisionId { get; set; }
        
        /// <summary>
        /// The amount of game ticks required to break a block without a extra tool
        /// </summary>
        public int BreakSpeed { get; set; }
        
        /// <summary>
        /// A collection of block components to apply to the block
        /// </summary>
        public List<string> Components { get; set; }

    }
}