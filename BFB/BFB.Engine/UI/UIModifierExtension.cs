using System;
using BFB.Engine.UI.Components;
using Microsoft.Xna.Framework;

namespace BFB.Engine.UI
{
    public static class UIModifierExtension
    {

        public static IComponent Name(this IComponent component, string name)
        {
            component.Name = name;
            return component;
        }
        
        /**
         * Positioning Modifiers
         */
        
        #region Top

        /**
         * Constrain to top with pixels
         */
        public static IComponent Top(this IComponent component, int pixels)
        {
            //update component y-position
            component.Y = (component.Parent?.Y + pixels) ?? 0;
            return component;
        }
        
        /**
         * Constrain to top as percent
         */
        public static IComponent Top(this IComponent component, float percent)
        {
            //calculate component position as percent
            component.Y = (int) ((component.Parent?.Y + (component.Parent?.Height * percent)) ?? 0);
            return component;
        }
        
        #endregion
        
        #region Left

        /**
         * Constrain to Left with pixels
         */
        public static IComponent Left(this IComponent component, int pixels)
        {
            component.X = component.Parent?.X + pixels ?? 0;
            return component;
        }
        
        /**
         * Constrain to Left as percent
         */
        public static IComponent Left(this IComponent component, float percent)
        {
            //TODO
            return component;
        }
        
        #endregion
        
        #region Right

        /**
         * Constrain to Right with pixels
         */
        public static IComponent Right(this IComponent component, int pixels)
        {
            //TODO
            return component;
        }
        
        /**
         * Constrain to Right as percent
         */
        public static IComponent Right(this IComponent component, float percent)
        {
            //TODO
            return component;
        }
        
        #endregion
        
        #region Bottom

        /**
         * Constrain to bottom with pixels
         */
        public static IComponent Bottom(this IComponent component, int pixels)
        {
            //TODO
            return component;
        }
        
        /**
         * Constrain to Bottom as percent
         */
        public static IComponent Bottom(this IComponent component, float percent)
        {
            //TODO
            return component;
        }
        
        #endregion
        
        #region Width

        /**
         * Constrain Width with pixels
         */
        public static IComponent Width(this IComponent component, int pixels)
        {
            component.Width = pixels;//TODO
            return component;
        }
        
        /**
         * Constrain Width as percent
         */
        public static IComponent Width(this IComponent component, float percent)
        {
            component.Width = (int) (component.Parent?.Width * percent ?? 100);//TODO
            return component;
        }
        
        #endregion
        
        #region Height

        /**
         * Constrain Height with pixels
         */
        public static IComponent Height(this IComponent component, int pixels)
        {
            component.Height = pixels;//TODO
            return component;
        }
        
        /**
         * Constrain Height as percent
         */
        public static IComponent Height(this IComponent component, float percent)
        {
            Console.WriteLine(component.Parent.Name + "name");
            component.Height = (int) ((component.Parent?.Height * percent) ?? 100);//TODO
            return component;
        }
        
        #endregion

        #region AspectRatio

        public static IComponent AspectRatio(this IComponent component, float ratio)
        {
            //TODO
            return null;
        }

        #endregion
        
        /**
         * Visual Modifiers
         */

        #region Color

        public static IComponent Color(this IComponent component, Color color)
        {
            component.Color = color;
             return component;
        }

        #endregion
        
        #region Background    
        
        public static IComponent Background(this IComponent component, Color color)
        {
            component.Background = color;
            return component;
        }
        
        #endregion
        
        #region Font
        
//        public static IComponent Color(this ITextComponent component, Color color)
//        {
//            //TODO
//            return null;
//        }

        #endregion
    }
}