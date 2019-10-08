using BFB.Engine.UI.Modifiers;
using Microsoft.Xna.Framework;

namespace BFB.Engine.UI.Components
{
    public class UIHstackComponent : UIComponent
    {
        public UIHstackComponent() : base(nameof(UIHstackComponent))
        {
            StackDirection = StackDirection.Horizontal;
            AddConstraint(new UIHeightConstraint(1f));
            AddConstraint(new UIWidthConstraint(1f));
            AddConstraint(new UICenterConstraint());
        }
    }
}