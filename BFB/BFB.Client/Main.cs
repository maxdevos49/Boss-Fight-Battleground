using System;
using BFB.Client.Scenes;
using BFB.Engine.Event;
using BFB.Engine.Input;
using BFB.Engine.Scene;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BFB.Client
{
    public class MainGame : Game
    {

        private InputManager _inputManager;
        private SceneManager _sceneManager;
        private readonly GraphicsDeviceManager _graphicsManager;//Kinda ok name but will probably change later to graphics device manager
        private EventManager _eventManager;
        private SpriteBatch _spriteBatch;
        private bool _windowSizeIsBeingChanged;

        
        #region Main

        [STAThread]
        private static void Main()
        {
            using (MainGame game = new MainGame())
                game.Run();
        }

        #endregion

        #region Constructor

        private MainGame()
        {
            //Monogame Setup
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            //Init Graphics manager. (Needs to be in the constructor)
            _graphicsManager = new GraphicsDeviceManager(this);
            _windowSizeIsBeingChanged = false;
        }

        #endregion

        #region Initialize

        protected override void Initialize()
        {
            #region Window Options
            
            Window.Title = "Boss Fight Battlegrounds";
            Window.ClientSizeChanged += new EventHandler<EventArgs>(Window_ClientSizeChanged);
            Window.AllowUserResizing = true;
            
            #endregion
            
            #region Init Managers
            
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _eventManager = new EventManager();
            _inputManager = new InputManager(_eventManager, new InputConfig());
            _sceneManager = new SceneManager(Content, _graphicsManager, _eventManager);

            #endregion
            
            #region Register Scenes/Start Main Scene
            
            //Register a scene here
            _sceneManager.AddScene(new Scene[] {
                new DebugScene(),
                new ExampleScene(),
                new TileMapTestScene(),
                new ConnectionScene(),
                new MenuScene(),
            });

            //start first scene
            _sceneManager.StartScene(nameof(MenuScene));
            
            #endregion

            #region Global Keypress Event Registration
            
            _eventManager.AddEventListener("keypress", (Event) =>
            {
                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (Event.Keyboard.KeyEnum)
                {
                    case Keys.F3:
                        
                        //Toggle debug scene
                        if (_sceneManager.ActiveSceneExist(nameof(DebugScene)))
                            _sceneManager.StopScene(nameof(DebugScene));
                        else
                            _sceneManager.LaunchScene(nameof(DebugScene));
                        
                        break;
                    case Keys.M:
                        
                        //Return to main menu
                        _sceneManager.StartScene(nameof(MenuScene));
                            
                        break;
                }
            });

            #endregion
            
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
            
            //Process the events in the queue
            _eventManager.ProcessEvents();

            //Call update for active scenes
            _sceneManager.UpdateScenes(gameTime);

            base.Update(gameTime);
        }

        #endregion

        #region Draw

        protected override void Draw(GameTime gameTime)
        {

            //Clear screens
            GraphicsDevice.Clear(Color.LightBlue);

            //Starts drawing buffer
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);

            //Draw Active Scenes
            _sceneManager.DrawScenes(gameTime, _spriteBatch);

            //draws graphics buffer
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        #endregion
        
        #region Window Resizing

        private void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            _windowSizeIsBeingChanged = !_windowSizeIsBeingChanged;
            
            if (!_windowSizeIsBeingChanged) return;
            _graphicsManager.PreferredBackBufferWidth = Window.ClientBounds.Width;
            _graphicsManager.PreferredBackBufferHeight = Window.ClientBounds.Height;
            _graphicsManager.ApplyChanges();

            _eventManager.Emit("window-resize");
        }
        
        #endregion
    }
}