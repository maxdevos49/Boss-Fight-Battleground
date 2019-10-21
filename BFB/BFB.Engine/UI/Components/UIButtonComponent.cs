using System;
using BFB.Engine.Event;
using BFB.Engine.UI.Constraints;
using Microsoft.Xna.Framework;

namespace BFB.Engine.UI.Components
{
    public class UIButtonComponent : UIComponent
    {

        private Action<UIEvent,UIComponentAttributes> _actionClick;
        private Action<UIEvent,UIComponentAttributes> _actionHover;
        
        public UIButtonComponent(string text, Action<UIEvent,UIComponentAttributes> actionClick = null, Action<UIEvent,UIComponentAttributes> actionHover = null) : base(nameof(UIButtonComponent))
        {
            
            //Inner text component
            this.Text(text)
                .Color(DefaultAttributes.Color)
                .Background(DefaultAttributes.Background);
            
            //Buttons can be tabbed using tab index
            Focusable = true;

            //Click event listener
            AddEvent("click", (e) =>
            {
                //Used for applying event based attributes
                UIComponentAttributes attr = new UIComponentAttributes();
                
                //UI defined handler
                _actionClick?.Invoke(e,attr);

                //Apply any attributes
                RenderAttributes = DefaultAttributes.CascadeAttributes(attr);
            });
            
            //Hover event listener
            AddEvent("hover", (e) =>
            {
                //Used for applying event based attributes
                UIComponentAttributes attr = new UIComponentAttributes();
                
                //UI defined handler
                _actionHover?.Invoke(e,attr);
                
                //Apply any attributes
                RenderAttributes = DefaultAttributes.CascadeAttributes(attr);

            });

            //Assign default click action
            _actionClick = actionClick;
            
            //Assign default hover action
            _actionHover = actionHover ?? ((e,a) =>
            {
                a.Background = Color.Red;
            });
        }
    }
}