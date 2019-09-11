using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace experimentGame2
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private Texture2D _texture;
        private Vector2 _position;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            _texture = Content.Load<Texture2D>("Box");
            _position = new Vector2(100, 100);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                _position.Y -= 5;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                _position.Y += 5;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                _position.X -= 5;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                _position.X += 5;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            spriteBatch.Begin();

            spriteBatch.Draw(_texture, _position, Color.White);

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