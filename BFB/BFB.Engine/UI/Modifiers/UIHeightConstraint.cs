using BFB.Engine.UI.Components;

namespace BFB.Engine.UI.Modifiers
{
    public class UIHeightConstraint : UIConstraint
    {
        private readonly int? _pixels;
        private readonly float? _percent;
        
        public UIHeightConstraint(int pixels) : base(nameof(UIHeightConstraint))
        {
            _pixels = pixels;
            _percent = null;
        }
        
        public UIHeightConstraint(float percent) : base(nameof(UIHeightConstraint))
        {
            _pixels = null;
            _percent = percent;
        }

        public override void Apply(UIComponent component)
        {
            if (_pixels == null)
            {
                //Percent
                component.Height = (int)((_percent ?? 0) * component.Parent.Height);
            }
            else
            {
                //Pixel
                component.Height = _pixels ?? 0;
            }
        }
    }
}
