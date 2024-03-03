﻿using System;
using BFB.Client.Helpers;
using BFB.Client.Scenes;
using BFB.Client.UI;
using BFB.Engine.Content;
using BFB.Engine.Event;
using BFB.Engine.Input;
using BFB.Engine.Scene;
using BFB.Engine.UI;
using BFB.Engine.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BFB.Client
{
    public class MainGame : Game
    {
        #region Properties

        private readonly GraphicsDeviceManager _graphicsDeviceManager;
        private InputManager _inputManager;
        private SceneManager _sceneManager;
        private UIManager _uiManager;

        private EventManager<GlobalEvent> _globalEventManager;
        private EventManager<InputEvent> _inputEventManager;

        private BFBContentManager _contentManager;
        private AudioManager _audioManager; 

        private SpriteBatch _spriteBatch;
        private bool _windowSizeIsBeingChanged;

        #endregion

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
            _graphicsDeviceManager = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 900, 
                PreferredBackBufferHeight = 500
            };

            _graphicsDeviceManager.PreparingDeviceSettings += (sender, e) =>
            {
                //Enables vsync
                e.GraphicsDeviceInformation.PresentationParameters.PresentationInterval = PresentInterval.Two;
            };

            _windowSizeIsBeingChanged = false;
        }

        #endregion

        #region Initialize

        protected override void Initialize()
        {
            #region Window Options

            Window.Title = "Boss Fight Battlegrounds";
            // On M1 Mac self installed SDL does not support this. Commenting for now
            // Window.ClientSizeChanged += Window_ClientSizeChanged;
            // Window.AllowUserResizing = true;

            #endregion

            #region Init Managers

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _globalEventManager = new EventManager<GlobalEvent>();
            _inputEventManager = new EventManager<InputEvent>();

            _inputManager = new InputManager(_inputEventManager);
            _contentManager = new BFBContentManager(Content, GraphicsDevice);
            _audioManager = new AudioManager(_contentManager);
            _uiManager = new UIManager(_graphicsDeviceManager.GraphicsDevice, _contentManager);
            _sceneManager = new SceneManager(Content, _graphicsDeviceManager, _globalEventManager, _uiManager);

            //Map Dependencies on scenes
            Scene.SceneManager = _sceneManager;
            Scene.AudioManager = _audioManager; 
            Scene.UIManager = _uiManager;
            Scene.ContentManager = _contentManager;
            Scene.GraphicsDeviceManager = _graphicsDeviceManager;
            Scene.GlobalEventManager = _globalEventManager;
            Scene.InputEventManager = _inputEventManager;

            //Map dependencies on UILayers
            UILayer.SceneManager = _sceneManager;
            UILayer.UIManager = _uiManager;
            UILayer.GlobalEventManager = _globalEventManager;

            //map dependencies to the UIComponent
            UIComponent.UIManager = _uiManager;

            //catch input events
            _inputEventManager.OnEventProcess = _uiManager.ProcessEvents;

            #endregion

            #region Register Scenes/Start Main Scene

            //Register a scene here
            _sceneManager.AddScene(new Scene[]
            {
                new MainMenuScene(),
                new GameScene(),
            });

            #endregion

            #region Register UILayers

            _uiManager.AddUILayer(new UILayer[]
            {
                new LoginUI(),
                new MainMenuUI(),
                new ServerMenuUI(),
                new AddServerUI(),
                new EditServerListUI(),
                new SettingsMenuUI(),
                new HelpUI(),
                new HudUI(),
                new GameMenuUI(),
                new MonsterMenuUI(),
                new ChatUI(),
                new StoreUI(),
                new CreditCardUI(), 
                new CompletedTransactionUI(), 
                new LoadingGameUI(), 
                new InventoryUI(), 
                new CountdownUI(),
                new GameOverUI(), 
                new SoundSettingsUI(), 
                new ControlUI()
            });
            
            #endregion
            
            base.Initialize();
        }

        #endregion

        #region LoadContent

        protected override void LoadContent()
        {
            ClientDataRegistry.Settings =  ClientSettings.GetSettings();
            
            //Parses content data from file named "content.json"
            _contentManager.ParseContent(ClientDataRegistry.Settings.DevelopmentMode);

            //Global texture load
            Texture2D defaultTexture = new Texture2D(_graphicsDeviceManager.GraphicsDevice, 1, 1);
            defaultTexture.SetData(new[] {Color.White});
            _contentManager.AddTexture("default", defaultTexture);

            AtlasTexture defaultAtlas = new AtlasTexture
            {
                Height = 1,
                Width = 1,
                Scale = 1,
                Texture = defaultTexture,
                TextureKey = "default",
                X = 0,
                Y = 0
            };
            _contentManager.AddAtlasTexture("default", defaultAtlas);

            //start first scene
            _sceneManager.StartScene(nameof(MainMenuScene));
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
            _globalEventManager.ProcessEvents();
            _inputEventManager.ProcessEvents();

            //Call update for active scenes and uiLayers
            _sceneManager.UpdateScenes(gameTime);
            _uiManager.UpdateLayers(gameTime);

            base.Update(gameTime);
        }

        #endregion

        #region Draw

        protected override void Draw(GameTime gameTime)
        {
            //Clear screens
            GraphicsDevice.Clear(Color.LightBlue);

            //Starts drawing buffer
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp,
                DepthStencilState.None, RasterizerState.CullCounterClockwise);

            //Draw Active Scenes
            _sceneManager.DrawScenes(gameTime, _spriteBatch);

            _uiManager.Draw(_spriteBatch);

            //draws graphics buffer
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        #endregion

        #region Window Resizing

        private void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            _windowSizeIsBeingChanged = !_windowSizeIsBeingChanged;
            _uiManager.WindowResize();

            if (!_windowSizeIsBeingChanged) return;

            _graphicsDeviceManager.PreferredBackBufferWidth = Window.ClientBounds.Width;
            _graphicsDeviceManager.PreferredBackBufferHeight = Window.ClientBounds.Height;

            _graphicsDeviceManager.ApplyChanges();
        }

        #endregion
    }
}