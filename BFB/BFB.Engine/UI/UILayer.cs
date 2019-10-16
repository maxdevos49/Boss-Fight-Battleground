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

        private List<UIComponent> _eventComponents;

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

        public void ProcessEvents(UIEvent e)
        {
            foreach (UIComponent eventComponent in _eventComponents)
            {
                
            }
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