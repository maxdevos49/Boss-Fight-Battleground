using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace testGame
{

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D target_Sprite;
        Texture2D crosshairs_Sprite;
        Texture2D background_Sprite;

        SpriteFont gameFont;

        Vector2 targetPosition = new Vector2(200, 200);
        const int TARGET_RADIUS = 45;
        const int CROSSHAIRS_RADIUS = 25;
        int score = 0;

        MouseState mState;
        bool mReleased = true;
        float mouseTargetDist;
        float timer = 10f;

        //Constructor for the game
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //IsMouseVisible = true; //Lets you see mouse on the game screen

            
        }

        //Starts when the game starts
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        //Load images and sounds, etc.
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            target_Sprite = Content.Load<Texture2D>("target");
            crosshairs_Sprite = Content.Load<Texture2D>("crosshairs");
            background_Sprite = Content.Load<Texture2D>("clouds");

            gameFont = Content.Load<SpriteFont>("galleryFont");

            // TODO: use this.Content to load your game content here
        }

        //To remove assets not present in certain level or game
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        //Game loop, runs every frame
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (timer > 0)
            {
                timer -= (float)gameTime.ElapsedGameTime.TotalSeconds; //automatically will update for time
            }

            mState = Mouse.GetState();
            mouseTargetDist = Vector2.Distance(targetPosition, new Vector2(mState.X, mState.Y));
            
            if (mState.LeftButton == ButtonState.Pressed && mReleased == true){
                if (mouseTargetDist < TARGET_RADIUS && timer > 0){
                    score++;

                    Random rand = new Random();

                    targetPosition.X = rand.Next(TARGET_RADIUS, graphics.PreferredBackBufferWidth - TARGET_RADIUS + 1);
                    targetPosition.Y = rand.Next(TARGET_RADIUS, graphics.PreferredBackBufferHeight - TARGET_RADIUS + 1);
                }
                mReleased = false;
            }
            
            if (mState.LeftButton == ButtonState.Released)
            {

                mReleased = true;
            }

            base.Update(gameTime);
        }

        //Anything that involves drawing images or text to the screen
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            spriteBatch.Draw(background_Sprite, new Vector2(0, 0), Color.White);//background is drawn first to stay in background

            if (timer > 0)
            {
                spriteBatch.Draw(target_Sprite, new Vector2(targetPosition.X - TARGET_RADIUS, targetPosition.Y - TARGET_RADIUS), Color.White);
                spriteBatch.DrawString(gameFont, "Score: " + score.ToString(), new Vector2(3, 40), Color.Black);
                spriteBatch.DrawString(gameFont, "Time Left: " + Math.Ceiling(timer).ToString() + " Seconds", new Vector2(3, 3), Color.Black);
            }

            if (timer < 0)
            {
                spriteBatch.DrawString(gameFont, "GAME OVER", new Vector2(200, 200), Color.Red);
                spriteBatch.DrawString(gameFont, "Score: " + score.ToString(), new Vector2(200, 240), Color.Black);
            }
            
            spriteBatch.Draw(crosshairs_Sprite, new Vector2(mState.X - CROSSHAIRS_RADIUS, mState.Y - CROSSHAIRS_RADIUS), Color.Green);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
