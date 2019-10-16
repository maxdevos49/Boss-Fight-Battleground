using BFB.Engine.UI.Components;

namespace BFB.Engine.UI.Modifiers
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
            component.FontSize = _fontScale;
        }
    }
}