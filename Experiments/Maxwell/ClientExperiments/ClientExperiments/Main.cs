using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using ClientExperiments.Engine;

namespace ClientExperiments
{
    public class MainGame: Game
    {

        private SceneManager SceneManager;
        private GraphicsDeviceManager GraphicsManager;
        private SpriteBatch SpriteBatch;
        
        #region Main

        [STAThread]
        static void Main()
        {
            using (var game = new MainGame())
                game.Run();
        }

        #endregion

        #region Constructor

        public MainGame()
        {
            //Monogame Setup
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            //Init Graphics manager
            GraphicsManager = new GraphicsDeviceManager(this);

            //Init Scene Manager
            SceneManager = new SceneManager(Content);

            //Add scenes to scene manager

            //start first scene
            //SceneManager.Start("");


        }

        #endregion

        #region Initialize

        protected override void Initialize()
        {
            //Init spritebatch for drawing
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            base.Initialize();
        }

        #endregion

        #region LoadContent(Unused)

        protected override void LoadContent()
        {
           
        }

        #endregion

        #region Update

        protected override void Update(GameTime gameTime)
        {
            //TODO Input
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //Call update for active scenes
            SceneManager.UpdateScenes(gameTime);

            base.Update(gameTime);
        }

        #endregion

        #region Draw

        protected override void Draw(GameTime gameTime)
        {

            //Clear screen
            GraphicsDevice.Clear(Color.Black);

            //Not sure this 
            SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            //Draw Active Scenes
            SceneManager.DrawScenes(gameTime);

            SpriteBatch.End();

            base.Draw(gameTime);
        }

        #endregion
    }
}