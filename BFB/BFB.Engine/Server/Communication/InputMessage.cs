using System;
using BFB.Engine.Entity;
using BFB.Engine.Math;

namespace BFB.Engine.Server.Communication
{
    [Serializable]
    public class InputMessage : DataMessage
    {
        public PlayerState PlayerInputState { get; set; }
    }
}