using System;
using BFB.Engine.Event;
using BFB.Engine.UI.Constraints;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BFB.Engine.UI.Components
{
    public class UIButtonComponent : UIComponent
    {

        #region Properties
        
        private Action<UIEvent,UIComponentAttributes> _actionClick;
        
        private Action<UIEvent,UIComponentAttributes> _actionHover;
        
        #endregion
        
        #region Constructor
        
        public UIButtonComponent(string text, Action<UIEvent,UIComponentAttributes> actionClick = null, Action<UIEvent,UIComponentAttributes> actionHover = null) : base(nameof(UIButtonComponent))
        {
            
            //Inner text component
            this.Text(text)
                .Color(DefaultAttributes.Color)
                .Background(DefaultAttributes.Background);
            
            Focusable = true;
            
            _actionClick = actionClick;
            _actionHover = actionHover ?? ((e,a) =>
            {
                a.Background = Color.Red;
            });
            
            AddEvent("click", ClickEventHandler);
            AddEvent("hover", HoverEventHandler);
            AddEvent("keypress", KeyPressEventHandler);
            
        }
        
        #endregion
        
        #region keyPressEventHandler

        private void KeyPressEventHandler(UIEvent e)
        {
            if(e.Keyboard.KeyEnum == Keys.Enter)
                ClickEventHandler(e);
        }
        
        #endregion
        
        #region ClickEventHandler
        
        private void ClickEventHandler(UIEvent e)
        {
            UIComponentAttributes attr = new UIComponentAttributes();
            _actionClick?.Invoke(e,attr);
            RenderAttributes = DefaultAttributes.CascadeAttributes(attr);
        }
        
        #endregion
        
        #region HoverEventHandler
        
        private void HoverEventHandler(UIEvent e)
        {
            UIComponentAttributes attr = new UIComponentAttributes();
            _actionHover?.Invoke(e,attr);
            RenderAttributes = DefaultAttributes.CascadeAttributes(attr);
        }
        
        #endregion
    }
}