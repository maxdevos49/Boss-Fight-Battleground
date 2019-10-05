using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace BFB.Engine.UI.Components
{
    public struct Container : IContainer
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Color Background { get; set; }
        public Color Color { get; set; }
        public Stack<IComponent> Contents { get; set; }
    }
}