//C#
using System;

//Engine
using ClientExperiments.Engine.Scene;

//Monogame
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ClientExperiments.Scenes
{
    public class TestScene2 : Scene
    {
        private Texture2D enemy;
        private int x;
        private int y;
        private float r;

        public TestScene2() : base(nameof(TestScene2)) { }

        public override void Init()
        {
            x = 600;
            y = 0;
            r = 0;

            _eventManager.AddEventListener("mousedown", (Event) =>
            {
                x = Event.Mouse.X;
                y = Event.Mouse.Y;

                r += 0.1f;
            });
        }

        public override void Load()
        {
            enemy = _contentManager.Load<Texture2D>("Sprites\\enemy");
        }

        public override void Unload()
        {
            //Currently unused
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(GameTime gameTime, SpriteBatch graphics)
        {
            Random rnd = new Random();

            for (int i = 0; i < 10; i++)
            {
                x = rnd.Next(_graphicsManager.GraphicsDevice.Viewport.Width);
                y = rnd.Next(_graphicsManager.GraphicsDevice.Viewport.Height);

                graphics.Draw(enemy, new Vector2(x, y), new Rectangle(0, 0, 100, 100), Color.White, r, new Vector2(enemy.Width/2, enemy.Height/2), 1.0f, SpriteEffects.None, 1);
            }

        }


    }
}
