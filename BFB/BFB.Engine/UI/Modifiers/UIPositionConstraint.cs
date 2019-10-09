using BFB.Engine.UI.Components;

namespace BFB.Engine.UI.Modifiers
{
    public class UIPositionConstraint : UIConstraint
    {
        private readonly int? _top;
        private readonly int? _right;
        private readonly int? _bottom;
        private readonly int? _left;
        
        public UIPositionConstraint(int? top = null, int? right = null, int? bottom = null, int? left = null) : base(nameof(UIPositionConstraint))
        {
            _top = top;
            _right = right;
            _bottom = bottom;
            _left = left;
        }

        public override void Apply(UIComponent component)
        {
            if (_top != null)
            {
                component.Y = component.Parent.Y + (int) _top;
                component.Height -= (int) _top;
            }

            if (_right != null)
            {
                component.Width -= (int) _right;
            }

            if (_bottom != null)
            {
                component.Height -= (int) _bottom;
            }

            if (_left != null)
            {
                component.X = component.Parent.X + (int) _left;
                component.Width -= (int) _left;
            }
                
        }
    }
}