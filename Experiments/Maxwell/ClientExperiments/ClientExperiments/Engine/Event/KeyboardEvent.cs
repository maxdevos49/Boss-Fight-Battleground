//C#
using System.Collections.Generic;

//Monogame
using Microsoft.Xna.Framework.Input;

namespace ClientExperiments.Engine.Event
{

    public partial class Event
    {
        public KeyboardEvent Keyboard { get; set; }
    }

    public class KeyboardEvent
    {
        public string Key { get; set; }

        public Keys KeyEnum { get; set; }

        public KeyboardState KeyboardState { get; set; }
    }
}
