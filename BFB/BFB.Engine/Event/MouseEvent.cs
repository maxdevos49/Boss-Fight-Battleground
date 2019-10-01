using JetBrains.Annotations;
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

        [UsedImplicitly]
        public ButtonState LeftButton { get; set; }

        [UsedImplicitly]
        public ButtonState RightButton { get; set; }

        [UsedImplicitly]
        public ButtonState MiddleButton { get; set; }
    }
}
