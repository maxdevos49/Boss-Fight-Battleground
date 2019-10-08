using BFB.Engine.UI.Modifiers;

namespace BFB.Engine.UI.Components
{
    public class UIBlockComponent : UIComponent
    {
        public UIBlockComponent() : base(nameof(UIBlockComponent))
        {
            StackDirection = StackDirection.None;
            AddConstraint(new UIHeightConstraint(1f));
            AddConstraint(new UIWidthConstraint(1f));
            AddConstraint(new UICenterConstraint());
        }
    }
}