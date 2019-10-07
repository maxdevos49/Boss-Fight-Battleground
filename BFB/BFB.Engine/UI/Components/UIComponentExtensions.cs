using System;

namespace BFB.Engine.UI.Components
{
    public static class UIComponentExtensions
    {
        #region Vstack
        
        public static UIComponent Vstack(this UIComponent parentNode, Action<UIComponent> handler)
        {
            return AddNode(parentNode, new VstackComponent(), handler);
        }
        
        #endregion

        #region AddNode
        
        private static UIComponent AddNode(UIComponent parent, UIComponent newChild, Action<UIComponent> handler)
        {
            
            parent.AddChild(newChild);
            handler(newChild);
            return newChild;
        }
        
        #endregion
    }
}