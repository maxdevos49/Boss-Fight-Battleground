using Microsoft.Xna.Framework;

namespace BFB.Engine.UI.Components
{
    public interface IComponent : INode
    {
        string Name { get; set; }
        int X { get; set; }
        int Y { get; set; }
        int Width { get; set; }
        int Height { get; set; }
        
        Color Background { get; set; }
        Color Color { get; set; }
        
        
        
        
    }
}