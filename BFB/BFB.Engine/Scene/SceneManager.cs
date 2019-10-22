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
using BFB.Engine.UI;

//Jetbrains
using JetBrains.Annotations;


namespace BFB.Engine.Scene
{
    public class SceneManager
    {
        //Dependencies
        private readonly ContentManager _contentManager;
        private readonly GraphicsDeviceManager _graphicsManager;
        private readonly EventManager<GlobalEvent> _eventManager;
        private readonly UIManager _uiManager;

        //Properties
        private readonly Dictionary<string, Scene> _allScenes;
        private readonly List<Scene> _activeScenes;

        #region constructor

        public SceneManager(ContentManager contentManager, GraphicsDeviceManager graphicsManager, EventManager<GlobalEvent> eventManager, UIManager uiManager)
        {
            _contentManager = contentManager;
            _graphicsManager = graphicsManager;
            _eventManager = eventManager;
            _uiManager = uiManager;
            
            _allScenes = new Dictionary<string, Scene>();
            _activeScenes = new List<Scene>();
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
            
//            scene.InjectDependencies(this, _contentManager, _graphicsManager, _eventManager);
            _allScenes.Add(scene.Key, scene);
        }

        #endregion

        #region StartScene(string key)

        /**
         * Stops all active scenes and then starts the specified scene
         * */
        public void StartScene(string key)
        {
            if (!SceneExist(key)) return;
            
            //Shutdown any currently active scenes
            StopScenes();
            
            //Shutdown active UI layers
            _uiManager.StopLayers();

            //Start the single scene
            LaunchScene(key);
        }

        #endregion

        #region LaunchScene(string key)

        /**
         * Starts a single scene in parallel to any already running scenes.
         * */
        public void LaunchScene(string key)
        {
            if (!SceneExist(key) || ActiveSceneExist(key)) return;
            
            //Add to active scene
            _activeScenes.Add(_allScenes[key]);

            //update that newly added scene's status
            _activeScenes[_activeScenes.Count-1].Start();
        }

        #endregion

        #region PauseScene(string key)

        /**
         * Pauses the scene that is specified
         * */
        [UsedImplicitly]
        public void PauseScene(string key)
        {
            if (!ActiveSceneExist(key)) return;

            foreach (var scene in _activeScenes.Where(scene => key == scene.Key))
            {
                scene.Pause();
                break;
            }
        }

        #endregion

        #region StopScene(string key)

        /**
         * Shuts down the scene that is specified by the key
         * */
        public void StopScene(string key)
        {
            if (!ActiveSceneExist(key)) return;
            
            foreach (var scene in _activeScenes.Where(scene => key == scene.Key))
            {
                //Change the scene status/call any methods that may helpful for shutting down the scene
                scene.Stop();

                //remove the scene from active scenes
                _activeScenes.Remove(scene);
                break;
            }
        }

        [UsedImplicitly]
        public void StopScenes()
        {
            foreach (Scene scene in _activeScenes)
            {
                //Change the scene status/call any methods that may helpful for shutting down the scene
                scene.Stop();
            }
            
            _activeScenes.Clear();
        }

        #endregion

        #region DrawScenes(GameTime gameTime, SpriteBatch graphics)

        /**
         * Draws all active scenes
         * */
        public void DrawScenes(GameTime gameTime, SpriteBatch graphics)
        {
            foreach (Scene scene in _activeScenes)
                scene.Draw(gameTime, graphics);
        }

        #endregion

        #region UpdateScene(GameTime gameTime)

        /**
         * Updates all active scenes
         * */
        public void UpdateScenes(GameTime gameTime)
        {
            foreach (var scene in _activeScenes.Where(scene => scene.GetStatus() == SceneStatus.Active))
                scene.Update(gameTime);
        }

        #endregion

        #region ActiveSceneExist(string key)

        /**
         * Checks if the scene is running or not
         * */
        [UsedImplicitly]
        public bool ActiveSceneExist(string key)
        {
            return _activeScenes.Any(scene => key == scene.Key);
        }

        #endregion

        #region SceneExist(string key)

        /**
         * Checks if the scene exist regardless if it is running
         * */
        [UsedImplicitly]
        public bool SceneExist(string key)
        {
            return _allScenes.ContainsKey(key);
        }

        #endregion

        #region MoveSceneUp(string key)
        
        public void MoveSceneUp(string key)
        {
            int index = _activeScenes.TakeWhile(scene => key != scene.Key).Count();

            if (_activeScenes[index].Key != key)
                return;

            if (index + 1 >= _activeScenes.Count) return;
            
            Scene temp = _activeScenes[index+1];
            _activeScenes[index+1] = _activeScenes[index];
            _activeScenes[index] = temp;
        }
        
        #endregion
        
        #region MoveSceneDown(string key)
        
        public void MoveSceneDown(string key)
        {
            int index = _activeScenes.TakeWhile(scene => key != scene.Key).Count();

            if (_activeScenes[index].Key != key)
                return;

            if (index <= 0) return;
            
            Scene temp = _activeScenes[index-1];
            _activeScenes[index-1] = _activeScenes[index];
            _activeScenes[index] = temp;
        }
        
        #endregion

    }
}
