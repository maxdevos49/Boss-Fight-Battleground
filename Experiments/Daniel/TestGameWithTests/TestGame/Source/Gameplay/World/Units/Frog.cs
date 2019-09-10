using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGame.Source.Engine;

namespace TestGame.Source.Gameplay.World
{
    public class Frog : Unit
    {
        public Frog(int hp, Vector2 POS, Vector2 DIMS) : base("2d\\frog", POS, DIMS)
        {

        }

        public override void Update()
        {
            int rnd = Globals.rand.Next(1000);


            if (velocity.Y > 0)
                velocity.Y -= 0.5f;
            if (velocity.Y < 0)
                velocity.Y += 0.5f;
            if (velocity.X > 0 && !isInAir)
                velocity.X -= 0.25f;
            if (velocity.X < 0 && !isInAir)
                velocity.X += 0.25f;

            if (rnd > 980 && !isInAir)
            {
                rnd = Globals.rand.Next(5);
                if (rnd == 0)
                    velocity.X = 8;
                if (rnd == 1)
                    velocity.X = -8;
                if (rnd == 2)
                {
                    isInAir = true;
                    velocity.Y = -18;
                }
                if (rnd == 3)
                {
                    isInAir = true;
                    velocity.Y = -18;
                    velocity.X = 8;
                }
                if (rnd == 4)
                {
                    isInAir = true;
                    velocity.Y = -18;
                    velocity.X = -8;
                }
            }


            pos = new Vector2(velocity.X + pos.X, velocity.Y + pos.Y + Globals.world.gravity.Y);

            if (pos.Y > Globals.screenHeight - dims.Y / 2)
            {
                pos.Y = Globals.screenHeight - dims.Y / 2;
                isInAir = false;
            }
            base.Update();
        }

        public override void Draw(Vector2 OFFSET)
        {
            base.Draw(OFFSET);
        }

    }
}
