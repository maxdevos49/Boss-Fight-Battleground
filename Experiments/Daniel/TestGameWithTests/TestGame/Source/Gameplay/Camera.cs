using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestGame.Source.Engine;

namespace TestGame.Source.Gameplay
{
    public class Camera
    {
        public Matrix Transform { get; private set; }

        public void Follow(Basic2d sprite)
        {
            Transform = Matrix.CreateTranslation(
                -sprite.pos.X - (sprite.dims.X / 2),
                /*-sprite.pos.Y - (sprite.dims.Y / 2)*/ 0,
                0) * 
                Matrix.CreateTranslation(
                    Globals.screenWidth / 2, 
                    /*Globals.screenHeight / 2*/ 0, 
                    0);
        }
    }
}
