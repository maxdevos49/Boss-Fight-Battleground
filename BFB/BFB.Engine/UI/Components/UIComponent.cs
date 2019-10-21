using System;
using System.Collections.Generic;
using System.Linq;
using BFB.Engine.Event;
using BFB.Engine.UI.Constraints;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BFB.Engine.UI.Components
{
    public abstract class UIComponent
    {
        
        #region Properties
        
        /**
         * Indicates the name of the UI Element. Mostly used for debugging
         * purposes and viewing what element is present
         */
        public string Name { get; }
        
        /**
         * Indicates the child Index the component is. Used for stacking elements properly
         */
        private int Index { get; set; }
        
        /**
         * Used when building component and assigning child component indexs
         */
        private int NextChildIndex { get; set; }
        

        /**
         * Indicates with true or false whether the element can be focused using tab
         * on the keyboard or using a click event
         */
        public bool Focusable { get; protected set; }
        

        /**
         * Contains the constraints that will be applied to the component
         * when the component is built. Components will be applied in the
         * same order as they are added.
         */
        protected List<UIConstraint> Constraints { get; }
        
        /**
         * Holds any event listeners the element may have. These handlers
         * only take UIEvents so any events that it can receive must be a
         * UIEvent
         */
        protected Dictionary<string, Action<UIEvent>> EventHandlers { get; }
        
        /**
         * Reference to the parent component. This is null if it is the root element
         */
        public UIComponent Parent { get; private set; }

        /**
         * Holds a list of child components. Children order is maintained
         * in the order of which they are added so container ordering
         * follows this
         */
        public List<UIComponent> Children { get; }


        /**
         * These attributes are generated when the component is first built.
         * They are only updated when they are built
         */
        public UIComponentAttributes DefaultAttributes { get; set; }
        
        /**
         * These attributes are always used at render time and must be regenerated every
         * render cycle. They are what allows say a button to return to its default
         * state with out writing extra code to return it to that state
         */
        public UIComponentAttributes RenderAttributes { get; set; }

        
        #endregion
        
        #region Constructor

        protected UIComponent(string name)
        {
            //Component name
            Name = name;
            
            //Indexing for drawing children
            Index = 0;
            NextChildIndex = 0;
            
            //Parent and children
            Parent = null;
            Children = new List<UIComponent>();
            
            //UIConstraints and attributes
            Constraints = new List<UIConstraint>();
            DefaultAttributes = new UIComponentAttributes
            {
                TextureKey = "default",
                FontKey = "default",
                Grow = 1,
                FontSize = 1,
                Background = Color.Transparent,
                Color = Color.Black,
            };
            
            RenderAttributes = new UIComponentAttributes();
            
            //Events and focus
            Focusable = false;
            EventHandlers = new Dictionary<string, Action<UIEvent>>();
            
            
        }
        
        #endregion
        
        #region AddChild

        public void AddChild(UIComponent node)
        {
            node.Index = NextChildIndex;
            NextChildIndex++;

            node.Parent = this;
            Children.Add(node);
        }

        #endregion
        
        #region AddConstraint

        public UIComponent AddConstraint(UIConstraint constraint)
        {
            Constraints.Add(constraint);
            return this;
        }
        
        #endregion
        
        #region ProcessEvent

        /**
         * Returning true means do everything as normal or no event was caught
         */
        public void ProcessEvent(UIEvent e)
        {

            if (EventHandlers.ContainsKey(e.EventKey))
            {
                EventHandlers[e.EventKey](e);

            }
        }
        
        #endregion
        
        #region AddEvent

        public void AddEvent(string eventKey, Action<UIEvent> handler)
        {
            if (!EventHandlers.ContainsKey(eventKey))
            {
                EventHandlers.Add(eventKey, handler);
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
                DefaultAttributes.X = Parent.DefaultAttributes.X;
                DefaultAttributes.Y = Parent.DefaultAttributes.Y;
                DefaultAttributes.Width = Parent.DefaultAttributes.Width;
                DefaultAttributes.Height = Parent.DefaultAttributes.Height;
            }

            //Always apply all constraints
            foreach (UIConstraint uiConstraint in Constraints)
            {
                uiConstraint.Apply(this);
            }

            if (EventHandlers.Any())
                layer.AddEventComponent(this);
            
            if(Focusable)
                layer.AddTabIndexComponent(this);
                
            //If its the root node then parent will be null
            if (Parent == null)
            {
                RenderAttributes = DefaultAttributes.CascadeAttributes(DefaultAttributes);
                return;
            }
            
            int totalProportionNumber = Parent.Children.Sum(x => x.DefaultAttributes.Grow);
            int leadingProportionNumber = Parent.Children.TakeWhile(child => child.Index != Index).Sum(x => x.DefaultAttributes.Grow);
            
            //Correctly render in the correct stack direction
            switch (Parent.DefaultAttributes.StackDirection)
            {
                case StackDirection.Horizontal:

                    int oneProportionWidth = (DefaultAttributes.Width) / totalProportionNumber;
                    
                    DefaultAttributes.X += leadingProportionNumber * oneProportionWidth;
                    DefaultAttributes.Width = oneProportionWidth * DefaultAttributes.Grow;
                    DefaultAttributes.Height = DefaultAttributes.Height;
                    
                    break;
                case StackDirection.Vertical:

                    int oneProportionHeight = (DefaultAttributes.Height) / totalProportionNumber;
                    
                    DefaultAttributes.Y += leadingProportionNumber * oneProportionHeight;
                    DefaultAttributes.Height = oneProportionHeight * DefaultAttributes.Grow;
                    DefaultAttributes.Width = DefaultAttributes.Width;
                    
                    break;
            }
            
            RenderAttributes = DefaultAttributes.CascadeAttributes(DefaultAttributes);

        }
        
        #endregion
        
        #region Render
        
        /**
         * Used to render the UIComponent by default as a simple panel
         */
        public virtual void Render(SpriteBatch graphics, Texture2D texture, SpriteFont font)
        {
            
            graphics.Draw(texture, new Rectangle(RenderAttributes.X, RenderAttributes.Y, RenderAttributes.Width,RenderAttributes.Height), RenderAttributes.Background);
            
        }
        
        #endregion
    }

    #region UIAttributes
    
    public class UIComponentAttributes
    {
        #region Properties
        
        public string TextureKey { get; set; }
        
        public string FontKey { get; set; }
        
        public float FontSize { get; set; }

        public int X { get; set; }
        
        public int Y { get; set; }
        
        public int Width { get; set; }
        
        public int Height { get; set; }
        
        public Color Color { get; set; }
        
        public Color Background { get; set; }
        
        /**
          * Describes the proportion that the element should take up depending on
          * the stack direction and amount of children. All components have a
          * default of 1. If a component is assigned 2 and 3 other siblings have
          * the default of 1 then the component assigned 2 will be the size of two
          * components if there was 5 components split evenly
          */
        public int Grow { get; set; }
        
        /**
          * Indicates the stack direction that the child elements will stack within a container.
          * Elements will always start on the leading side
          */
        public StackDirection StackDirection { get; set; }
        
        #endregion

        /**
         * This methods takes a attribute object and overrides styles that
         * are present and not null. All properties are null by default and
         * will only be cascaded if is present
         */
        public UIComponentAttributes CascadeAttributes(UIComponentAttributes cascadingAttributes)
        {
            return new UIComponentAttributes
            {
                TextureKey = cascadingAttributes.TextureKey ?? TextureKey,
                FontKey = cascadingAttributes.FontKey ?? FontKey,
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                FontSize = cascadingAttributes.FontSize != 1 ? cascadingAttributes.FontSize : FontSize,
                X = cascadingAttributes.X != 0 ? cascadingAttributes.X : X,
                Y = cascadingAttributes.Y != 0 ? cascadingAttributes.Y : Y,
                Width = cascadingAttributes.Width != 0 ? cascadingAttributes.Width : Width,
                Height = cascadingAttributes.Height != 0 ? cascadingAttributes.Height : Height,
                Color = cascadingAttributes.Color != Color.Black ? cascadingAttributes.Color : Color,
                Background = cascadingAttributes.Background != Color.Transparent ? cascadingAttributes.Background : Background,
                Grow = cascadingAttributes.Grow != 1 ? cascadingAttributes.Grow : Grow,
                StackDirection= cascadingAttributes.StackDirection != StackDirection.Horizontal ? cascadingAttributes.StackDirection : StackDirection,
            };
        }
        
    }
    #endregion
    
    public enum StackDirection
    {
        Vertical,
        Horizontal,
        None
    }
}