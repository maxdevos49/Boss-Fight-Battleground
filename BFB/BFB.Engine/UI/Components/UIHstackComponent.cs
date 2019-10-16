using BFB.Engine.UI.Modifiers;
using Microsoft.Xna.Framework;

namespace BFB.Engine.UI.Components
{
    public class UIHstackComponent : UIComponent
    {
        public UIHstackComponent() : base(nameof(UIHstackComponent))
        {
            StackDirection = StackDirection.Horizontal;
        }
    }
}