//C#
using System.Collections.Generic;
using JetBrains.Annotations;

//Monogame
using Microsoft.Xna.Framework.Input;

namespace BFB.Engine.Event
{

    public partial class Event
    {
        public KeyboardEvent Keyboard { get; set; }
    }

    public class KeyboardEvent
    {
        [UsedImplicitly]
        public string Key { get; set; }

        public Keys KeyEnum { get; set; }

        public KeyboardState KeyboardState { get; set; }
    }
}
