using System;
using System.Collections.Generic;
using System.Linq;
using BFB.Engine.Event;
using BFB.Engine.UI.Modifiers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BFB.Engine.UI.Components
{
    public abstract class UIComponent
    {
        
        #region Properties
        
        public string Name { get; set; }
        
        public int Index { get; set; }
        
        public string TextureKey { get; set; }
        
        public string FontKey { get; set; }
        
        public float FontSize { get; set; }

        public int X { get; set; }
        
        public int Y { get; set; }
        
        public int Width { get; set; }
        
        public int Height { get; set; }
        
        public Color Color { get; set; }
        
        public Color Background { get; set; }
        
        public StackDirection StackDirection { get; set; }
        
        public int Grow { get; set; }
        
        public UIComponent Parent { get; set; }
        
        public List<UIComponent> Children { get; set; }

        private readonly Dictionary<string, Action<UIEvent>> _eventHandlers;

        private readonly List<UIConstraint> _constraints;

        private int _nextChildIndex;
        
        #endregion
        
        #region Constructor

        protected UIComponent(string name)
        {
            Name = name;
            Index = 0;
            TextureKey = "default";
            FontKey = "default";
            FontSize = 1f;
            X = 0;
            Y = 0;
            Width = 0;
            Height = 0;
            Color = Color.White;
            Background = Color.Transparent;
            
            Grow = 1;
            StackDirection = StackDirection.None;
            Children = new List<UIComponent>();
            Parent = null;
            _constraints = new List<UIConstraint>();
            _eventHandlers = new Dictionary<string, Action<UIEvent>>();
            _nextChildIndex = 0;
        }
        
        #endregion
        
        #region AddChild

        public void AddChild(UIComponent node)
        {
            node.Index = _nextChildIndex;
            _nextChildIndex++;

            node.Parent = this;
            Children.Add(node);
        }

        #endregion
        
        #region AddConstraint

        public UIComponent AddConstraint(UIConstraint constraint)
        {
            _constraints.Add(constraint);
            return this;
        }
        
        #endregion
        
        #region ProcessEvent

        public void ProcessEvent(UIEvent e)
        {
            foreach ((string key, Action<UIEvent> value) in _eventHandlers)
            {
                if (key == e.EventKey)
                    value.Invoke(e);

                if (!e.Propagate())
                    break;
            }
        }
        
        #endregion
        
        #region AddEvent

        public void AddEvent(string eventKey, Action<UIEvent> handler)
        {
            if (!_eventHandlers.ContainsKey(eventKey))
            {
                _eventHandlers.Add(eventKey, handler);
            }
        }
        
        #endregion
        
        #region Build
        
        /**
         * Used to generate a UIComponent making use of all applied constraints and modifiers.
         * Modifiers and constraints are applied in the same order as they are defined
         */
        public void Build(UILayer layer)
        {
            //Default take up as much space as available
            if (Parent != null)
            {
                X = Parent.X;
                Y = Parent.Y;
                Width = Parent.Width;
                Height = Parent.Height;
            }

            //Always apply all constraints
            foreach (UIConstraint uiConstraint in _constraints)
            {
                uiConstraint.Apply(this);
            }

            if (_eventHandlers.Any())
                layer.AddEventComponent(this);
                
            //If its the root node then parent will be null
            if (Parent == null) return;
            
            int totalProportionNumber = Parent.Children.Sum(x => x.Grow);
            int leadingProportionNumber = Parent.Children.TakeWhile(child => child.Index != Index).Sum(x => x.Grow);
            
            //Correctly render in the correct stack direction
            switch (Parent.StackDirection)
            {
                case StackDirection.Horizontal:

                    int oneProportionWidth = Width / totalProportionNumber;
                    
                    X += leadingProportionNumber * oneProportionWidth;
                    Width = oneProportionWidth * Grow;
                    Height = Height;
                    
                    break;
                case StackDirection.Vertical:

                    int oneProportionHeight = Height / totalProportionNumber;
                    
                    Y += leadingProportionNumber * oneProportionHeight;
                    Height = oneProportionHeight * Grow;
                    Width = Width;
                    
                    break;
            }
        }
        
        #endregion
        
        #region Render
        
        /**
         * Used to render the UIComponent by default as a simple panel
         */
        public virtual void Render(SpriteBatch graphics, Texture2D texture, SpriteFont font)
        {
            graphics.Draw(texture, new Rectangle(X, Y, Width,Height), Background);
        }
        
        #endregion
    }

    public enum StackDirection
    {
        Vertical,
        Horizontal,
        None
    }
}