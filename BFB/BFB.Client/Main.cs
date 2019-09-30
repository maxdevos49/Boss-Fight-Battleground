﻿//c#
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
using BFB.Engine.Server;
using BFB.Engine.Server.Communication;
using Microsoft.Xna.Framework.Input;

namespace BFB
{
    public class MainGame : Game
    {

        private InputManager _inputManager;
        private SceneManager _sceneManager;
        private readonly GraphicsDeviceManager _graphicsManager;//Kinda ok name but will probably change later
        private EventManager _eventManager;

        private SpriteBatch _spriteBatch;
        
        #region Main

        [STAThread]
        static void Main()
        {
            using (MainGame game = new MainGame())
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
            _graphicsManager = new GraphicsDeviceManager(this);
        }

        #endregion

        #region Initialize

        protected override void Initialize()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _eventManager = new EventManager();
            _inputManager = new InputManager(_eventManager, new InputConfig());
            _sceneManager = new SceneManager(Content, _graphicsManager, _eventManager);

            //Add scenes to scene manager
            _sceneManager.AddScene(new Scene[] {
                new DebugScene(),
                new ExampleScene(),
                new ConnectionScene()
            });

            //start first scene
            _sceneManager.StartScene(nameof(ConnectionScene));

            //global key press events
            _eventManager.AddEventListener("keypress", (Event) =>
            {
                //Enable debug
                switch (Event.Keyboard.KeyEnum)
                {
                    case Keys.F3:

                        Console.WriteLine("Launching Debug Scene");

                        if (_sceneManager.ActiveSceneExist(nameof(DebugScene)))
                            _sceneManager.StopScene(nameof(DebugScene));
                        else
                            _sceneManager.LaunchScene(nameof(DebugScene));
                        break;
//                        case Keys.F4:
//                            _clientSocketManager?.Disconnect();
//                            _clientSocketManager = new ClientSocketManager("127.0.0.1", 6969);
//
//                            bool ponged = false;
//            
//                            _clientSocketManager.On("ping", message =>
//                            {
//                                _clientSocketManager.Emit("pong", new DataMessage{Message = "Pong pong pong"});
//                                ponged = true;
//                            });
//
//                            while (true)
//                            {
//                                if (!ponged) continue;
//                                
//                                Console.WriteLine("Ping Received!!");
//                                _clientSocketManager.Disconnect();
//                                break;
//                            }
//                            break;
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
            //Checks for inputs and then fires events for those inputs
            _inputManager.CheckInputs();

            //Call update for active scenes
            _sceneManager.UpdateScenes(gameTime);

            base.Update(gameTime);
        }

        #endregion

        #region Draw

        protected override void Draw(GameTime gameTime)
        {

            //Clear screens
            GraphicsDevice.Clear(Color.White);

            //Starts drawing buffer
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            //Draw Active Scenes
            _sceneManager.DrawScenes(gameTime, _spriteBatch);

            //draws graohics buffer
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        #endregion
    }
}