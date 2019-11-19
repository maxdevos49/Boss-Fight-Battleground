using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace BFB.Engine.Content
{
    /// <summary>
    /// A texture on the client side that is not static, and animates.
    /// </summary>
    public class AnimatedTexture
    {
        /// <summary>
        /// Used to identify the animated texture. This should be unique from all other textures
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// The location based off the content managagers root directory for the .xnb of the texture.
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// The texture after it is loaded with the LoadContent method.
        /// </summary>
        public Texture2D Texture { get; set; }
        
        /// <summary>
        /// The number of frames, or images, in the animation.
        /// </summary>
        public int Frames { get; set; }

        /// <summary>
        /// The height, in pixel, of each frame.
        /// </summary>
        public int FrameHeight { get; set; }

        /// <summary>
        /// The width, in pixels, of each frame.
        /// </summary>
        public int FrameWidth { get; set; }

        /// <summary>
        /// The number of columns in the spritesheet.
        /// </summary>
        public int Columns { get; set; }

        /// <summary>
        /// The number of rows in the spritesheet
        /// </summary>
        public int Rows { get; set; }
        
        /// <summary>
        /// How big the texture is relative to its original size.
        /// </summary>
        public float Scale { get; set; }
        
        // Use the format "<r>,<g>,<b>,<alpha>" to specify a color in the json
        public string ColorConfig { get; set; }
        
        /// <summary>
        /// The parsed color
        /// </summary>
        [JsonIgnore]
        public Color ParsedColor { get; set; }
        
        /// <summary>
        /// If a random color should be used as a tint
        /// </summary>
        public bool RandomColor {get;set;}


        /// <summary>
        /// The different animations that one texture can play.
        /// </summary>
        public Dictionary<AnimationState,AnimationSet> AnimationSets { get; set; }

    }
    
}