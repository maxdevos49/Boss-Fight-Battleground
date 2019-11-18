using System.Collections.Generic;

namespace BFB.Engine.Inventory
{
    public class WallConfiguration
    {
        /// <summary>
        /// The Texture to use for rendering
        /// </summary>
        public string TextureKey { get; set; }
        
        /// <summary>
        /// The key to reference the walls item
        /// </summary>
        public string ItemKey { get; set; }
        
        /// <summary>
        /// The speed in which the block will break in ticks 
        /// </summary>
        public int BreakSpeed { get; set; }
        
        /// <summary>
        /// The components to use with the wall
        /// </summary>
        public List<string> Components { get; set; }
    }
}