using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGame.Source.Engine;

namespace TestGame.Source.Gameplay.World
{
    public class Background : Basic2d
    {
        public Background(string PATH, Vector2 POS, Vector2 DIMS) : base(PATH, POS, DIMS)
        {

        }

        public override void Update()
        {
            base.Update();
        }

        public void Draw(Vector2 OFFSET, int scroll)
        {
            if (texture != null)
            {
                Rectangle source = new Rectangle(scroll, 0, texture.Width, texture.Height);
                Globals.spriteBatch.Draw(texture, new Rectangle((int)(pos.X + OFFSET.X), (int)(pos.Y + OFFSET.Y), (int)dims.X, (int)dims.Y), source, Color.White, 0.0f, new Vector2(texture.Bounds.Width / 2, texture.Bounds.Height / 2), new SpriteEffects(), 0);
            }
        }
    }
}
