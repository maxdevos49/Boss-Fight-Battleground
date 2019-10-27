using System;

namespace BFB.Engine.Content
{
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