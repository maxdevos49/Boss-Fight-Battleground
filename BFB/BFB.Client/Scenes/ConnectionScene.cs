﻿using System;
using System.Collections.Generic;
using System.Threading;
using BFB.Engine.Entity;
using BFB.Engine.Entity.Components.Graphics;
using BFB.Engine.Math;
using BFB.Engine.Scene;
using BFB.Engine.Server;
using BFB.Engine.Server.Communication;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BFB.Client.Scenes
{
    public class ConnectionScene : Scene
    {

        private readonly object _lock;
        
        //Temp
        private readonly BfbVector _mouse;
        private Texture2D _spaceshipTexture;

        private readonly ClientSocketManager _server;
        private readonly Dictionary<string, ClientEntity> _entities;

        public ConnectionScene() : base(nameof(ConnectionScene))
        {
            _lock = new object();
            _entities = new Dictionary<string, ClientEntity>();
            _server = new ClientSocketManager("127.0.0.1", 6969);
            _mouse = new BfbVector();
        }

        protected override void Init()
        {
            
            /**
             * Scene events
             */
            #region Update Input State
            
            EventManager.AddEventListener("mousemove", (e) =>
            {
                lock (_lock)
                {
                    _mouse.X = e.Mouse.X;
                    _mouse.Y = e.Mouse.Y;
                }
            });
            
            #endregion

            /**
             * Reserved Socket Manager routes
             */
            #region Client Connect

            _server.OnConnect((m) =>
            {
                //Anything that needs done when this client connects
                Console.WriteLine("Client Connected");
            });
            
            #endregion
            
            #region Client Authentication
            
            _server.OnAuthentication((m) =>
            {
                //Anything that needs done when this client authenticates.
                Console.WriteLine("Client Authenticating");
                return null;
            });
            
            #endregion

            #region Client Ready
            
            _server.OnReady(() =>
            {
                Console.WriteLine("Client Ready!");
                //Do something when client is fully ready after authentication is confirmed
            });
            
            #endregion
            
            #region Client Disconnect
            
            _server.OnDisconnect((m) =>
            {
                //Anything that needs done when this client disconnects
                Console.WriteLine("Disconnected");
            });
            
            #endregion
            
            /**
             * Custom Socket Routes
             */
            
            #region SendInput
            
            _server.On("/players/getUpdates", (m) =>
            {
                _server.Emit("/player/input", new InputMessage {MousePosition = _mouse});
            });
            
            #endregion
            
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
                        }
                        else
                        {
                            _entities.Add(em.EntityId,new ClientEntity(em.EntityId,
                                new EntityOptions
                                {
                                    Dimensions = em.Dimensions,
                                    Position = em.Position,
                                    Rotation = em.Rotation,
                                    Origin = em.Origin
                                }, new AnimationComponent(_spaceshipTexture)));
                        }
                    }
                }
            });
            
            #endregion

            if (!_server.Connect())
                Console.WriteLine("Connection Failed.");

        }

        #region Load
        
        protected override void Load()
        {
            _spaceshipTexture = ContentManager.Load<Texture2D>("Sprites\\SpaceshipSpritesheet");
        }
        
        #endregion

        #region Update
        
        public override void Update(GameTime gameTime)
        {
            lock (_lock)
            {
                foreach ((string key, ClientEntity entity) in _entities)
                {
                    entity.Update();
                }
            }
        }
        
        #endregion

        #region Draw
        
        public override void Draw(GameTime gameTime, SpriteBatch graphics)
        {
            lock (_lock)
            {
                foreach ((string key, ClientEntity entity) in _entities)
                {
                    entity.Draw(graphics);
                }
            }

        }
        
        #endregion
    }
}