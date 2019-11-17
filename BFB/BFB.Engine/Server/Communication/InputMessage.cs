using System;
using BFB.Engine.Entity;
using BFB.Engine.Input.PlayerInput;
using BFB.Engine.Math;

namespace BFB.Engine.Server.Communication
{
    /// <summary>
    /// Used to pass input from the client to the server
    /// </summary>
    [Serializable]
    public class InputMessage : DataMessage
    {
        /// <summary>
        /// The players current input
        /// </summary>
        public PlayerState PlayerInputState { get; set; }
    }
}