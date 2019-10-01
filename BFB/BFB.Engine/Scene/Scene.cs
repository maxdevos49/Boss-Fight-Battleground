using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using BFB.Engine.Event;

namespace BFB.Engine.Scene
{
    public abstract class Scene
    {

        public SceneManager _sceneManager;
        public ContentManager _contentManager;
        public GraphicsDeviceManager _graphicsManager;
        public EventManager _eventManager;

        private List<int> eventListenerIds;
        
        //TODO State Manager

        private SceneStatus Status;
        public readonly string Key;

        protected Scene(string key)
        {
            Key = key;
            Status = SceneStatus.INOPERABLE;
            eventListenerIds = new List<int>();
        }

        //Inject the scene dependencies
        public void InjectDependencies(SceneManager sceneManager, ContentManager contentManager, GraphicsDeviceManager graphicsManager, EventManager eventManager)
        {
            _sceneManager = sceneManager;
            _contentManager = contentManager;
            _graphicsManager = graphicsManager;
            _eventManager = eventManager;

            //Indicate the scene is now in a operable state but inactive
            Status = SceneStatus.INACTIVE;
        }

        /**
         * Stops the scene by updating the scene status
         * */
        public void Stop()
        {
            //unload textures
            Status = SceneStatus.INACTIVE;
            Unload();
        }

        /**
         * Pauses the scene by updating the scene status
         * */
        public void Pause()
        {
            Status = SceneStatus.PAUSED;
        }

        /**
         * Starts the scene by updating the scene status
         * */
        public void Start()
        {
            if (Status == SceneStatus.INACTIVE)
            {
                Status = SceneStatus.LOADING;
                Load();
                Init();
            }
            Status = SceneStatus.ACTIVE;
        }

        /**
         * Gets the current status of the scene
         * */
        public SceneStatus GetStatus()
        {
            return Status;
        }

        public virtual void Init() { }

        public virtual void Load() { }

        public virtual void Unload()
        {
            foreach (int id in eventListenerIds)
                _eventManager.RemoveEventListener(id);
        }

        public virtual void Update(GameTime gameTime) { }

        public virtual void Draw(GameTime gameTime, SpriteBatch graphics) { }

        public void AddEventListener(string eventKey, Action<Event.Event> eventHandler)
        {
            eventListenerIds.Add(_eventManager.AddEventListener(eventKey, eventHandler));
        }
    }
}
