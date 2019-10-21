using BFB.Engine.UI.Components;
using Microsoft.Xna.Framework;

namespace BFB.Engine.UI.Constraints
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
            component.DefaultAttributes.Color = _color;
        }
    }
}