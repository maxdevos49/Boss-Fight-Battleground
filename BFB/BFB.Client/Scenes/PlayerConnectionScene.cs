using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Threading;
using BFB.Client.UI;
using BFB.Engine.Content;
using BFB.Engine.Entity;
using BFB.Engine.Entity.GraphicsComponents;
using BFB.Engine.Input.PlayerInput;
using BFB.Engine.Math;
using BFB.Engine.Scene;
using BFB.Engine.Server;
using BFB.Engine.Server.Communication;
using BFB.Engine.TileMap;
using BFB.Engine.TileMap.Generators;
using BFB.Engine.UI.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BFB.Client.Scenes
{
    public class PlayerConnectionScene : Scene
    {

        private readonly object _lock;
        
        private PlayerInput _playerInput;

        private readonly ClientSocketManager _server;
        private readonly Dictionary<string, ClientEntity> _entities;
        private readonly Camera2D _camera2;
        private readonly WorldManager _world;


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
                GetWorldGenerator = options => new RemoteWorld(options)
            });
            
            _camera2 = new Camera2D(_world,GraphicsDeviceManager.GraphicsDevice);

            
        }

        
        #region Init
        protected override void Init()
        {

            //TODO Change how the connection is supplied where its started to better handle a server menu style choice
            
            MainMenuUI layer = (MainMenuUI)UIManager.GetLayer(nameof(MainMenuUI));
            _server.Ip = layer.model.Ip.Split(":")[0];
            _server.Port = Convert.ToInt32(layer.model.Ip.Split(":")[1]);
            
            
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

            #region Client Ready
            
            _server.OnReady = () =>
            {
                Console.WriteLine("Client Ready!");
                //Do something when client is fully ready after authentication is confirmed
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

            #region Handle Player Disconnect
            
            _server.On("/player/disconnect", message =>
            {
                //Remove player who disconnected
                lock(_lock)
                {
                    _entities.Remove(message.Message);
                }
            });
            
            #endregion
            
            #region Handle player Updates
            
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
                            {
                                _camera2.Focus = em.Position.ToVector2();
                            }
                        }
                        else
                        {
                            Console.WriteLine("Test: " + em.AnimationTextureKey);
                            
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

            #region Handle Chunk Updates

            //TODO Chunk data does not seem to actually be sent. Initialize entire chunk map?
            
            _server.On("/players/chunkUpdates", message =>
            {
                ChunkUpdatesMessage m = (ChunkUpdatesMessage) message;

                //Process full chunk updates
                foreach (ChunkUpdate chunkUpdate in m.ChunkUpdates)
                {
                    Console.WriteLine("New Chunk Loaded: " + chunkUpdate.ChunkKey);
                    _world.ChunkMap[chunkUpdate.ChunkX, chunkUpdate.ChunkY].ChunkKey = chunkUpdate.ChunkKey;
                    _world.ChunkMap[chunkUpdate.ChunkX, chunkUpdate.ChunkY].Block = chunkUpdate.Block;
                    _world.ChunkMap[chunkUpdate.ChunkX, chunkUpdate.ChunkY].Wall = chunkUpdate.Wall;

                }
                
                //process tile map updates
                foreach (ChunkTileUpdates chunkTileUpdates in m.ChunkTileUpdates)
                {
                    Console.WriteLine("ChunkTileUpdates loaded: " + chunkTileUpdates.ChunkKey);

                    Chunk chunk = _world.ChunkMap[chunkTileUpdates.ChunkX, chunkTileUpdates.ChunkY];

                    foreach (TileUpdate chunkTileUpdate in chunkTileUpdates.TileChanges)
                    {
                        chunk.ApplyBlockUpdate(chunkTileUpdate, true);
                    }
                }
            });
            
            #endregion
            
            //Launch hud ui
            UIManager.Start(nameof(HudUI));
            
            if (!_server.Connect())
                Console.WriteLine("Connection Failed.");
            
            _world.GenerateWorld();
        }
        
        #endregion

        #region Load
        
        protected override void Load()
        {
        }
        
        #endregion

        #region Unload

        protected override void Unload()
        {
            lock (_lock)
            {
                _entities.Clear();
            }
            _server.Disconnect("Scene Close");
            base.Unload();
        }
        
        #endregion
        
        #region Update
        
        public override void Update(GameTime gameTime)
        {
            lock (_lock)
            {
                //Interpolation
                foreach ((string _, ClientEntity entity) in _entities)
                    entity.Update();
            }
            
            if(_playerInput.InputChanged())
                _server.Emit("/player/input", new InputMessage {PlayerInputState = _playerInput.GetPlayerState()});
            
            _camera2.Update(gameTime);
        }
        
        #endregion

        #region Draw
        
        public override void Draw(GameTime gameTime, SpriteBatch graphics)
        {
            
            //TODO make a client world renderer that houses information including the camera, world options, and the tile map
            graphics.End();
            graphics.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, _camera2.Transform);
            
            const int scale = 15;
            
            for(int y = 0; y < _world.WorldOptions.WorldChunkHeight * _world.WorldOptions.ChunkSize; y++)
            {
                for (int x = 0; x < _world.WorldOptions.WorldChunkWidth * _world.WorldOptions.ChunkSize; x++)
                {
                    switch(_world.GetBlock(x, y))
                    {
                        case WorldTile.Grass:
                            if(_camera2.IsInView(new Vector2(x * scale, y * scale),ContentManager.GetTexture("grass")))
                                graphics.Draw(ContentManager.GetTexture("grass"), new Vector2(x * scale, y * scale), Color.White);
                            break;
                        case WorldTile.Dirt:
                            if(_camera2.IsInView(new Vector2(x * scale, y * scale),ContentManager.GetTexture("dirt")))
                                graphics.Draw(ContentManager.GetTexture("dirt"), new Vector2(x * scale, y * scale), Color.White);
                            break;
                        case WorldTile.Stone:
                            if(_camera2.IsInView(new Vector2(x * scale, y * scale),ContentManager.GetTexture("stone")))
                                graphics.Draw(ContentManager.GetTexture("stone"), new Vector2(x * scale, y * scale), Color.White);
                            break;
                    }
                }
            }
            
            lock (_lock)
            {
                foreach ((string _, ClientEntity entity) in _entities)
                    entity.Draw(graphics);
            }
            
            graphics.End();
            graphics.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
            
            
        }
        
        #endregion
    }
}
