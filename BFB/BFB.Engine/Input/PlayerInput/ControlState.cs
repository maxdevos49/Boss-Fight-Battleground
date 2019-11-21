using System;
using BFB.Engine.Math;

namespace BFB.Engine.Input.PlayerInput
{
    /// <summary>
    /// Holds what state a player is. Is serializable to be sent from the server to the client.
    /// </summary>
    [Serializable]
    public class ControlState
    {
        public bool Left;
        public bool Right;
        public bool Jump;
        public bool HotBarLeft;
        public bool HotBarRight;
        public bool LeftClick;
        public bool RightClick;
        public BfbVector Mouse;
        
        public ControlState()
        {
            Left = false;
            Right = false;
            Jump = false;
            HotBarLeft = false;
            HotBarRight = false;
            LeftClick = false;
            RightClick = false;
            Mouse = new BfbVector(0,0);

        }
        
        public ControlState Clone()
        {
            return new ControlState
            {
                Left = Left,
                Right = Right,
                Jump = Jump,
                HotBarLeft = HotBarLeft,
                HotBarRight = HotBarRight,
                LeftClick = LeftClick,
                RightClick = RightClick,
                Mouse = new BfbVector(Mouse.X,Mouse.Y) 
            };
        }
    }
}