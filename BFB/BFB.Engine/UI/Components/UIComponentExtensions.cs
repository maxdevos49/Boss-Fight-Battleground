using System;
using System.Linq.Expressions;
using System.Net.NetworkInformation;
using BFB.Engine.Event;

namespace BFB.Engine.UI.Components
{
    public static class UIComponentExtensions
    {
        #region Vstack
        
        public static UIComponent Vstack(this UIComponent parentNode, Action<UIComponent> handler)
        {
            return AddNode(parentNode, new UIVstackComponent(), handler);
        }
        
        #endregion
        
        #region Hstack
                          
        public static UIComponent Hstack(this UIComponent parentNode, Action<UIComponent> handler)
        {
            return AddNode(parentNode, new UIHstackComponent(), handler);
        }
      
      #endregion
      
        #region Zstack

        public static UIComponent Zstack(this UIComponent parentNode, Action<UIComponent> handler)
        {
            return AddNode(parentNode, new UIZstackComponent(), handler);
        }

        #endregion

        #region Spacer

        public static UIComponent Spacer(this UIComponent component, int? grow = null)
        {
            return AddNode(component, new UISpacerComponent(grow), null);
        }
        
        #endregion
        
        #region Text

        public static UIComponent TextFor<TModel>(this UIComponent component, TModel model, Func<TModel, string> stringSelector)
        {
            return AddNode(component, new UITextComponent<TModel>(model, stringSelector), null);
        }
        
        public static UIComponent Text(this UIComponent component, string text)
        {
            return AddNode(component, new UITextComponent<string>(text), null);
        }
        
        #endregion
        
        #region Slider (TODO)
        
        #endregion
        
        #region Textbox (TODO)

        public static UIComponent TextBoxFor<TModel>(
            this UIComponent component,
            TModel model,
            Expression<Func<TModel, string>> selector,
            Action<UIEvent,UIComponentAttributes> clickAction = null,
            Action<UIEvent,UIComponentAttributes> keyPressAction = null,
            Action<UIEvent,UIComponentAttributes> hoverAction = null
            )
        {
            return AddNode(
                component,
                new UITextBoxComponent<TModel>(
                    model, 
                    selector,
                    clickAction,
                    keyPressAction,
                    hoverAction
                    ),
                null);
        }
        
        #endregion
        
        #region Button

        public static UIComponent Button(this UIComponent component, string text, Action<UIEvent,UIComponentAttributes> clickAction = null, Action<UIEvent,UIComponentAttributes> hoverAction = null)
        {
            return AddNode(component, new UIButtonComponent(text, clickAction, hoverAction), null);
        }
       
        
        #endregion
        
        #region Toggle (TODO)
        
        #endregion
        
        #region AddNode (Private)
        
        private static UIComponent AddNode(UIComponent parent, UIComponent newChild, Action<UIComponent> handler)
        {
            
            parent.AddChild(newChild);
            handler?.Invoke(newChild);
            return newChild;
        }
        
        #endregion
    }
}