using System;
using BFB.Engine.Math;

namespace BFB.Engine.Server.Communication
{
    [Serializable]
    public class InputMessage : DataMessage
    {
        public BfbVector MousePosition { get; set; }
        //Add player input vector to here but do it as separate properties
       // public PlayerState PlayerInputState { get; set; } // add this and remove mouse position wh
    }
}