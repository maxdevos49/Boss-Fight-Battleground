using BFB.Engine.UI.Components;
using Microsoft.Xna.Framework;

namespace BFB.Engine.UI.Modifiers
{
    public class UIColorConstraint : UIConstraint
    {
        private readonly Color _color;

        public UIColorConstraint(Color color) : base(nameof(UIColorConstraint))
        {
            _color = color;
        }

        public override void Apply(UIComponent component)
        {
            component.Color = _color;
        }
    }
}