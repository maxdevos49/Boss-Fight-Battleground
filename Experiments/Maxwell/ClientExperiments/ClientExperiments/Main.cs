using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

//Engine
using ClientExperiments.Engine.Scene;
using ClientExperiments.Engine.Event;
using ClientExperiments.Engine.Input;

//Project
using ClientExperiments.Scenes;
using Microsoft.Xna.Framework.Input;

namespace ClientExperiments
{
    public class MainGame : Game
    {

        private InputManager InputManager;
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

            //Init input manager
            InputManager = new InputManager(EventManager, new InputConfig());

            //Init Scene Manager
            SceneManager = new SceneManager(Content, GraphicsManager, EventManager);

            //Add scenes to scene manager
            SceneManager.AddScene(new Scene[] {
                new TestScene2(),
                new TestScene(),
                new DebugScene()
            });

            //start first scene
            SceneManager.StartScene(nameof(TestScene));

            //Launch second scene in parallel
            SceneManager.LaunchScene(nameof(TestScene2));

            //global key press event
            EventManager.AddEventListener("keypress", (Event) =>
            {
                //Enable debug
                switch (Event.Keyboard.KeyEnum)
                {
                    case Keys.F3:

                        Console.WriteLine("Launching Debug Scene");

                        if (SceneManager.ActiveSceneExist(nameof(DebugScene)))
                            SceneManager.StopScene(nameof(DebugScene));
                        else
                            SceneManager.LaunchScene(nameof(DebugScene));

                        break;
                    //case Keys.F4:
                    //    GraphicsManager.ToggleFullScreen();
                    //    break;
                }
            });

            base.Initialize();
        }

        #endregion

        #region LoadContent

        protected override void LoadContent()
        {
            //Load global textures here. (Fonts, sprites, and audio)

            //font
            Content.Load<SpriteFont>("Fonts\\Papyrus");
        }

        #endregion

        #region UnloadContent

        protected override void UnloadContent()
        {

            Content.Unload();

            base.UnloadContent();
        }

        #endregion

        #region Update

        protected override void Update(GameTime gameTime)
        {
            //Checks for inputs and then fires events for those inputs
            InputManager.CheckInputs();

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

            //Starts drawing
            SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            //Draw Active Scenes
            SceneManager.DrawScenes(gameTime, SpriteBatch);

            //Ends drawing
            SpriteBatch.End();

            base.Draw(gameTime);
        }

        #endregion
    }
}