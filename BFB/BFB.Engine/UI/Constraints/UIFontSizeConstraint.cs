using BFB.Engine.UI.Components;

namespace BFB.Engine.UI.Constraints
{
    public class UIFontSizeConstraint : UIConstraint
    {

        private readonly float _fontScale;
        
        public UIFontSizeConstraint(float fontScale) : base(nameof(UIFontSizeConstraint))
        {
            _fontScale = fontScale;
        }

        public override void Apply(UIComponent component)
        {
            component.DefaultAttributes.FontSize = _fontScale;
        }
    }
}