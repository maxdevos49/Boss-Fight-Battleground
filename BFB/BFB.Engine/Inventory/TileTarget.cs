namespace BFB.Engine.Inventory
{
    public class TileTarget
    {
        /// <summary>
        /// The amount of straight ticks that a tile has been focused
        /// </summary>
        public int Progress { get; set; }
        
        /// <summary>
        /// Total Ticks needed to break the current block
        /// </summary>
        public int ProgressTotal { get; set; }

        /// <summary>
        /// The percentage of the current block break
        /// </summary>
        public float ProgressPercent => (float)Progress / ProgressTotal;
        
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