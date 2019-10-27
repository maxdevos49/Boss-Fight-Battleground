using Microsoft.Xna.Framework;

namespace BFB.Engine.UI.Components
{
    public class UIRootComponent : UIComponent
    {
        public UIRootComponent(Rectangle bounds) : base(nameof(UIRootComponent))
        {
            DefaultAttributes.X = bounds.X;
            DefaultAttributes.Y = bounds.Y;
            DefaultAttributes.Width = bounds.Width;
            DefaultAttributes.Height = bounds.Height;
        }
    }
}