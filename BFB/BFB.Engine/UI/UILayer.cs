using System;
using System.Collections.Generic;
using System.Linq;
using BFB.Engine.Event;
using BFB.Engine.Scene;
using BFB.Engine.UI.Components;
using Microsoft.Xna.Framework.Input;

namespace BFB.Engine.UI
{
    /// <summary>
    /// Used to create a layer for ui elements
    /// </summary>
    public abstract class UILayer
    {
        #region Properties

        /// <summary>
        /// The identification key for the uilayer
        /// </summary>
        public string Key { get; }
    
        /// <summary>
        /// The UIManager for the UILayer
        /// </summary>
        public static UIManager UIManager { get; set; }
        
        /// <summary>
        /// The SceneManager class
        /// </summary>
        public static SceneManager SceneManager { get; set; }
        
        public static EventManager<GlobalEvent> GlobalEventManager { get; set; }
//        public static EventManager<InputEvent> InputEventManager { get; set; }
        
        /// <summary>
        /// Used to indicate if the scene should be drawn in debug mode
        /// </summary>
        public bool Debug { get; set; }
        
        /// <summary>
        /// Represents the foundation of the UI
        /// </summary>
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

        private readonly List<int> _eventGlobalListenerIds;

        #endregion
        
        #region Constructor
        
        /// <summary>
        /// Constructs a UILayer for displaying UIElements
        /// </summary>
        /// <param name="key"></param>
        protected UILayer(string key)
        {
            Key = key;
            RootUI = null;
            Debug = false;
            
            _eventGlobalListenerIds = new List<int>();
            _tabPosition = null;
            _tabIndex = new List<UIComponent>();
            _eventIndex = new List<UIComponent>();
        }
        
        #endregion

        #region Initilize Root
        
        /// <summary>
        /// Initializes the layer for drawing
        /// </summary>
        /// <param name="rootNode"></param>
        public void InitializeRoot(UIRootComponent rootNode)
        {
            _eventIndex.Clear();
            _tabIndex.Clear();
            RootUI = rootNode;
        }
        
        #endregion

        #region ProcessEvents
        
        /// <summary>
        /// Processes Input Events into UIEvents
        /// </summary>
        /// <param name="events"></param>
        /// <returns></returns>
        public bool ProcessEvents(IEnumerable<UIEvent> events)
        {

            bool eventNotAccepted = true;
            
            foreach (UIEvent uiEvent in events)
            {

                foreach (UIComponent component in _eventIndex)
                {
                    component.RenderAttributes = component.DefaultAttributes.CascadeAttributes(component.DefaultAttributes);
                }
                
                if (uiEvent.EventKey == "click" || uiEvent.EventKey == "mouseup" || uiEvent.EventKey == "hover")
                {
                    List<UIComponent> components = _eventIndex.Where(c => c.DefaultAttributes.X <= uiEvent.Mouse.X  && (c.DefaultAttributes.X + c.DefaultAttributes.Width) >= uiEvent.Mouse.X 
                                                                        && c.DefaultAttributes.Y <= uiEvent.Mouse.Y  && (c.DefaultAttributes.Y + c.DefaultAttributes.Height) >= uiEvent.Mouse.Y)
                                                                .OrderBy(c => c.DefaultAttributes.Width * c.DefaultAttributes.Height)//Smaller areas are closer to the top
                                                                .ToList();

                    if (!components.Any()) 
                        continue;

                    //Focus component if it is focusable
                    if (uiEvent.EventKey == "click")
                    {
                        if (_tabPosition != null)
                            _tabIndex[(int)_tabPosition].Focused = false;
                            
                        components[0].Focused = true;
                        _tabPosition = _tabIndex.FindIndex(x => x == components[0]);
                    }
                    
                    //Start processing each component
                    foreach (UIComponent component in components)
                    {
                        uiEvent.Component = component;
                        
                        //Process event here
                        component.ProcessEvent(uiEvent);
                       
                        if ( uiEvent.Propagate())
                            eventNotAccepted = false;
                    }
                }
                else
                {

                    if (uiEvent.Keyboard.KeyboardState.IsKeyDown(Keys.Escape))
                    {
                        if(_tabPosition!= null)
                            _tabIndex[(int) _tabPosition].Focused = false;

                        _tabPosition = null;
                    }
                    
                    if (uiEvent.EventKey == "focus" && _tabIndex.Any())
                    {
                        if(_tabPosition != null)
                            _tabIndex[(int) _tabPosition].Focused = false;
                        
                        if (_tabPosition == null)
                            _tabPosition = 0;
                        else if (uiEvent.Keyboard.KeyboardState.IsKeyDown(Keys.LeftShift))
                            _tabPosition--;//shift + tab
                        else
                            _tabPosition++;//tab

                        if (_tabPosition > _tabIndex.Count - 1)
                            _tabPosition = 0;
                        else if (_tabPosition < 0)
                            _tabPosition = _tabIndex.Count - 1;

                        _tabIndex[(int) _tabPosition].Focused = true;

                    }

                    //If no focusable elements then we are done
                    if (!_tabIndex.Any()) 
                        continue;

                    if (_tabPosition == null) 
                        continue;
                    
                    uiEvent.Component = _tabIndex[(int) _tabPosition];
                        
                    _tabIndex[(int) _tabPosition].ProcessEvent(uiEvent);

                }
            }

            return eventNotAccepted;
        }

        #endregion
        
        #region AddEventComponent
        
        /// <summary>
        /// Adds a UIComponents that uses events into the Event Index
        /// </summary>
        /// <param name="component">The UIComponent that will emit events</param>
        public void AddEventComponent(UIComponent component)
        {
            _eventIndex.Add(component);
        }
        
        #endregion
        
        #region AddTabIndexComponent
        
        /// <summary>
        /// Adds a UIComponent to the tabindex if it can be tabbed
        /// </summary>
        /// <param name="component">The UIComponent that can be tabbed</param>
        public void AddTabIndexComponent(UIComponent component)
        {
            _tabIndex.Add(component);
        }
        
        #endregion
        
        #region AddGlobalListener
        
        /// <summary>
        /// Allows for adding of event listeners that are automatically disposed of when the UILayer is stopped
        /// </summary>
        /// <param name="eventKey">The event to listen for</param>
        /// <param name="eventHandler">The event handler to perform an action when a event is revieved</param>
        public void AddGlobalListener(string eventKey, Action<GlobalEvent> eventHandler)
        {
            _eventGlobalListenerIds.Add(GlobalEventManager.AddEventListener(eventKey, eventHandler));
        }
        
        #endregion
        
        #region Start

        public void Start()
        {
            Init();
        }
        
        #endregion

        #region Init
        
        /// <summary>
        /// An optional init method for a UILayer to use for setup. Called every time a UILayer is started
        /// </summary>
        protected virtual void Init() {}
        
        #endregion

        #region Stop
        
        /// <summary>
        /// Called when a UILayer needs to be stopped
        /// </summary>
        public void Stop()
        {
            RootUI = null;
            _eventIndex.Clear();
            _tabIndex.Clear();
            
            foreach (int id in _eventGlobalListenerIds)
                GlobalEventManager.RemoveEventListener(id);
        }
        
        #endregion
        

        /// <summary>
        /// The area to define the UIComponent element layouts
        /// </summary>
        public abstract void Body();
    }
}