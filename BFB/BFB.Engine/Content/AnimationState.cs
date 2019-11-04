using System;

namespace BFB.Engine.Content
{
    /// <summary>
    /// The animation state, decided by the server, so it is serializable. The client then learns what the animation state is and figures out how to get that from the spritesheet.
    /// </summary>
    [Serializable]
    public enum AnimationState
    {
        None,
        IdleLeft,
        IdleRight,
        MoveLeft,
        MoveRight,
        Jump,
        Fall
    }
}