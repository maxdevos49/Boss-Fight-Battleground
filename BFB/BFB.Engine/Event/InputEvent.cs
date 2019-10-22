using BFB.Engine.Input;

namespace BFB.Engine.Event
{
    public class InputEvent : Event
    {
        public MouseStatus Mouse { get; set; }

        public KeyboardStatus Keyboard { get; set; }
    }
  
}