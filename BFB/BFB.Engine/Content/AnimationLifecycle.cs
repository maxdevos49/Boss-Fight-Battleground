namespace BFB.Engine.Content
{
    /// <summary>
    /// Different states that an animation can be in. Paused means that the animation is stopped, playing means that the animation is playing, and looping means the animation will repeat once it cycles through.
    /// </summary>
    public enum AnimationLifecycle
    {
        Paused,
        Playing,
        Looping,
    }
}