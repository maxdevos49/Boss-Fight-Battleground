using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace BFB.Engine.UI.Components
{
    public class Component : IComponent
    {
        #region Node Info
        public IComponent Parent { get; set; }
        public List<IComponent> Children { get; set; }

        #endregion
        
        #region Component Properties
        
        public string Name { get; set; }
        
        public int X { get; set; }
        
        public int Y { get; set; }
        
        public int Width { get; set; }
        
        public int Height { get; set; }
        
        public Color Color { get; set; }
        
        public Color Background { get; set; }
        
        #endregion

        #region Constructor
        
        public Component()
        {
            //Component
            Name = "";
            X = 0;
            Y = 0;
            Width = 0;
            Height = 0;
            Color = Color.White;
            Background = Color.White;
            
            //Node tree
            Parent = null;
            Children = new List<IComponent>();
        }
        
        #endregion
        
        #region AddChild
        
        public void AddChild(IComponent component)
        {
            component.Parent = this;//Does not always work
            Children.Add(component);
            
//            var counter = 0;
//            var next = component;
//            while (true)
//            {
//                next = next.Parent;
//                    
//                if (next == null)
//                    break;
//
//                counter++;
//            }
//            Console.WriteLine($"Add Depth: {counter}");
            
        }
        
        #endregion
    }
}