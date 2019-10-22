using Microsoft.Xna.Framework.Input;

namespace BFB.Engine.Input
{
    public class MouseStatus
    {
        public int X { get; set; }

        public int Y { get; set; }

        public ButtonState LeftButton { get; set; }

        public ButtonState RightButton { get; set; }

        public ButtonState MiddleButton { get; set; }
        
        public MouseState MouseState { get; set; }
    }

}