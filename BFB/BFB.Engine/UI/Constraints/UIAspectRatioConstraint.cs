using System;
using BFB.Engine.UI.Components;

namespace BFB.Engine.UI.Modifiers
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
            if ((component.Width / _ratio) > component.Parent.Height)
            {
                component.Width = (int)(component.Height *_ratio);
            }
            else
            {
                component.Height = (int)(component.Width /_ratio);
            }

        }
    }
}