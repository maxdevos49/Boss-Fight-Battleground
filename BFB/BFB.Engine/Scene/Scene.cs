using System;
using System.Collections.Generic;
using BFB.Engine.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using BFB.Engine.Event;
using BFB.Engine.Server;
using BFB.Engine.UI;
using JetBrains.Annotations;
using BFB.Engine.Audio;

namespace BFB.Engine.Scene
{
    /// <summary>
    /// A scene, or screen of the game.
    /// </summary>
    public abstract class Scene
    {
        /// <summary>
        /// Scene manager used to control scene state
        /// </summary>
        public static SceneManager SceneManager { get; set; }

        /// <summary>
        /// UI Manager for controlling UI related things
        /// </summary>
        public static UIManager UIManager { get; set; }

        /// <summary>
        /// Used to distribute content across the application without loading things twice and so on
        /// </summary>
        public static BFBContentManager ContentManager { get; set; }

        /// <summary>
        /// Used to play audio
        /// </summary>
        public static AudioManager AudioManager { get; set; }

        /// <summary>
        /// Contains useful drawing stuff
        /// </summary>
        public static GraphicsDeviceManager GraphicsDeviceManager { get; set; }

        /// <summary>
        /// Used for global events
        /// </summary>
        public static EventManager<GlobalEvent> GlobalEventManager { get; set; }

        /// <summary>
        /// Used for input events only
        /// </summary>
        public static EventManager<InputEvent> InputEventManager { get; set; }
        
        /// <summary>
        /// Used for interfacing a scene with a server
        /// </summary>
        public ClientSocketManager Client { get; set; }
        
        private readonly List<int> _eventInputListenerIds;
        private readonly List<int> _eventGlobalListenerIds;
        
        private SceneStatus _status;
        public readonly string Key;

        protected Scene(string key)
        {
            Key = key;
            _status = SceneStatus.Inactive;
            _eventInputListenerIds = new List<int>();
            _eventGlobalListenerIds = new List<int>();
        }

        /// <summary>
        /// Stops the scene by updating the scene status and calling unload on the scene
        /// </summary>
        public void Stop()
        {
            _status = SceneStatus.Inactive;
            Unload();
        }

        /// <summary>
        /// Pauses the scene by updating the scene status
        /// </summary>
        public void Pause()
        {
            _status = SceneStatus.Paused;
        }

        /// <summary>
        /// Starts the scene by updating the scene status
        /// </summary>
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

        /// <summary>
        /// Gets the current status of the scene
        /// </summary>
        /// <returns></returns>
        public SceneStatus GetStatus()
        {
            return _status;
        }

        protected virtual void Init() { }

        protected virtual void Load() { }

        protected virtual void Unload()
        {
            foreach (int id in _eventGlobalListenerIds)
                GlobalEventManager.RemoveEventListener(id);
            
            foreach (int id in _eventInputListenerIds)
                InputEventManager.RemoveEventListener(id);
            
            Client?.Disconnect("Scene Close");
        }

        public virtual void Update([UsedImplicitly] GameTime gameTime) { }

        public virtual void Draw(GameTime gameTime, SpriteBatch graphics) { }

        public void AddInputListener(string eventKey, Action<InputEvent> eventHandler)
        {
            _eventInputListenerIds.Add(InputEventManager.AddEventListener(eventKey, eventHandler));
        }
        
        public void AddGlobalListener(string eventKey, Action<GlobalEvent> eventHandler)
        {
            _eventGlobalListenerIds.Add(GlobalEventManager.AddEventListener(eventKey, eventHandler));
        }
    }
}
