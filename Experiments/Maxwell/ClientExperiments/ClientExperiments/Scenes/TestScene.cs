using ClientExperiments.Engine.Scene;
using ClientExperiments.Engine.Event;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ClientExperiments.Engine.Input;

namespace ClientExperiments.Scenes
{
    public class TestScene : Scene
    {
        private Texture2D grass;
        private Texture2D spaceship;
        private int x;
        private int y;
        //private string message;

        public TestScene() : base(nameof(TestScene)) { }

        public override void Init()
        {
            x = 0;
            y = 0;

            _eventManager.AddEventListener("mousedown", (Event) =>
            {
                x = Event.Mouse.X;
                y = Event.Mouse.Y;
            });

        }

        public override void Load()
        {
            grass = _contentManager.Load<Texture2D>("Sprites\\grass");
            spaceship = _contentManager.Load<Texture2D>("Sprites\\spaceship");
            //font = _contentManager.Load<SpriteFont>("Fonts\\Papyrus");
        }

        public override void Unload()
        {
            //Currently unused
        }

        public override void Update(GameTime gameTime)
        {
            x++;
            y++;

            if(x > 600)
            {
                x = 0;
            }

            if(y > 600)
            {
                y = 0;
            }

        }

        public override void Draw(GameTime gameTime, SpriteBatch graphics)
        {
            graphics.Draw(grass, new Rectangle(0, 0, 800, 500), Color.White);

            graphics.Draw(spaceship, new Vector2(x, y), new Rectangle(0, 0, 100, 100), Color.White, 0, new Vector2(spaceship.Width / 2, spaceship.Height / 2), 1.0f, SpriteEffects.None, 1);

        }



    }
}
