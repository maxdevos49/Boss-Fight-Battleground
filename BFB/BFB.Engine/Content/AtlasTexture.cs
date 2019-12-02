using Microsoft.Xna.Framework.Graphics;

namespace BFB.Engine.Content
{
    public class AtlasTexture
    {
        public string TextureKey { get; set; }
        
        public Texture2D Texture { get; set; }
        
        public float Scale { get; set; }
        
        public int X { get; set; }
        
        public int Y { get; set; }
        
        public int Width { get; set; }
        
        public int Height { get; set; }
    }
}