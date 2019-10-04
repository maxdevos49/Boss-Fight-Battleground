using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using BFB.Engine.Event;
using JetBrains.Annotations;

namespace BFB.Engine.Scene
{
    public abstract class Scene
    {
        protected SceneManager SceneManager;
        protected ContentManager ContentManager;
        protected GraphicsDeviceManager GraphicsDeviceManager;
        protected EventManager EventManager;
        private readonly List<int> _eventListenerIds;
        
        private SceneStatus _status;
        public readonly string Key;

        protected Scene(string key)
        {
            Key = key;
            _status = SceneStatus.Inoperable;
            _eventListenerIds = new List<int>();
        }

        //Inject the scene dependencies
        public void InjectDependencies(SceneManager sceneManager, ContentManager contentManager, GraphicsDeviceManager graphicsManager, EventManager eventManager)
        {
            SceneManager = sceneManager;
            ContentManager = contentManager;
            GraphicsDeviceManager = graphicsManager;
            EventManager = eventManager;

            //Indicate the scene is now in a operable state but inactive
            _status = SceneStatus.Inactive;
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

        public virtual void Unload()
        {
            foreach (int id in _eventListenerIds)
                EventManager.RemoveEventListener(id);
        }

        public virtual void Update([UsedImplicitly] GameTime gameTime) { }

        public virtual void Draw(GameTime gameTime, SpriteBatch graphics) { }

        public void AddEventListener(string eventKey, Action<Event.Event> eventHandler)
        {
            _eventListenerIds.Add(EventManager.AddEventListener(eventKey, eventHandler));
        }
    }
}
