using BFB.Engine.UI.Components;

namespace BFB.Engine.UI.Modifiers
{
    public class UIWidthConstraint : UIConstraint
    {
        private readonly int? _pixels;
        private readonly float? _percent;
        
        public UIWidthConstraint(int pixels) : base(nameof(UIWidthConstraint))
        {
            _pixels = pixels;
            _percent = null;
        }
        
        public UIWidthConstraint(float percent) : base(nameof(UIWidthConstraint))
        {
            _pixels = null;
            _percent = percent;
        }

        public override void Apply(UIComponent component)
        {
            if (_pixels == null)
            {
                //Percent
                component.Width = (int)((_percent ?? 0) * component.Parent.Width);
            }
            else
            {
                //Pixel
                component.Width = _pixels ?? 0;
            }
        }
    }
}