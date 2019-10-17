using System;
using System.Collections.Generic;
using BFB.Engine.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using BFB.Engine.Event;
using BFB.Engine.UI;
using JetBrains.Annotations;

namespace BFB.Engine.Scene
{
    public abstract class Scene
    {
        /**
         * Scene manager used to control scene state
         */
        public static SceneManager SceneManager { get; set; }
        
        /**
         * UI Manager for controlling UI related things
         */
        public static UIManager UIManager { get; set; }
        
        /**
         * Used to distribute content across the application without loading things twice and so on
         */
        public static BFBContentManager ContentManager { get; set; }
        
        /**
         * Contains useful drawing stuff
         */
        public static GraphicsDeviceManager GraphicsDeviceManager { get; set; }
        
        /**
         * Used for global events
         */
        public static EventManager<GlobalEvent> GlobalEventManager { get; set; }
        
        /**
         * Used for input events only
         */
        public static EventManager<InputEvent> InputEventManager { get; set; }
        
        
        private readonly List<int> _eventListenerIds;
        
        private SceneStatus _status;
        public readonly string Key;

        protected Scene(string key)
        {
            Key = key;
            _status = SceneStatus.Inactive;
            _eventListenerIds = new List<int>();
        }

        /**
         * Stops the scene by updating the scene status
         * */
        public void Stop()
        {
            //unload textures
            _status = SceneStatus.Inactive;
            Unload();
        }

        /**
         * Pauses the scene by updating the scene status
         * */
        public void Pause()
        {
            _status = SceneStatus.Paused;
        }

        /**
         * Starts the scene by updating the scene status
         * */
        public void Start()
        {
            if (_status == SceneStatus.Inactive)
            {
                _status = SceneStatus.Loading;
                Load();
                Init();
            }
            _status = SceneStatus.Active;
        }

        /**
         * Gets the current status of the scene
         * */
        public SceneStatus GetStatus()
        {
            return _status;
        }

        protected virtual void Init() { }

        protected virtual void Load() { }

        protected virtual void Unload()
        {
            foreach (int id in _eventListenerIds)
                GlobalEventManager.RemoveEventListener(id);
        }

        public virtual void Update([UsedImplicitly] GameTime gameTime) { }

        public virtual void Draw(GameTime gameTime, SpriteBatch graphics) { }

        public void AddEventListener(string eventKey, Action<Event.Event> eventHandler)
        {
            _eventListenerIds.Add(GlobalEventManager.AddEventListener(eventKey, eventHandler));
        }
    }
}
