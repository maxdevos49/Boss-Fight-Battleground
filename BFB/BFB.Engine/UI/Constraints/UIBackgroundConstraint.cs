using BFB.Engine.UI.Components;
using Microsoft.Xna.Framework;

namespace BFB.Engine.UI.Constraints
{
    public class UIBackgroundConstraint :UIConstraint
    {
        private readonly Color _background;

        public UIBackgroundConstraint(Color background) : base(nameof(UIBackgroundConstraint))
        {
            _background = background;
        }

        public override void Apply(UIComponent component)
        {
            component.DefaultAttributes.Background = _background;
        }
    }
}