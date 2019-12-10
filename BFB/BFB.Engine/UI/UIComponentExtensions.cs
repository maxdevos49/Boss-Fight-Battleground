using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BFB.Engine.Event;
using BFB.Engine.Inventory;
using BFB.Engine.UI.Components;

namespace BFB.Engine.UI
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
   
        #region Textbox

        public static UIComponent TextBoxFor<TModel>(
            this UIComponent component,
            TModel model,
            Expression<Func<TModel, string>> selector,
            Action<UIEvent,UIAttributes> clickAction = null,
            Action<UIEvent,UIAttributes> keyPressAction = null,
            Action<UIEvent,UIAttributes> hoverAction = null
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
        
        #region List

        public static UIComponent ListFor<TModel, TItem>(
            this UIComponent component,
            TModel model,
            Expression<Func<TModel, List<TItem>>> listSelector,
            Action<UIComponent, TItem> itemTemplate,
            StackDirection stackDirection = StackDirection.Vertical
        )
        {
            return AddNode(component,
                new UIListComponent<TModel, TItem>(
                    model,
                    listSelector,
                    itemTemplate, stackDirection),
                null);
        }
        
        #endregion
        
        #region ScrollableContainer

        public static UIComponent ScrollableContainer(this UIComponent parentNode, Action<UIComponent> handler)
        {
            UIScrollableContainer child = new UIScrollableContainer();
            parentNode.AddChild(child);
            handler?.Invoke(child);
            UIComponent childInner = child.Children.FirstOrDefault();
            
            if(childInner != null)
                child.AddStack(childInner);
            
            return child;
        }
        
        #endregion
        
        #region Button

        public static UIComponent Button(this UIComponent component, string text, Action<UIEvent,UIAttributes> clickAction = null, Action<UIEvent,UIAttributes> hoverAction = null)
        {
            return AddNode(component, new UIButtonComponent(text, clickAction, hoverAction), null);
        }
       
        
        #endregion
        
        #region InventorySlot

        public static UIComponent InventorySlot(this UIComponent component, ClientInventory inventory, byte slotId, Action<UIEvent, byte> clickAction = null,Action<UIEvent,byte> enterAction = null, bool hotBarMode = false)
        {
            return AddNode(component, new UIInventorySlot(inventory, slotId, clickAction, enterAction, hotBarMode), null);
        }
        
        #endregion
        
        #region ChatUI
        
        public static UIComponent ChatUI(this UIComponent component)
        {
            return AddNode(component, new UIChatComponent(), null);
        }
        
        #endregion
        
        #region HudMeter

        public static UIComponent HudMeter<TModel>(this UIComponent component, TModel model, Expression<Func<TModel,ushort?>> valueSelector, bool percentMode = false, bool mode = false)
        {
            return AddNode(component, new UIHudMeterComponent<TModel>(model, valueSelector, percentMode,mode), null);
        }
        
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