using Microsoft.Xna.Framework.Input;

namespace BFB.Engine.Input
{
    /// <summary>
    /// The potential status of a keyboard key, pressed, unpressed, held down, etc.
    /// </summary>
    public class KeyboardStatus
    {
        public Keys KeyEnum { get; set; }

        public KeyboardState KeyboardState { get; set; }
    }
}