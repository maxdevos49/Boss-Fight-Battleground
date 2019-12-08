using System;
using BFB.Engine.Event;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BFB.Engine.UI.Components
{
    public class UIButtonComponent : UIComponent
    {

        #region Properties
        
        private readonly Action<UIEvent,UIAttributes> _actionClick;
        
        private readonly Action<UIEvent,UIAttributes> _actionHover;
        
        #endregion
        
        #region Constructor
        
        public UIButtonComponent(string text, Action<UIEvent,UIAttributes> actionClick = null, Action<UIEvent,UIAttributes> actionHover = null) : base(nameof(UIButtonComponent))
        {
            
            //Inner text component
            this.Text(text)
                .Color(Color.Black);
            
            this.Background(new Color(169,170,168))
                .Border(3, new Color(211,212,210));
            
            Focusable = true;
            
            _actionClick = actionClick;
            _actionHover = actionHover ?? ((e,a) =>
            {
                a.Background = new Color(125,125,125);
                a.Color = Color.White;
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
            UIAttributes attr = new UIAttributes();
            _actionClick?.Invoke(e,attr);
            RenderAttributes = DefaultAttributes.CascadeAttributes(attr);
        }
        
        #endregion
        
        #region HoverEventHandler
        
        private void HoverEventHandler(UIEvent e)
        {
            UIAttributes attr = new UIAttributes();
            _actionHover?.Invoke(e,attr);
            RenderAttributes = DefaultAttributes.CascadeAttributes(attr);
        }
        
        #endregion
    }
}