using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace BFB.Engine.Content
{
    public class AnimatedTexture
    {
        /**
         * Used to identify the animated texture. This should be unique from all other textures
         */
        public string Key { get; set; }

        public string Location { get; set; }

        public Texture2D Texture { get; set; }
        
        public int Frames { get; set; }

        public int FrameHeight { get; set; }

        public int FrameWidth { get; set; }

        public int Columns { get; set; }

        public int Rows { get; set; }
        
        public float Scale { get; set; }

        public Dictionary<AnimationState,AnimationSet> AnimationSets { get; set; }

    }
    
}