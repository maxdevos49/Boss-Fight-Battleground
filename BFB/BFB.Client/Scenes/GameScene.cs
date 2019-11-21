using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BFB.Client.UI;
using BFB.Engine.Content;
using BFB.Engine.Entity;
using BFB.Engine.Event;
using BFB.Engine.Graphics;
using BFB.Engine.Input.PlayerInput;
using BFB.Engine.Math;
using BFB.Engine.Scene;
using BFB.Engine.Server;
using BFB.Engine.Server.Communication;
using BFB.Engine.Simulation.GraphicsComponents;
using BFB.Engine.TileMap;
using BFB.Engine.TileMap.Generators;
using BFB.Engine.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BFB.Client.Scenes
{
    public class PlayerConnectionScene : Scene
    {

        private readonly object _lock;
        
        private readonly PlayerInput _playerInput;

        private readonly WorldRenderer _worldRenderer;

        private readonly WorldManager _world;
        
        private bool _gameReady;

        private ClientEntity _playerEntity;
        
        private readonly Dictionary<string, ClientEntity> _entities;


        public PlayerConnectionScene() : base(nameof(PlayerConnectionScene))
        {
            Client = new ClientSocketManager();
            
            _lock = new object();
            _entities = new Dictionary<string, ClientEntity>();
            _worldRenderer = new WorldRenderer();
            _playerInput = new PlayerInput();

            _gameReady = false;

            _world = new WorldManager(new WorldOptions
            {
                Seed = 1234,
                ChunkSize = 16,
                WorldChunkWidth = 20,
                WorldChunkHeight = 10,
                WorldScale = 15,
                WorldGenerator = options => new RemoteWorld(options)
            });
        }

        
        #region Init
        protected override void Init()
        {

            MainMenuUI layer = (MainMenuUI)UIManager.GetLayer(nameof(MainMenuUI));//TODO This is messy. Change sometime
            Client.Ip = layer.Model.Ip.Split(":")[0];
            Client.Port = Convert.ToInt32(layer.Model.Ip.Split(":")[1]);
            
            UIManager.Start(nameof(LoadingGameUI),this);
            
            /**
             * Scene events
             */
            #region Update Input State
            
            _playerInput.Init(this);

            #endregion

            /**
             * Reserved Socket Manager routes
             */
            #region Client Connect

            Client.OnConnect = (m) =>
            {
                GlobalEventManager.Emit("onConnectionStatus", new GlobalEvent("Server Connected..."));
                Thread.Sleep(100);
            };
            
            #endregion
            
            #region Client Authentication
            
            Client.OnAuthentication = (m) =>
            {
                GlobalEventManager.Emit("onConnectionStatus", new GlobalEvent("Authenticating User..."));
                Thread.Sleep(100);

                return null;
            };
            
            #endregion

            #region Client Prepare

            Client.OnPrepare = message =>
            {
                GlobalEventManager.Emit("onConnectionStatus", new GlobalEvent("Preparing World..."));
                Thread.Sleep(100);

                _world.ApplyWorldInitData((WorldDataMessage)message);
                _worldRenderer.Init(_world,ContentManager, GraphicsDeviceManager.GraphicsDevice);

            };
            
            #endregion
            
            #region Client Ready
            
            Client.OnReady = () =>
            {
                GlobalEventManager.Emit("onConnectionStatus", new GlobalEvent("Ready..."));
            };
            
            #endregion
            
            #region Client Disconnect
            
            Client.OnDisconnect = (m) =>
            {
                UIManager.Start(nameof(LoadingGameUI),this);//TODO Doesnt get called very often but should never be when it is user caused
                GlobalEventManager.Emit("onConnectionStatus", new GlobalEvent("Disconnected By Server"));
            };
            
            #endregion
            
            /**
             * Custom Socket Routes
             */
            #region Handle Entity Disconnect
            
            Client.On("/player/disconnect", message =>
            {
                //Remove entity who disconnected
                lock(_lock)
                {
                    _entities.Remove(message.Message);
                }
            });
            
            #endregion
            
            #region Handle Entity Updates
            
            Client.On("/players/updates", message =>
            {
                EntityUpdateMessage m = (EntityUpdateMessage) message;
                
                lock(_lock)
                {
                    foreach (EntityMessage em in m.Updates)
                    {

                        if (_entities.ContainsKey(em.EntityId))
                        {
                            _entities[em.EntityId].Position = em.Position;
                            _entities[em.EntityId].Velocity = em.Velocity;
                            _entities[em.EntityId].Rotation = em.Rotation;
                            _entities[em.EntityId].AnimationState = em.AnimationState;
                            _entities[em.EntityId].Facing = em.Facing;
                            _entities[em.EntityId].Meta = em.Meta;
                            
                            if (em.EntityId == Client.ClientId)
                                _worldRenderer.Camera.Focus = em.Position.ToVector2();
                        }
                        else
                        {
                            _entities.Add(em.EntityId, ClientEntity.ClientEntityFactory(em, ContentManager));
                        }
                    }
                }
            });
            
            #endregion

            #region Handle Chunk Updates

            Client.On("/players/chunkUpdates", message =>
            {
                _world.ApplyChunkUpdateMessage((ChunkUpdatesMessage) message);
            });

            #endregion

            #region KeyEventListener

            AddInputListener("keypress", e =>
            {
                if (e.Keyboard.KeyEnum == Keys.M)
                    UIManager.Start(nameof(MonsterMenuUI),this);
                else if (e.Keyboard.KeyEnum == Keys.F3)
                    _worldRenderer.Debug = !_worldRenderer.Debug;
            });
            
            #endregion
            
            if (!Client.Connect())
                GlobalEventManager.Emit("onConnectionStatus", new GlobalEvent("Connection Failed"));

        }
        
        #endregion

        #region Unload

        protected override void Unload()
        {
            lock (_lock)
            {
                _entities.Clear();
            }
            
            base.Unload();
        }
        
        #endregion
        
        #region Update

        public override void Update(GameTime gameTime)
        {
            lock (_lock)
            {
                if (Client.EmitAllowed())
                {
                    _gameReady = _entities.ContainsKey(Client?.ClientId ?? "Nope Key not Found D:");

                    if (_gameReady && Client?.ClientId != null)
                    {
                        _playerEntity = _entities[Client.ClientId];
                        UIManager.Start(nameof(HudUI), this);
                    }
                    else
                        return;
                }

                if (!_gameReady)
                    return;
            
                _worldRenderer.Update(gameTime, _entities.Values.ToList());
            }

            if (!_playerInput.InputChanged()) return;
            
            //convert mouse coordinates before we send them to server
            ControlState controlState = _playerInput.GetPlayerState();
            controlState.Mouse = _worldRenderer.ViewPointToMapPoint(controlState.Mouse);
            
            //send input to server
            Client.Emit("/player/input", new InputMessage { ControlInputState = controlState });
        }
        
        #endregion

        #region Draw
        
        public override void Draw(GameTime gameTime, SpriteBatch graphics)
        {
            if (!_gameReady)
                return;
            
            List<ClientEntity> entities;

            lock (_lock)
            {
                entities = _entities.Values.ToList();
            }
            
            //Draw world and entities
            _worldRenderer.Draw(graphics, gameTime, _world, entities, _playerEntity,_playerInput.PeekPlayerState(), Client);
        }
        
        #endregion
    }
}
