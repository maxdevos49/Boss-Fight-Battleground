using BFB.Engine.UI.Modifiers;
using Microsoft.Xna.Framework;

namespace BFB.Engine.UI.Components
{
    public class VstackComponent : UIComponent
    {
        public VstackComponent() : base(nameof(VstackComponent))
        {
            //Default constraints
            AddConstraint(new UIBackgroundConstraint(Color.Transparent));
            AddConstraint(new UIWidthConstraint(1.0f));
            AddConstraint(new UIHeightConstraint(1.0f));
        }
    }
}