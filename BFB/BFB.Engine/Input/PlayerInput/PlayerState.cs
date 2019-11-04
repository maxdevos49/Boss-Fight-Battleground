using System;
using BFB.Engine.Math;

namespace BFB.Engine.Input.PlayerInput
{
    /// <summary>
    /// Holds what state a player is. Is serializable to be sent from the server to the client.
    /// </summary>
    [Serializable]
    public class PlayerState
    {
        public bool Left;
        public bool Right;
        public bool Jump;
        public bool Attack;
        public bool SwitchWeapon;
        public bool LeftClick;
        public bool RightClick;
        public BfbVector Mouse;
        
        public PlayerState()
        {
            Left = false;
            Right = false;
            Jump = false;
            Attack = false;
            SwitchWeapon = false;
            LeftClick = false;
            RightClick = false;
            Mouse = new BfbVector(0,0);

        }
    }
}