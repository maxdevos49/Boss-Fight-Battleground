using Microsoft.Xna.Framework.Input;

namespace BFB.Engine.Input
{
    /// <summary>
    /// The state at which a mouse can be. Holds the X and Y coordinate, and the the states of each click.
    /// </summary>
    public class MouseStatus
    {
        public int X { get; set; }

        public int Y { get; set; }

        public ButtonState LeftButton { get; set; }

        public ButtonState RightButton { get; set; }

        public ButtonState MiddleButton { get; set; }
        
        public int Scroll { get; set; }
        
        public int ScrollAmount { get; set; }
        
        public MouseState MouseState { get; set; }
        
    }

}