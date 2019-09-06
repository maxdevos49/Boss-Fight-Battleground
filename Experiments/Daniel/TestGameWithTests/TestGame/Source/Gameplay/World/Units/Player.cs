using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGame.Source.Engine;

namespace TestGame.Source.Gameplay.World
{
    public class Player : Unit
    {
        private Vector2 velocity;
        public Player(string PATH, Vector2 POS, Vector2 DIMS) : base(PATH, POS, DIMS)
        {
            velocity = new Vector2(0, 0);
        }

        public override void Update()
        {
            if (velocity.Y > 0)
                velocity.Y -= 0.5f;
            if (velocity.Y < 0)
                velocity.Y += 0.5f;
            if (velocity.X > 0)
               velocity.X -= 0.25f;
            if (velocity.X < 0)
                velocity.X += 0.25f;


            if (Globals.keyboard.GetPress("W") && !isInAir)
            {
                velocity.Y = -12;
                isInAir = true;
            }
            if (Globals.keyboard.GetPress("S"))
                velocity.Y = 12;
            if (Globals.keyboard.GetPress("A"))
               velocity.X = -4;
            if (Globals.keyboard.GetPress("D"))
                velocity.X = 4;

            pos.X += velocity.X;
            pos.Y += velocity.Y + Globals.world.gravity.Y;

            if (pos.Y < 0 + dims.Y / 2)
                pos.Y = 0 + dims.Y / 2;
            if (pos.Y > Globals.screenHeight - dims.Y / 2)
            {
                pos.Y = Globals.screenHeight - dims.Y / 2;
                isInAir = false;
            }
            if (pos.X < 0 + dims.X / 2)
                pos.X = 0 + dims.X / 2;
            if (pos.X > Globals.screenWidth - dims.X / 2)
                pos.X = Globals.screenWidth - dims.X / 2;

            if(Globals.mouse.LeftClick())
            {
                GameGlobals.PassProjectile(new Fireball(new Vector2(pos.X, pos.Y), this, new Vector2(Globals.mouse.newMousePos.X, Globals.mouse.newMousePos.Y)));
            }
            base.Update();
        }

        public override void Draw(Vector2 OFFSET)
        {
            base.Draw(OFFSET);
        }
    }
}
