using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace BFB.Engine.UI
{
    public static class UIComponentExtension
    {
        #region Vstack
        
        [UsedImplicitly]
        public static void Vstack<TModel>(this UIBuilder<TModel> uiBuilder)
        {
            //TODO
        }
        
        #endregion;

        #region Hstack
        
        [UsedImplicitly]
        public static void Hstack<TModel>(this UIBuilder<TModel> uiBuilder)
        {
            //TODO
        }
        
        #endregion

        #region List
        
        [UsedImplicitly]
        public static void List<TModel,TProperty>(this UIBuilder<TModel> uiBuilder, Expression<Func<TModel,TProperty>> property)
         {
             //TODO
         }
        
        #endregion

        #region Text
        
        [UsedImplicitly]
        public static void Text<TModel,TProperty>(this UIBuilder<TModel> uiBuilder, Func<TModel,TProperty> property)
        {
            //TODO
        }
        
        #endregion

        #region Spacer
        
        [UsedImplicitly]
        public static void Spacer<TModel>(this UIBuilder<TModel> uiBuilder)
        {
            //TODO
        }
        
        #endregion
    }

}