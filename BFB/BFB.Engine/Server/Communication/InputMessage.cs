using System;
using BFB.Engine.Math;

namespace BFB.Engine.Server.Communication
{
    [Serializable]
    public class InputMessage : DataMessage
    {
        public BfbVector MousePosition { get; set; }
    }
}