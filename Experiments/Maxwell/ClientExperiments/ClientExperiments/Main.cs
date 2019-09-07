using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using ClientExperiments.Engine.Scene;
using ClientExperiments.Engine.Event;

using ClientExperiments.Scenes;

namespace ClientExperiments
{
    public class MainGame: Game
    {

        private SceneManager SceneManager;
        private GraphicsDeviceManager GraphicsManager;
        private EventManager EventManager;
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

            //Init Graphics manager. (Needs to be in the constructor)
            GraphicsManager = new GraphicsDeviceManager(this);
        }

        #endregion

        #region Initialize

        protected override void Initialize()
        {
            
            //Init spritebatch for drawing
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            //Init event manager
            EventManager = new EventManager();

            //Init Scene Manager
            SceneManager = new SceneManager(Content, GraphicsManager, EventManager);

            //Add scenes to scene manager
            SceneManager.AddScene(new Scene[] {
                new TestScene2(),
                new TestScene()
            });

            //start first scene
            SceneManager.StartScene(nameof(TestScene));

            //Launch second scene in parallel
            SceneManager.LaunchScene(nameof(TestScene2));

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
            SceneManager.DrawScenes(gameTime, SpriteBatch);

            SpriteBatch.End();

            base.Draw(gameTime);
        }

        #endregion
    }
}