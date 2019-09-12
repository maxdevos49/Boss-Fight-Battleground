using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace test2
{
    class HelloWorld
    {
        public float rot;
        public Vector2 pos, dims, vel;
        public Texture2D texture;
        public bool isInAir;


        public HelloWorld(string PATH, Vector2 POS, Vector2 DIMS)
        {
            pos = POS;
            dims = DIMS;

            isInAir = true;
            vel = new Vector2(5, 5);

            texture = Globals.content.Load<Texture2D>(PATH);
        }

        public virtual void Update()
        {
            pos = new Vector2(pos.X + vel.X, pos.Y + vel.Y);
            if (pos.X < 0)
                vel.X = 5;
            if (pos.X > Globals.screenWidth)
                vel.X = -5;
            if (pos.Y < 0)
                vel.Y = 5;
            if (pos.Y > Globals.screenHeight)
                vel.Y = -5;
        }

        public virtual void Draw(Vector2 OFFSET)
        {
            if (texture != null)
            {
                Globals.spriteBatch.Draw(texture, new Rectangle((int)(pos.X + OFFSET.X), (int)(pos.Y + OFFSET.Y), (int)dims.X, (int)dims.Y), null, Color.White, rot, new Vector2(texture.Bounds.Width / 2, texture.Bounds.Height / 2), new SpriteEffects(), 0.2f);
            }
        }

        public virtual void Draw(Vector2 OFFSET, Vector2 ORIGIN)
        {
            Globals.spriteBatch.Draw(texture, new Rectangle((int)(pos.X + OFFSET.X), (int)(pos.Y + OFFSET.Y), (int)dims.X, (int)dims.Y), null, Color.White, rot, new Vector2(ORIGIN.X, ORIGIN.Y), new SpriteEffects(), 0.2f);
        }
    }
}
