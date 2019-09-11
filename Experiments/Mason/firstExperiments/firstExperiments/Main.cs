using firstExperiments.Source;
using firstExperiments.Source.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace firstExperiments
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;

        SpriteBatch spriteBatch;

        World world;

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
            Globals.content = this.Content;
            Globals.spriteBatch = new SpriteBatch(GraphicsDevice);

            world = new World();
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            KeyboardState keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.W) || keyState.IsKeyDown(Keys.Up))
                world.hero.UpdatePosRelative(0, -5);
            else if (keyState.IsKeyDown(Keys.A) || keyState.IsKeyDown(Keys.Left))
                world.hero.UpdatePosRelative(-5, 0);
            else if (keyState.IsKeyDown(Keys.S) || keyState.IsKeyDown(Keys.Down))
                world.hero.UpdatePosRelative(0, 5);
            else if (keyState.IsKeyDown(Keys.D) || keyState.IsKeyDown(Keys.Right))
                world.hero.UpdatePosRelative(5, 0);

            world.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            Globals.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            world.Draw();

            Globals.spriteBatch.End();

            base.Draw(gameTime);
        }

        static void Main()
        {
            using (var game = new Game1())
                game.Run();
        }
    }


}