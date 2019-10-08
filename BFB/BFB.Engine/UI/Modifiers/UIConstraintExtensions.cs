using BFB.Engine.UI.Components;
using Microsoft.Xna.Framework;

namespace BFB.Engine.UI.Modifiers
{
    public static class UIConstraintExtensions
    {
        
        #region Width

        public static UIComponent Width(this UIComponent component, int pixels)
        {
            return component.AddConstraint(new UIWidthConstraint(pixels));
        }
        
        public static UIComponent Width(this UIComponent component, float percent)
        {
            return component.AddConstraint(new UIWidthConstraint(percent));
        }
        
        #endregion
        
        #region Height

        public static UIComponent Height(this UIComponent component, int pixels)
        {
            return component.AddConstraint(new UIHeightConstraint(pixels));
        }
        
        public static UIComponent Height(this UIComponent component, float percent)
        {
            return component.AddConstraint(new UIHeightConstraint(percent));
        }
        
        #endregion
        
        #region Background

        public static UIComponent Background(this UIComponent component, Color color)
        {
            return component.AddConstraint(new UIBackgroundConstraint(color));
        }

        #endregion
        
        #region Center

        public static UIComponent Center(this UIComponent component)
        {
            return component.AddConstraint(new UICenterConstraint());
        }

        #endregion
        
        #region Grow

        public static UIComponent Grow(this UIComponent component, int proportion)
        {
            component.Grow = proportion;
            return component;
        }
        
        #endregion
        
        #region FontSize

        public static UIComponent FontSize(this UIComponent component, float fontScale)
        {
            return component.AddConstraint(new UIFontSizeConstraint(fontScale));
        }   
        
        #endregion
        
        #region Color

        public static UIComponent Color(this UIComponent component, Color color)
        {
            return component.AddConstraint(new UIColorConstraint(color));
        }

        #endregion
        
    }
}