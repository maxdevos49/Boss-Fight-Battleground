using System;
using System.Collections;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace BFB.Engine.UI.Components
{
    public static class UIComponentExtension
    {
        #region View

        public static IComponent View<TModel>(this UIContext<TModel> uiContext, Action<IComponent> handler)
        {
            handler(uiContext.GetRoot());
            return uiContext.GetRoot();
        }
        
        #endregion
        
        #region Vstack
        
        [UsedImplicitly]
        public static IComponent Vstack(this IComponent component, Action<IComponent> handler)
        {
            Component c = new Component();//TODO
            component.AddChild(c);
            handler(c);
            return c;
        }
        
        #endregion;

        #region Hstack
        
        [UsedImplicitly]
        public static IComponent Hstack(this IComponent component, Action<IComponent> handler)
        {
            //TODO
            return null;
        }
        
        #endregion

        #region List
        
        [UsedImplicitly]
        public static IComponent List<TModel,TProperty>(this IComponent component, Expression<Func<TModel,TProperty>> property, Action<IComponent> handler) where TProperty: IEnumerable
         {
             //TODO
             return null;
         }
        
        #endregion

        #region Text
        
        [UsedImplicitly]
        public static IComponent Text<TModel,TProperty>(this IComponent component, Func<UIContext<TModel>,TModel,TProperty> property)
        {
            //TODO
            return null;
        }
        
        #endregion

        #region Spacer
        
        [UsedImplicitly]
        public static IComponent Spacer(this IComponent component)
        {
            //TODO
            return null;
        }
        
        #endregion
    }

}