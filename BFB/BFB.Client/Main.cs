//c#
using System;

//Monogame
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

//Engine
using BFB.Engine.Scene;
using BFB.Engine.Event;
using BFB.Engine.Input;

//Project
using BFB.Client.Scenes;

using Microsoft.Xna.Framework.Input;

namespace BFB
{
    public class MainGame : Game
    {

        private InputManager InputManager;
        private SceneManager SceneManager;
        private GraphicsDeviceManager GraphicsManager;//Kinda ok name but will probably change later
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
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            EventManager = new EventManager();
            InputManager = new InputManager(EventManager, new InputConfig());
            SceneManager = new SceneManager(Content, GraphicsManager, EventManager);

            //Add scenes to scene manager
            SceneManager.AddScene(new Scene[] {
                new DebugScene(),
                new ExampleScene()
            });

            //start first scene
            SceneManager.StartScene(nameof(ExampleScene));

            //global key press events
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
            //global font load
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
            //Process the events in the queue
            EventManager.ProcessEvents();
            
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

            //Clear screens
            GraphicsDevice.Clear(Color.White);

            //Starts drawing buffer
            SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            //Draw Active Scenes
            SceneManager.DrawScenes(gameTime, SpriteBatch);

            //draws graohics buffer
            SpriteBatch.End();

            base.Draw(gameTime);
        }

        #endregion
    }
}