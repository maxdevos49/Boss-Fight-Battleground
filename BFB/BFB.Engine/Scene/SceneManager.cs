//c#
using System;
using System.Linq;
using System.Collections.Generic;

//Monogame
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

//Engine
using BFB.Engine.Event;

//Jetbrains
using JetBrains.Annotations;


namespace BFB.Engine.Scene
{
    public class SceneManager
    {
        //Dependencies
        private readonly ContentManager _contentManager;
        private readonly GraphicsDeviceManager _graphicsManager;
        private readonly EventManager _eventManager;

        //Properties
        private readonly Dictionary<string, Scene> _allScenes;
        private readonly Dictionary<string, Scene> _activeScenes;

        #region constructor

        public SceneManager(ContentManager contentManager, GraphicsDeviceManager graphicsManager, EventManager eventManager)
        {
            _contentManager = contentManager;
            _graphicsManager = graphicsManager;
            _eventManager = eventManager;

            _allScenes = new Dictionary<string, Scene>();
            _activeScenes = new Dictionary<string, Scene>();
        }

        #endregion

        #region AddScene(Scene[] scenes)

        /**
         * Adds an array of scenes to the scene manager
         * */
        public void AddScene(IEnumerable<Scene> scenes)
        {
            foreach (Scene scene in scenes)
            {
                AddScene(scene);
            }
        }

        #endregion

        #region AddScene(Scene scenes)

        /**
         * Adds a single scene to the scene manager
         * */
        [UsedImplicitly]
        public void AddScene(Scene scene)
        {
            if (SceneExist(scene.Key)) return;
            
            scene.InjectDependencies(this, _contentManager, _graphicsManager, _eventManager);
            
            _allScenes.Add(scene.Key, scene);
        }

        #endregion

        #region StartScene

        /**
         * Stops all active scenes and then starts the specified scene
         * */
        public void StartScene(string key)
        {
            if (!SceneExist(key)) return;
            
            //Shutdown any currently active scenes
            foreach (KeyValuePair<string, Scene> scene in _activeScenes)
            {
                //stop the scene
                StopScene(scene.Key);
            }

            //Start the single scene
            LaunchScene(key);
        }

        #endregion

        #region LaunchScene

        /**
         * Starts a single scene in parallel to any already running scenes.
         * */
        public void LaunchScene(string key)
        {
            if (!SceneExist(key) || ActiveSceneExist(key)) return;
            
            //Add to active scene
            _activeScenes.Add(key, _allScenes[key]);

            //update scene status
            _activeScenes[key].Start();
        }

        #endregion

        #region PauseScene

        /**
         * Pauses the scene that is specified
         * */
        [UsedImplicitly]
        public void PauseScene(string key)
        {
            if (ActiveSceneExist(key))
            {
                //Update scene status
                _activeScenes[key].Pause();
            }
        }

        #endregion

        #region StopScene

        /**
         * Shuts down the scene that is specified by the key
         * */
        public void StopScene(string key)
        {
            if (!ActiveSceneExist(key)) return;
            
            //Change the scene status/call any methods that may helpful for shutting down the scene
            _activeScenes[key].Stop();

            //remove the scene from active scenes
            _activeScenes.Remove(key);
        }

        #endregion

        #region DrawScenes

        /**
         * Draws all active scenes
         * */
        public void DrawScenes(GameTime gameTime, SpriteBatch graphics)
        {
            try
            {
                foreach (KeyValuePair<string, Scene> scene in _activeScenes.ToList())
                    scene.Value?.Draw(gameTime, graphics);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        #endregion

        #region UpdateScene

        /**
         * Updates all active scenes
         * */
        public void UpdateScenes(GameTime gameTime)
        {
            try
            {
                foreach (KeyValuePair<string, Scene> scene in _activeScenes.ToList().Where(scene => scene.Value?.GetStatus() == SceneStatus.Active))
                    scene.Value?.Update(gameTime);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        #endregion

        #region ActiveSceneExist

        /**
         * Checks if the scene is running or not
         * */
        [UsedImplicitly]
        public bool ActiveSceneExist(string key)
        {
            return _activeScenes.ContainsKey(key);
        }

        #endregion

        #region SceneExist

        /**
         * Checks if the scene exist regardless if it is running
         * */
        [UsedImplicitly]
        public bool SceneExist(string key)
        {
            return _allScenes.ContainsKey(key);
        }

        #endregion

    }
}
