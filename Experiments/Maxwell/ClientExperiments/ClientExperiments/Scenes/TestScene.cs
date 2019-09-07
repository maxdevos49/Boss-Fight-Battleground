using ClientExperiments.Engine.Scene;
using ClientExperiments.Engine.Event;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ClientExperiments.Scenes
{
    public class TestScene : Scene
    {
        private Texture2D grass;
        private Texture2D spaceship;
        private SpriteFont font;
        private int x;
        private int y;
        private int trigger;

        public TestScene() : base(nameof(TestScene)) { }

        public override void Init()
        {
            x = 0;
            y = 0;
            trigger = 0;


            _eventManager.AddEventListener("test-event", (Event) =>
            {
                trigger++;
                Event.StopPropagation();

            });
        }

        public override void Load()
        {
            grass = _contentManager.Load<Texture2D>("Sprites\\grass");
            spaceship = _contentManager.Load<Texture2D>("Sprites\\spaceship");
            font = _contentManager.Load<SpriteFont>("Fonts\\Papyrus");
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
            graphics.Draw(spaceship, new Rectangle(x, y, 100, 100), Color.White);

            graphics.DrawString(font, "Triggers: " + trigger, new Vector2(50, 50), Color.Black);

        }



    }
}
