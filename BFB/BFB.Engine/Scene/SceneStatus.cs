namespace BFB.Engine.Scene
{
    /// <summary>
    /// The state that a scene is in.
    /// </summary>
    public enum SceneStatus
    {
        Active,
        Inactive,
        Paused,
        Loading,
        Unloading,
        Hidden,
        Inoperable
    }
}
