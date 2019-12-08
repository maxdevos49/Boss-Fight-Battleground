using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BFB.Client.UI;
using BFB.Engine.Entity;
using BFB.Engine.Event;
using BFB.Engine.Graphics;
using BFB.Engine.Input.PlayerInput;
using BFB.Engine.Scene;
using BFB.Engine.Server;
using BFB.Engine.Server.Communication;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BFB.Client.Scenes
{
    public class GameScene : Scene
    {

        #region Properties
        
        private readonly object _lock;
        
        private readonly PlayerInput _playerInput;

        private readonly WorldRenderer _worldRenderer;

        private ClientDataRegistry _clientData;

        #endregion

        #region Constructor
        
        public GameScene() : base(nameof(GameScene))
        {
            _lock = new object();
            Client = new ClientSocketManager();
            
            _worldRenderer = new WorldRenderer();
            _playerInput = new PlayerInput();
        }

        #endregion
        
        #region Init
        
        protected override void Init()
        {
            
            _clientData = ClientDataRegistry.GetInstance();

            Client.Ip = ClientDataRegistry.Ip;
            Client.Port = ClientDataRegistry.Port;
            
            UIManager.StartLayer(nameof(LoadingGameUI),this);
            
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

                _clientData.World.ApplyWorldInitData((WorldDataMessage)message);
                _worldRenderer.Init(_clientData.World,ContentManager, GraphicsDeviceManager.GraphicsDevice);

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
                UIManager.StartLayer(nameof(LoadingGameUI),this);//TODO Doesnt get called very often but should never be when it is user caused
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
                    _clientData.Entities.Remove(message.Message);
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

                        if (_clientData.Entities.ContainsKey(em.EntityId))
                        {
                            _clientData.Entities[em.EntityId].Position = em.Position;
                            _clientData.Entities[em.EntityId].Velocity = em.Velocity;
                            _clientData.Entities[em.EntityId].Rotation = em.Rotation;
                            _clientData.Entities[em.EntityId].AnimationState = em.AnimationState;
                            _clientData.Entities[em.EntityId].Facing = em.Facing;
                            _clientData.Entities[em.EntityId].Meta = em.Meta;

                            if (em.EntityId == Client.ClientId)
                                _worldRenderer.Camera.Focus = _clientData.Entities[em.EntityId].OriginPosition.ToVector2();
                        }
                        else
                        {
                            _clientData.Entities.Add(em.EntityId, ClientEntity.ClientEntityFactory(em, ContentManager));
                        }
                    }
                }
            });
            
            #endregion

            #region Handle Chunk Updates

            Client.On("/players/chunkUpdates", message =>
            {
                _clientData.World.ApplyChunkUpdateMessage((ChunkUpdatesMessage) message);
            });

            #endregion
            
            #region HandleInventoryUpdates
            
             Client.On("/players/inventoryUpdates", message =>
             {
                 _clientData.Inventory.ApplySlotUpdates((InventorySlotMessage) message);
             });
            
            #endregion

            #region KeyEventListener

            AddInputListener("keypress", e =>
            {
                if (e.Keyboard.KeyEnum == Keys.M)
                    UIManager.StartLayer(nameof(MonsterMenuUI),this);
                else if (e.Keyboard.KeyEnum == Keys.F3)
                    _worldRenderer.Debug = !_worldRenderer.Debug;
            });
            
            #endregion

            #region ScrollEventListener

            AddInputListener("mousescroll", e =>
            {
                if(e.Keyboard.KeyboardState.IsKeyDown(Keys.LeftControl))
                    _worldRenderer.Camera.ApplyZoom(e.Mouse.VerticalScrollAmount/1000f);
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
                _clientData.ClearInstance();
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
                    if (!_clientData.GameReady)
                    {
                        if (!_clientData.Entities.ContainsKey(Client?.ClientId ?? "Nope Key not Found D:"))
                            return;
                        
                        _clientData.GameReady = true;
                        _clientData.Client = _clientData.Entities[Client.ClientId];
                        UIManager.StartLayer(nameof(HudUI), this);
                    }

                if (!_clientData.GameReady)
                    return;
            
                _worldRenderer.Update(gameTime, _clientData.Entities.Values.ToList(), Client.Tps/60);
                
                _playerInput.UpdateHotBar(_clientData.Inventory.ActiveSlot);
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
            
            lock (_lock)
            {
                if (!_clientData.GameReady)
                    return;


                List<ClientEntity> entities = _clientData.Entities.Values.ToList();
                
                //Draw world and entities
                _worldRenderer.Draw(graphics, gameTime, _clientData.World, entities, _clientData.Client,_playerInput.PeekPlayerState(), Client);
            }

        }
        
        #endregion
    }
}
