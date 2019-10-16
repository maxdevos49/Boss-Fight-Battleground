using System;
using System.Numerics;
using BFB.Engine.Math;
using Microsoft.Xna.Framework.Input;

namespace BFB.Engine.Entity
{
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