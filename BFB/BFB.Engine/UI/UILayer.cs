using System;
using System.Collections.Generic;
using BFB.Engine.Event;
using BFB.Engine.UI.Components;

namespace BFB.Engine.UI
{
    public abstract class UILayer
    {
        public string Key { get; }
        
        public UIRootComponent RootUI { get; set; }

        private readonly List<UIComponent> _eventComponents;

        protected UILayer(string key)
        {
            Key = key;
            RootUI = null;
            
            _eventComponents = new List<UIComponent>();
        }

        /**
         * Initializes the view layer for building
         */
        public void InitializeRoot(UIRootComponent rootNode)
        {
            _eventComponents.Clear();
            RootUI = rootNode;
        }

        /**
         * Returning true is to return as if nothing happened
         */
        public bool ProcessEvents(UIEvent e)
        {
            foreach (UIComponent eventComponent in _eventComponents)
            {
                //TODO add a tab index/active component. (For key events and tabbing)
                //TODO only pick elements focused or at location depending on if mouse or key events
                
                if (eventComponent.ProcessEvent(e))
                    return false;
            }

            return true;
        }

        public void AddEventComponent(UIComponent component)
        {
            _eventComponents.Add(component);
        }

        public void Stop()
        {
            RootUI = null;
            _eventComponents.Clear();
        }

        public abstract void Body();
    }
}