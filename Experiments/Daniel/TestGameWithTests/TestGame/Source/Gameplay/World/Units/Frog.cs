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
        private Vector2 velocity;
        public Frog(string PATH, Vector2 POS, Vector2 DIMS) : base(PATH, POS, DIMS)
        {
            velocity = new Vector2(0, 0);
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Draw(Vector2 OFFSET)
        {
            base.Draw(OFFSET);
        }
    }
}
