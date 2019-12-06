using BFB.Engine.UI.Components;

namespace BFB.Engine.UI.Constraints
{
    public class UIAspectRatioConstraint : UIConstraint
    {
        private readonly float _ratio;
        public UIAspectRatioConstraint(float ratio) : base(nameof(UIAspectRatioConstraint))
        {
            _ratio = ratio;
        }

        public override void Apply(UIComponent component)
        {
            if (component.Parent == null)
                return;
            
            if (component.DefaultAttributes.Width / _ratio > component.Parent.DefaultAttributes.Height)
                component.DefaultAttributes.Width = (int)(component.DefaultAttributes.Height  * _ratio);
            else
                component.DefaultAttributes.Height = (int)(component.DefaultAttributes.Width /_ratio);
        }
    }
}