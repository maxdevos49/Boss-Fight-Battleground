using Microsoft.Xna.Framework;

namespace BFB.Engine.UI.Components
{
    public class UIZstackComponent : UIComponent
    {
        public UIZstackComponent() : base(nameof(UIZstackComponent))
        {
            DefaultAttributes.StackDirection = StackDirection.None;
        }
    }
}