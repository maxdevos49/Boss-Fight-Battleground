using System.Drawing;
using BFB.Engine.Math;
using BFB.Engine.TileMap;

namespace BFB.Engine.Simulation
{
    /// <summary>
    /// Represents information about a TileCollision
    /// </summary>
    public class TileCollision
    {
        /// <summary>
        /// The tile being collided with
        /// </summary>
        public WorldTile Tile { get; set; }
        
        /// <summary>
        /// The position of the tile being collided with
        /// </summary>
        public BfbVector TilePosition { get; set; }
        
        /// <summary>
        /// The side of the block that is being collided with
        /// </summary>
        public CollisionSide Side { get; set; }

        /// <summary>
        /// The upper vertical bound block position
        /// </summary>
        public int TopBlockY { get; set; }
        
        /// <summary>
        /// The lower vertical bound block position
        /// </summary>
        public int BottomBlockY { get; set; }
        
        /// <summary>
        /// The left horizontal bound block position
        /// </summary>
        public int LeftBlockX { get; set; }
        
        /// <summary>
        /// The right bound block position
        /// </summary>
        public int RightBlockX { get; set; }
        
        /// <summary>
        /// Gets the rectangle needed for detecting a tile collison
        /// </summary>
        /// <param name="tileSize">The tile size</param>
        /// <returns>A rectangle representing the tile to be checked</returns>
        public Rectangle TileRectangle(int tileSize) {
            return new Rectangle
            {
                X = (int) TilePosition.X * tileSize,
                Y = (int) TilePosition.Y * tileSize,
                Width = tileSize, 
                Height = tileSize
            };
        }
    }
}