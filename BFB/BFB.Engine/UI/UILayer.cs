using System;
using System.Collections.Generic;
using System.Linq;
using BFB.Engine.Event;
using BFB.Engine.Scene;
using BFB.Engine.UI.Components;

namespace BFB.Engine.UI
{
    public abstract class UILayer
    {
        #region Properties
        
        public static UIManager UIManager { get; set; }
        
        public static SceneManager SceneManager { get; set; }
        
        public string Key { get; }
        
        /**
         * Represents the foundation of the root UI
         */
        public UIRootComponent RootUI { get; set; }

        /**
         * Indicates the current focus position of the tabIndex
         */
        private int? _tabPosition;
        
        /**
         * Contains the elements that can be tabbed or focused
         */
        private readonly List<UIComponent> _tabIndex;
        
        /**
         * Contains the elements that have events tied to them
         */
        private readonly List<UIComponent> _eventIndex;

        #endregion
        
        #region Constructor
        protected UILayer(string key)
        {
            Key = key;
            RootUI = null;
            
            _tabPosition = null;
            _tabIndex = new List<UIComponent>();
            _eventIndex = new List<UIComponent>();
        }
        
        #endregion

        #region Initilize Root
        
        /**
         * Initializes the view layer for building
         */
        public void InitializeRoot(UIRootComponent rootNode)
        {
            _eventIndex.Clear();
            _tabIndex.Clear();
            RootUI = rootNode;
        }
        
        #endregion

        #region ProcessEvents
        
        /**
         * Returning true is to return as if nothing happened
         */
        public bool ProcessEvents(List<UIEvent> events)
        {

            bool eventNotAccepted = true;
            foreach (UIEvent uiEvent in events)
            {

                foreach (UIComponent component in _eventIndex)
                {
                    component.RenderAttributes = component.DefaultAttributes.CascadeAttributes(component.DefaultAttributes);
                }
                
                if (uiEvent.EventKey == "click"
                    || uiEvent.EventKey == "mouseup"
                    || uiEvent.EventKey == "hover")
                {
                    List<UIComponent> components = _eventIndex.Where(c =>
                                                                        c.DefaultAttributes.X <= uiEvent.Mouse.X 
                                                                        && (c.DefaultAttributes.X + c.DefaultAttributes.Width) >= uiEvent.Mouse.X 
                                                                        && c.DefaultAttributes.Y <= uiEvent.Mouse.Y 
                                                                        && (c.DefaultAttributes.Y + c.DefaultAttributes.Height) >= uiEvent.Mouse.Y)
                                                                .OrderBy(c => c.DefaultAttributes.Width * c.DefaultAttributes.Height)//Smaller areas are closer to the top
                                                                .ToList();
                    

                    //If any components were clicked
                    if (!components.Any()) continue;
                    
                    //Start processing each component
                    foreach (UIComponent component in components)
                    {

                        
                        uiEvent.Component = component;
                        
                        //Process event
                        component.ProcessEvent(uiEvent);
                       
                        if ( uiEvent.Propagate())
                            eventNotAccepted = false;
                    }
                }
                else
                {
                    Console.WriteLine($"UIEvent: Other");    
                    
                    if (_tabPosition != null)
                        uiEvent.Component = _tabIndex[_tabPosition ?? 0];
                    
                }
            }

            return eventNotAccepted;
        }

        #endregion
        
        #region AddEventComponent
        
        public void AddEventComponent(UIComponent component)
        {
            _eventIndex.Add(component);
        }
        
        #endregion
        
        #region AddTabIndexComponent
        
        public void AddTabIndexComponent(UIComponent component)
        {
            _tabIndex.Add(component);
        }
        
        #endregion

        #region Init
        
        public virtual void Init()
        {
           //Used for initilizing a UI layer 
        }
        
        #endregion

        #region Stop
        
        public void Stop()
        {
            RootUI = null;
            _eventIndex.Clear();
            _tabIndex.Clear();
        }
        
        #endregion

        public abstract void Body();
    }
}