using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGame.Source.Engine;

namespace TestGame.Source.Gameplay.World
{
    public class Projectile2d : Basic2d
    {
        public bool done;
        public float speed;
        public Vector2 direction;
        public Unit owner;
        public MyTimer timer;

        public Projectile2d(string PATH, Vector2 POS, Vector2 DIMS, Unit OWNER, Vector2 TARGET) : base(PATH, POS, DIMS)
        {
            done = false;
            speed = 10.0f;
            owner = OWNER;
            direction = TARGET - owner.pos;
            direction.Normalize();
            timer = new MyTimer(1200);
        }

        public virtual void Update(Vector2 OFFSET, List<Unit> UNITS)
        {
            pos += direction * speed;

            timer.UpdateTimer();
            if (timer.Test())
            {
                done = true;
            }

            if (HitSomething(UNITS))
            {
                // also reduce health of whatever was hit here
                done = true;
            }
        }

        public virtual bool HitSomething(List<Unit> UNITS)
        {
            for (int i = 0; i < UNITS.Count; i++)
            {
                if (pos.X > UNITS[i].pos.X - UNITS[i].dims.X/2 &&
                    pos.X < UNITS[i].pos.X + UNITS[i].dims.X/2 &&
                    pos.Y > UNITS[i].pos.Y - UNITS[i].dims.Y/2 &&
                    pos.Y < UNITS[i].pos.Y + UNITS[i].dims.Y/2)
                {
                    UNITS[i].health -= 5;
                    return true;
                }
            }
            return false;
        }

        public override void Draw(Vector2 OFFSET)
        {
            base.Draw(OFFSET);
        }

    }
}
