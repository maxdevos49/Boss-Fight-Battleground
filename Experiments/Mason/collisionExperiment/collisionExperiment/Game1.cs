using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using collisionExperiment.Models;
using collisionExperiment.Sprites;

namespace collisionExperiment
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private List<Sprite> _sprites;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            var playerTexture = Content.Load<Texture2D>("Soldier");

            _sprites = new List<Sprite>()
      {
        new Player(playerTexture)
        {
          Input = new Input()
          {
            Left = Keys.A,
            Right = Keys.D,
            Up = Keys.W,
            Down = Keys.S,
          },
          Position = new Vector2(100, 100),
          Color = Color.LightGreen,
          Speed = 5,
        },
        new Player(playerTexture)
        {
          Input = new Input()
          {
            Left = Keys.Left,
            Right = Keys.Right,
            Up = Keys.Up,
            Down = Keys.Down,
          },
          Position = new Vector2(300, 100),
          Color = Color.LightCoral,
          Speed = 5,
        },
      };
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            foreach (var sprite in _sprites)
                sprite.Update(gameTime, _sprites);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            foreach (var sprite in _sprites)
                sprite.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        static void Main()
        {
            using (var game = new Game1())
                game.Run();
        }
    }
}