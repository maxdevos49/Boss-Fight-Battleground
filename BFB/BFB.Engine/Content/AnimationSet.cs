namespace BFB.Engine.Content
{
    /// <summary>
    /// A series of information about an animation.
    /// </summary>
    public class AnimationSet
    {
        /// <summary>
        /// The frames per second that an animation will animate at.
        /// </summary>
        public int Fps { get; set; }
        /// <summary>
        /// Which frame of the spritesheet the animation starts at since there are different animations in one spritesheet.
        /// </summary>
        public int FrameStart { get; set; }
        /// <summary>
        /// Which frame of the spritesheet the animation ends at, since there are different animations in one spritesheet.
        /// </summary>
        public int FrameEnd { get; set; }
        /// <summary>
        /// If the sprite is flipped, for example running right vs running left using the same animation, but one is flipped.
        /// </summary>
        public bool Mirror { get; set; }
    }
}