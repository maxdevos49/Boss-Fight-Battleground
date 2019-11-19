namespace BFB.Engine.Inventory
{
    public class TileTarget
    {
        /// <summary>
        /// The amount of straight ticks that a tile has been focused
        /// </summary>
        public int Progress { get; set; }
        
        /// <summary>
        /// The tile X position
        /// </summary>
        public int X { get; set; }
        
        /// <summary>
        /// The tile Y position
        /// </summary>
        public int Y { get; set; }
    }
}