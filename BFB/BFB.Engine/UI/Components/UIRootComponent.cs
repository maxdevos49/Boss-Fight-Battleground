using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BFB.Engine.UI.Components
{
    public class UIRootComponent : UIComponent
    {
        public UIRootComponent(Rectangle bounds) : base(nameof(UIRootComponent))
        {
            X = bounds.X;
            Y = bounds.Y;
            Width = bounds.Width;
            Height = bounds.Height;
            Background = Color.Transparent;
        }
    }
}