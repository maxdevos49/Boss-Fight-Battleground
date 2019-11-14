using System;
using System.Collections.Generic;
using System.Linq;
using BFB.Client.UI;
using BFB.Engine.Content;
using BFB.Engine.Entity;
using BFB.Engine.Input.PlayerInput;
using BFB.Engine.Math;
using BFB.Engine.Scene;
using BFB.Engine.Server;
using BFB.Engine.Server.Communication;
using BFB.Engine.Simulation.GraphicsComponents;
using BFB.Engine.TileMap;
using BFB.Engine.TileMap.Generators;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BFB.Client.Scenes
{
    public class PlayerConnectionScene : Scene
    {

        private readonly object _lock;
        
        private readonly ClientSocketManager _server;
        
        private PlayerInput _playerInput;

        private WorldRenderer _worldRenderer;

        private readonly WorldManager _world;

        private readonly Dictionary<string, ClientEntity> _entities;

        public PlayerConnectionScene() : base(nameof(PlayerConnectionScene))
        {
            _lock = new object();
            _entities = new Dictionary<string, ClientEntity>();
            _server = new ClientSocketManager("127.0.0.1", 6969);

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

            //TODO Change how the connection is supplied where its started to better handle a server menu style choice
            MainMenuUI layer = (MainMenuUI)UIManager.GetLayer(nameof(MainMenuUI));
            _server.Ip = layer.Model.Ip.Split(":")[0];
            _server.Port = Convert.ToInt32(layer.Model.Ip.Split(":")[1]);
            
            
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

            _server.OnConnect = (m) =>
            {
                //Anything that needs done when this client connects
                Console.WriteLine("Client Connected");
            };
            
            #endregion
            
            #region Client Authentication
            
            _server.OnAuthentication = (m) =>
            {
                //Anything that needs done when this client authenticates.
                Console.WriteLine("Client Authenticating");
                return null;
            };
            
            #endregion

            #region Client Prepare

            _server.OnPrepare = message =>
            {
                _world.ApplyWorldInitData((WorldDataMessage)message);
            };
            
            #endregion
            
            #region Client Ready
            
            _server.OnReady = () =>
            {
                Console.WriteLine("Client Ready!");
                //Do something when client is fully ready after authentication is confirmed
                _worldRenderer = new WorldRenderer(_world, GraphicsDeviceManager.GraphicsDevice);
            };
            
            #endregion
            
            #region Client Disconnect
            
            _server.OnDisconnect = (m) =>
            {
                //Anything that needs done when this client disconnects
                Console.WriteLine("Disconnected");
            };
            
            #endregion
            
            /**
             * Custom Socket Routes
             */

            #region Handle Entity Disconnect
            
            _server.On("/player/disconnect", message =>
            {
                //Remove entity who disconnected
                lock(_lock)
                {
                    _entities.Remove(message.Message);
                }
            });
            
            #endregion
            
            #region Handle Entity Updates
            
            _server.On("/players/updates", message =>
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
                            
                            if (em.EntityId == _server.ClientId)
                                _worldRenderer.Camera.Focus = em.Position.ToVector2();
                        }
                        else
                        {
                            
                            _entities.Add(em.EntityId,new ClientEntity(em.EntityId,
                                new EntityOptions
                                {
                                    Dimensions = em.Dimensions,
                                    Position = em.Position,
                                    Rotation = em.Rotation,
                                    Origin = em.Origin,
                                }, new AnimationComponent(ContentManager.GetAnimatedTexture(em.AnimationTextureKey))));
                        }
                    }
                }
            });
            
            #endregion

            //Launch hud ui
            //UIManager.Start(nameof(HudUI)); //TODO restart the HudUI
            UIManager.Start(nameof(ChatUI));
            
            if (!_server.Connect())
                Console.WriteLine("Connection Failed.");
            #region Handle Chunk Updates

            _server.On("/players/chunkUpdates", message =>
            {
                _world.ApplyChunkUpdateMessage((ChunkUpdatesMessage) message);
            });
            
            #endregion
            
            //Launch hud ui
            UIManager.Start(nameof(HudUI));
            
            if (!_server.Connect())
                Console.WriteLine("Connection Failed.");
            
        }
        
        #endregion

        #region Unload

        protected override void Unload()
        {
            _server.Disconnect("Scene Close");
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

            if (!_playerInput.InputChanged() || !_server.EmitAllowed() || _worldRenderer == null) return;
            
            PlayerState playerState = _playerInput.GetPlayerState();
            playerState.Mouse = _worldRenderer.ViewPointToMapPoint(playerState.Mouse);
            _server.Emit("/player/input", new InputMessage {PlayerInputState = playerState});

        }
        
        #endregion

        #region Draw
        
        public override void Draw(GameTime gameTime, SpriteBatch graphics)
        {
            lock (_lock)
            {
                _worldRenderer?.Draw(graphics, _world, _entities.Values.ToList() , ContentManager);
            }
        }
        
        #endregion
    }
}
