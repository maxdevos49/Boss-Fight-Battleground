using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace testGame2
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D background_Sprite;
        Texture2D player_Sprite;

        const int PLAYER_X_DIAMETER = 50;
        Vector2 playerPosition = new Vector2(300, 364);

        KeyboardState keyIn;

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

            background_Sprite = Content.Load<Texture2D>("background");
            player_Sprite = Content.Load<Texture2D>("player");
        }


        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }


        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            keyIn = Keyboard.GetState();

            if (keyIn.IsKeyDown(Keys.Left) || keyIn.IsKeyDown(Keys.A))
            {
                if(playerPosition.X > 0)
                {
                    playerPosition.X -= 10;
                }
            }

            if (keyIn.IsKeyDown(Keys.Right) || keyIn.IsKeyDown(Keys.D))
            {
                if (playerPosition.X < graphics.PreferredBackBufferWidth - PLAYER_X_DIAMETER)
                {
                    playerPosition.X += 10;
                }
            }


            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            spriteBatch.Draw(background_Sprite, new Vector2(0, 0), Color.White);

            spriteBatch.Draw(player_Sprite, new Vector2(playerPosition.X, playerPosition.Y), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
