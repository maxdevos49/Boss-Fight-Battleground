using System.Collections.Generic;
using BFB.Engine.UI.Modifiers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BFB.Engine.UI.Components
{
    public abstract class UIComponent
    {
        
        #region Properties
        
        public string Name { get; set; }
        
        public string TextureKey { get; set; }

        public int X { get; set; }
        
        public int Y { get; set; }
        
        public int Width { get; set; }
        
        public int Height { get; set; }

        public Color Color { get; set; }
        
        public Color Background { get; set; }
        
        public UIComponent Parent { get; set; }
        
        public List<UIComponent> Children { get; set; }

        private List<UIConstraint> _constraints;

        
        #endregion
        
        #region Constructor

        public UIComponent(string name)
        {
            Name = name;
            TextureKey = "default";
            X = 0;
            Y = 0;
            Width = 0;
            Height = 0;
            Color = Color.Black;
            Background = Color.Transparent;
            Children = new List<UIComponent>();
            Parent = null;
            _constraints = new List<UIConstraint>();
        }
        
        #endregion
        
        #region AddChild

        public void AddChild(UIComponent node)
        {
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
        
        /**
         * Used to generate a UIComponent making use of all applied constraints and modifiers.
         * Modifiers and constraints are applied in the same order as they are defined
         */
        public virtual void Build()
        {
            foreach (UIConstraint uiConstraint in _constraints)
            {
                uiConstraint.Apply(this);
            }
        }
        
        /**
         * Used to render the UIComponent
         */
        public virtual void Render(SpriteBatch graphics, Texture2D texture)
        {
            graphics.Draw(texture, new Rectangle(X, Y, Width,Height), Background);
        }



    }
}