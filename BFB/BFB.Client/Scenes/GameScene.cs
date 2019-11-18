using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BFB.Client.UI;
using BFB.Engine.Content;
using BFB.Engine.Entity;
using BFB.Engine.Event;
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

namespace BFB.Client.Scenes
{
    public class PlayerConnectionScene : Scene
    {

        private readonly object _lock;
        
//        private readonly ClientSocketManager _server;
        
        private PlayerInput _playerInput;

        private WorldRenderer _worldRenderer;

        private readonly WorldManager _world;

        private readonly Dictionary<string, ClientEntity> _entities;

        public PlayerConnectionScene() : base(nameof(PlayerConnectionScene))
        {
            _lock = new object();
            _entities = new Dictionary<string, ClientEntity>();
            Client = new ClientSocketManager();

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
            
            UIManager.Start(nameof(LoadingGameUI));
            
            /**
             * Scene events
             */
            #region Update Input State
            
            _playerInput = new PlayerInput(this);

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
            };
            
            #endregion
            
            #region Client Ready
            
            Client.OnReady = () =>
            {
                GlobalEventManager.Emit("onConnectionStatus", new GlobalEvent("Ready..."));
                _worldRenderer = new WorldRenderer(_world, GraphicsDeviceManager.GraphicsDevice);
                UIManager.Start(nameof(HudUI));
            };
            
            #endregion
            
            #region Client Disconnect
            
            Client.OnDisconnect = (m) =>
            {
                UIManager.Start(nameof(LoadingGameUI));
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
                            
                            if (em.EntityId == Client.ClientId)
                                _worldRenderer.Camera.Focus = em.Position.ToVector2();
                        }
                        else
                        {

                            _entities.Add(em.EntityId, new ClientEntity(em.EntityId,
                                new EntityOptions
                                {
                                    Dimensions = em.Dimensions,
                                    Position = em.Position,
                                    Rotation = em.Rotation,
                                    Origin = em.Origin,
                                }, new ItemGraphicsComponent(ContentManager.GetTexture("tiles")))); // new AnimationComponent(ContentManager.GetAnimatedTexture(em.AnimationTextureKey))));
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
                _worldRenderer?.Update(gameTime, _entities.Values.ToList());
            }

            if (!_playerInput.InputChanged() || !Client.EmitAllowed() || _worldRenderer == null) return;
            
            PlayerState playerState = _playerInput.GetPlayerState();
            playerState.Mouse = _worldRenderer.ViewPointToMapPoint(playerState.Mouse);
            Client.Emit("/player/input", new InputMessage {PlayerInputState = playerState});

        }
        
        #endregion

        #region Draw
        
        public override void Draw(GameTime gameTime, SpriteBatch graphics)
        {
            lock (_lock)
            {
                _worldRenderer?.Draw(graphics, _world, _entities.Values.ToList() , ContentManager);

                if (_worldRenderer != null && _playerInput != null && Client.ClientId != null && _entities.ContainsKey(Client.ClientId))
                {
                    PlayerState playerState = _playerInput.GetPlayerState();
                    playerState.Mouse = _worldRenderer.ViewPointToMapPoint(playerState.Mouse);

                    _worldRenderer?.DebugPanel(graphics, gameTime, _world, _entities[Client.ClientId],
                        _entities.Values.ToList(), Client, playerState, ContentManager);
                }

            }

        }
        
        #endregion
    }
}
