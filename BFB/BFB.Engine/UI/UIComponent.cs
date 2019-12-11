using System;
using System.Collections.Generic;
using System.Linq;
using BFB.Engine.Content;
using BFB.Engine.Event;
using BFB.Engine.Helpers;
using BFB.Engine.UI.Constraints;
using JetBrains.Annotations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BFB.Engine.UI
{
    public abstract class UIComponent
    {
        
        #region Properties
        
        public static UIManager UIManager { get; set; }
        
        public UILayer ParentLayer { get; set; }
        
        
        [UsedImplicitly]
        public string Name { get; set; }
        
        /**
         * Indicates the child Index the component is. Used for stacking elements properly
         */
        private int Index { get; set; }
        
        
        public string Text { get; set; }
        
        /**
         * Used when building component and assigning child component indexs
         */
        private int NextChildIndex { get; set; }
        

        /**
         * Indicates with true or false whether the element can be focused using tab
         * on the keyboard or using a click event
         */
        [UsedImplicitly]
        public bool Focusable { get; set; }

        [UsedImplicitly]
        public bool Updateable { get; set; }
        
        /**
         * Indicates whether the element is focused or not
         */
        public bool Focused { get; set; }
        
        public bool IsHovered { get; set; }

        /**
         * Contains the constraints that will be applied to the component
         * when the component is built. Components will be applied in the
         * same order as they are added.
         */
        private List<UIConstraint> Constraints { get; }
        
        /**
         * Holds any event listeners the element may have. These handlers
         * only take UIEvents so any events that it can receive must be a
         * UIEvent
         */
        private Dictionary<string, Action<UIEvent>> EventHandlers { get; }
        
        /**
         * Reference to the parent component. This is null if it is the root element
         */
        [CanBeNull]
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
        public UIAttributes DefaultAttributes { get; set; }
        
        /**
         * These attributes are always used at render time and must be regenerated every
         * render cycle. They are what allows say a button to return to its default
         * state with out writing extra code to return it to that state
         */
        public UIAttributes RenderAttributes { get; set; }

        
        #endregion
        
        #region Constructor

        protected UIComponent(string name, bool updateable = false)
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
            DefaultAttributes = new UIAttributes
            {
                TextureKey = "default",
                FontKey = "default",
                Grow = 1,
                FontSize = 1,
                Background = Color.Transparent,
                Color = Color.Black,
                Position = Position.Relative
            };
            
            RenderAttributes = new UIAttributes();
            
            //Events and focus
            Focusable = false;
            Updateable = updateable;
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
                EventHandlers[e.EventKey](e);
        }
        
        #endregion
        
        #region AddEvent

        protected void AddEvent(string eventKey, Action<UIEvent> handler)
        {
            if (!EventHandlers.ContainsKey(eventKey))
                EventHandlers.Add(eventKey, handler);
        }
        
        #endregion
        
        #region Build
        
        /**
         * Used to generate a UIComponent making use of all applied constraints and modifiers.
         * Modifiers and constraints are applied in the same order as they are defined
         */
        public void Build(UILayer layer)
        {
            #region Inherit/Defaults

            DefaultAttributes.X = Parent?.DefaultAttributes.X ?? 0;
            DefaultAttributes.Y = Parent?.DefaultAttributes.Y ?? 0;
            DefaultAttributes.Width = Parent?.DefaultAttributes.Width ?? DefaultAttributes.Width;
            DefaultAttributes.Height = Parent?.DefaultAttributes.Height ?? DefaultAttributes.Height;

            if (DefaultAttributes.StackDirection == StackDirection.Inherit)
                DefaultAttributes.StackDirection = Parent?.DefaultAttributes.StackDirection ?? StackDirection.None;

            if (DefaultAttributes.Overflow == Overflow.Inherit)
                DefaultAttributes.Overflow = Parent?.DefaultAttributes.Overflow ?? Overflow.Show;

            if (DefaultAttributes.Sizing == Sizing.Inherit)
                DefaultAttributes.Sizing = Parent?.DefaultAttributes.Sizing ?? Sizing.Proportion;

            if (DefaultAttributes.Position == Position.Inherit)
                DefaultAttributes.Position = Parent?.DefaultAttributes.Position ?? Position.Relative;
            
            if (DefaultAttributes.JustifyText == JustifyText.Inherit)
                DefaultAttributes.JustifyText = Parent?.DefaultAttributes.JustifyText ?? JustifyText.Start;

            if (DefaultAttributes.TextWrap == TextWrap.Inherit)
                DefaultAttributes.TextWrap = Parent?.DefaultAttributes.TextWrap ?? TextWrap.Wrap;
            
            if (DefaultAttributes.FontScaleMode == FontScaleMode.Inherit)
                DefaultAttributes.FontScaleMode = Parent?.DefaultAttributes.FontScaleMode ?? FontScaleMode.FontSizeScale;
            
            if (DefaultAttributes.VerticalAlignText == VerticalAlignText.Inherit)
                DefaultAttributes.VerticalAlignText = Parent?.DefaultAttributes.VerticalAlignText ?? VerticalAlignText.Start;

            #endregion

            #region Apply Contraints/ Build Indexes

            if (EventHandlers.Any())
                layer.AddEventComponent(this);
            
            if(Focusable)
                layer.AddTabIndexComponent(this);
            
            if(Updateable)
                layer.AddUpdateIndexComponent(this);
                
            #endregion
            
            //If the parent node is null then we stop here
            if (Parent != null)
            {

                if (Parent.DefaultAttributes.Sizing == Sizing.Proportion)
                {
                    #region Proportional Container Positioning
                
                    int totalProportionNumber = Parent.Children.Sum(x => x.DefaultAttributes.Grow);
                    int leadingProportionNumber = Parent.Children.TakeWhile(child => child.Index != Index).Sum(x => x.DefaultAttributes.Grow);
                    
                    //Correctly render in the correct stack direction
                    switch (Parent.DefaultAttributes.StackDirection)
                    {
                        case StackDirection.Horizontal:

                            int oneProportionWidth = DefaultAttributes.Width / totalProportionNumber;
                            
                            DefaultAttributes.X += leadingProportionNumber * oneProportionWidth;
                            DefaultAttributes.OffsetX = leadingProportionNumber * oneProportionWidth;
                            DefaultAttributes.Width = oneProportionWidth * DefaultAttributes.Grow;
                            break;
                        case StackDirection.Vertical:

                            int oneProportionHeight = DefaultAttributes.Height / totalProportionNumber;
                            
                            DefaultAttributes.Y += leadingProportionNumber * oneProportionHeight;
                            DefaultAttributes.OffsetY = leadingProportionNumber * oneProportionHeight;
                            DefaultAttributes.Height = oneProportionHeight * DefaultAttributes.Grow;
                            break;
                    }
                    
                    #endregion
                }
                else if (Parent.DefaultAttributes.Sizing == Sizing.Dimension)
                {
                    #region Dimensional Container Positioning
                    
                    switch (Parent.DefaultAttributes.StackDirection)
                    {
                        case StackDirection.Horizontal:
                            int nextWidth = Index * Parent.Children[0].DefaultAttributes.Width;
                            DefaultAttributes.X += nextWidth;
                            DefaultAttributes.OffsetX = nextWidth;
                            break;
                        case StackDirection.Vertical:
                            int nextHeight = Index * Parent.Children[0].DefaultAttributes.Height;
                            DefaultAttributes.Y += nextHeight;
                            DefaultAttributes.OffsetY = nextHeight;
                            break;
                    }
                    
                    #endregion
                }
            }
            
            foreach (UIConstraint uiConstraint in Constraints)
                uiConstraint.Apply(this);

            RenderAttributes = DefaultAttributes.CascadeAttributes(DefaultAttributes);
        }
        
        #endregion
        
        #region Render
        
        /**
         * Used to render the UIComponent by default as a simple panel
         */
        public virtual void Render(SpriteBatch graphics, BFBContentManager content)
        {
            if (RenderAttributes.Position == Position.Relative)
            {
                RenderAttributes.X = RenderAttributes.OffsetX + Parent?.RenderAttributes.X ?? 0; 
                RenderAttributes.Y = RenderAttributes.OffsetY + Parent?.RenderAttributes.Y ?? 0; 
            }

            graphics.Draw(
                content.GetTexture(RenderAttributes.TextureKey),
                new Rectangle(
                    RenderAttributes.X, 
                    RenderAttributes.Y,
                    RenderAttributes.Width,
                    RenderAttributes.Height), 
                RenderAttributes.Background);

            if (!string.IsNullOrEmpty(Text))
                graphics.DrawUIText(this, content);

            if (RenderAttributes.BorderSize > 0)
                graphics.DrawBorder(new Rectangle(
                                        RenderAttributes.X,
                                        RenderAttributes.Y, 
                                        RenderAttributes.Width,
                                        RenderAttributes.Height),
                                    RenderAttributes.BorderSize,
                                    RenderAttributes.BorderColor,
                                    content.GetTexture("default"));
        }
        
        #endregion
        
        public virtual void Update(GameTime time) {}
        
    }

}