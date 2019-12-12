using Microsoft.Xna.Framework;

namespace BFB.Engine.Helpers
{
    public class BfbColor
    {
        private byte R { get; set; }
        private byte G { get; set; }
        private byte B { get; set; }
        private byte A { get; set; }

        public BfbColor(byte r, byte g,byte b, byte a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }
        
        public Color ToMonoColor()
        {
            return new Color
            {
                R = R,
                G = G,
                B = B,
                A = A
            };
        }
        
    }
}