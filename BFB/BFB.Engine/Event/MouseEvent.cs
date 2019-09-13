//Monogame
using Microsoft.Xna.Framework.Input;

namespace BFB.Engine.Event
{
    public partial class Event
    {
        public MouseEvent Mouse { get; set; }
    }

    public class MouseEvent
    {
        public int X { get; set; }

        public int Y { get; set; }

        public ButtonState LeftButton { get; set; }

        public ButtonState RightButton { get; set; }

        public ButtonState MiddleButton { get; set; }
    }
}
