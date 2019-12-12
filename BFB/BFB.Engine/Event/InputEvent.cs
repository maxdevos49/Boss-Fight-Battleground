using BFB.Engine.Input;

namespace BFB.Engine.Event
{
    /// <summary>
    /// Event created by input from a user
    /// </summary>
    public class InputEvent : Event
    {
        /// <summary>
        /// An object containing the current status of the mouse
        /// </summary>
        public MouseStatus Mouse { get; set; }

        /// <summary>
        /// An object containing the current status of the keyboard
        /// </summary>
        public KeyboardStatus Keyboard { get; set; }
    }
  
}