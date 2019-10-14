using BFB.Engine.UI.Modifiers;
using Microsoft.Xna.Framework;

namespace BFB.Engine.UI.Components
{
    public class UIZstackComponent : UIComponent
    {
        public UIZstackComponent() : base(nameof(UIZstackComponent))
        {
            StackDirection = StackDirection.Horizontal;
            AddConstraint(new UIHeightConstraint(1f));
            AddConstraint(new UIWidthConstraint(1f));
            AddConstraint(new UICenterConstraint());
        }
    }
}