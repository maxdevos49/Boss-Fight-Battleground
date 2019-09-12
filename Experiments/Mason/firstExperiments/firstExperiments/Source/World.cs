using System;
using firstExperiments.Source.Engine;
using Microsoft.Xna.Framework;

namespace firstExperiments.Source
{
    public class World
    {

        public Basic2d hero;

        public World()
        {
            hero = new Basic2d("2d/Hero", new Vector2(300, 300), new Vector2(48, 48));
        }

        public virtual void Update()
        {
            hero.Update();
        }

        public virtual void Draw()
        {
            hero.Draw();
        }
    }
}
