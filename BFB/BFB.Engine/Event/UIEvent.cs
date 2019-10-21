using System.Collections.Generic;
using BFB.Engine.UI.Components;
using Microsoft.Xna.Framework.Input;

namespace BFB.Engine.Event
{
    public class UIEvent : InputEvent
    {
        
        public UIComponent Component { get; set; }

        public UIEvent(InputEvent inputEvent)
        {
            EventId = inputEvent.EventId;
            Keyboard = inputEvent.Keyboard;
            Mouse = inputEvent.Mouse;
        }

        
        public static List<UIEvent> ConvertInputEventToUIEvent(InputEvent inputEvent)
        {
            List<UIEvent> eventList = new List<UIEvent>();
            
            //Using existing input events to generate the new event type
            switch (inputEvent.EventKey)
            {
                case "mousemove":
                    //hover event
                    eventList.Add(new UIEvent(inputEvent)
                    {
                        EventKey = "hover"
                    });
                    
                    break;
                case "mouseclick":
                    
                    //click event
                    eventList.Add(new UIEvent(inputEvent)
                    {
                        EventKey = "click"
                    });
                    
                    break;
                case "keypress":
                    
                    //keypress, focus, unfocus
                    eventList.Add(new UIEvent(inputEvent)
                    {
                        EventKey = "keypress"
                    });
                    
                    //only changes focus with tab
                    if (inputEvent.Keyboard.KeyEnum == Keys.Tab)
                    {
                        eventList.Add(new UIEvent(inputEvent)
                        {
                            EventKey = "focus"
                        });

                        eventList.Add(new UIEvent(inputEvent)
                        {
                            EventKey = "unfocus"
                        });
                    }

                    break;
                case "keyup":
                    
                    //keyup
                    eventList.Add(new UIEvent(inputEvent)
                    {
                        EventKey = "keyup"
                    });
                    
                    break;
                case "keydown":
                    
                    //keydown
                    eventList.Add(new UIEvent(inputEvent)
                    {
                        EventKey = "keydown"
                    });
                    
                    break;
            }

            return eventList;
        }
    }
}