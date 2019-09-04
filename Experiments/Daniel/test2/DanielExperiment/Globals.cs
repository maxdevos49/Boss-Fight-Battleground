using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test2
{
    public delegate void PassObject(object i);
    public delegate object PassObjectAndReturn(object i);

    public class Globals
    {
        public static int screenHeight, screenWidth;

        public static ContentManager content;
        public static SpriteBatch spriteBatch;

        public static GameTime gameTime;
    }
}
