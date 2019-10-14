using System;
using System.Collections.Generic;
using System.Linq;
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

        public readonly List<UIConstraint> Constraints;

        private int _nextChildIndex;
        
        #endregion
        
        #region Constructor

        public UIComponent(string name)
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
            Color = Color.Black;
            Background = Color.Transparent;
            
            Grow = 1;
            StackDirection = StackDirection.Horizontal;
            Children = new List<UIComponent>();
            Parent = null;
            Constraints = new List<UIConstraint>();

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
            Constraints.Add(constraint);
            return this;
        }
        
        #endregion
        
        /**
         * Used to generate a UIComponent making use of all applied constraints and modifiers.
         * Modifiers and constraints are applied in the same order as they are defined
         */
        public virtual void Build()
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
            foreach (UIConstraint uiConstraint in Constraints)
            {
                uiConstraint.Apply(this);
            }

            //If its the root node then parent will be null
            if (Parent == null) return;
            
            int totalProportionNumber = Parent.Children.Sum(x => x.Grow);
            int leadingProportionNumber = Parent.Children.TakeWhile(child => child.Index != Index).Sum(x => x.Grow);
            
            //Correctly render in the correct stack direction
            switch (Parent.StackDirection)
            {
                case StackDirection.Horizontal:

                    int oneProportionWidth = Width / totalProportionNumber;
                    
                    X += leadingProportionNumber * oneProportionWidth ;
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
        
        /**
         * Used to render the UIComponent by default as a simple panel
         */
        public virtual void Render(SpriteBatch graphics, Texture2D texture, SpriteFont font)
        {
            graphics.Draw(texture, new Rectangle(X, Y, Width,Height), Background);
        }
    }

    public enum StackDirection
    {
        Vertical,
        Horizontal,
        None
    }
}