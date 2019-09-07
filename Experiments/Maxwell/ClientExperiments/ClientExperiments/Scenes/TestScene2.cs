using ClientExperiments.Engine.Event;
using ClientExperiments.Engine.Scene;

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

            _eventManager.AddEventListener("test-event", (Event) =>
            {
                r+=0.1f;

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
            x--;
            y++;

            if (x < 0)
                x = 600;

            if (y > 600)
                y = 0;

            _eventManager.Emit("test-event");
        }

        public override void Draw(GameTime gameTime, SpriteBatch graphics)
        {
            graphics.Draw(enemy, new Vector2(x,y), new Rectangle(0,0,100,100), Color.White, r, new Vector2(0,0), 1.0f, SpriteEffects.None, 1);
        }


    }
}
