using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using ClientExperiments.Engine.Event;

namespace ClientExperiments.Engine.Scene
{
    public class SceneManager
    {
        //Dependencies
        public readonly ContentManager _contentManager;
        public readonly GraphicsDeviceManager _graphicsManager;
        public readonly EventManager _eventManager;

        //Properties
        private readonly Dictionary<string, Scene> AllScenes;
        private readonly Dictionary<string, Scene> ActiveScenes;

        #region constructor

        public SceneManager(ContentManager contentManager, GraphicsDeviceManager graphicsManager, EventManager eventManager)
        {
            _contentManager = contentManager;
            _graphicsManager = graphicsManager;
            _eventManager = eventManager;

            AllScenes = new Dictionary<string, Scene>();
            ActiveScenes = new Dictionary<string, Scene>();
        }

        #endregion

        #region AddScene(Scene[] scenes)

        /**
         * Adds an array of scenes to the scene manager
         * */
        public void AddScene(Scene[] scenes)
        {
            foreach (var scene in scenes)
            {
                AddScene(scene);
            }
        }

        #endregion

        #region AddScene(Scene scenes)

        /**
         * Adds a single scene to the scene manager
         * */
        public void AddScene(Scene scene)
        {
            if (!SceneExist(scene.Key))
            {
                scene.InjectDependencies(this, _contentManager, _graphicsManager, _eventManager);
                AllScenes.Add(scene.Key, scene);
            }
        }

        #endregion

        #region StartScene

        /**
         * Stops all active scenes and then starts the specified scene
         * */
        public void StartScene(string key)
        {
            if (SceneExist(key))
            {
                //Shutdown any currently active scenes
                foreach (var scene in ActiveScenes)
                {
                    //stop the scene
                    StopScene(scene.Key);
                }

                //Start the single scene
                LaunchScene(key);
            }
        }

        #endregion

        #region LaunchScene

        /**
         * Starts a single scene in parallel to any already running scenes.
         * */
        public void LaunchScene(string key)
        {
            if (SceneExist(key))
            {
                //Add to active scene
                ActiveScenes.Add(key, AllScenes[key]);

                //update scene status
                ActiveScenes[key].Start();
            }
        }

        #endregion

        #region PauseScene

        /**
         * Pauses the scene that is specified
         * */
        public void PauseScene(string key)
        {
            if (ActiveSceneExist(key))
            {
                //Update scene status
                ActiveScenes[key].Pause();
            }
        }

        #endregion

        #region StopScene

        /**
         * Shuts down the scene that is specified by the key
         * */
        public void StopScene(string key)
        {
            if (ActiveSceneExist(key))
            {

                //Change the scene status/call any methods that may helpful for shutting down the scene
                ActiveScenes[key].Stop();

                //remove the scene from active scenes
                ActiveScenes.Remove(key);
            }
        }

        #endregion

        #region DrawScenes

        /**
         * Draws all active scenes
         * */
        public void DrawScenes(GameTime gameTime, SpriteBatch graphics)
        {
            foreach (var scene in ActiveScenes)
                scene.Value.Draw(gameTime, graphics);
        }

        #endregion

        #region UpdateScene

        /**
         * Updates all active scenes
         * */
        public void UpdateScenes(GameTime gameTime)
        {
            foreach (var scene in ActiveScenes)
                if (scene.Value.GetStatus() == SceneStatus.ACTIVE)
                    scene.Value.Update(gameTime);
        }

        #endregion

        #region ActiveSceneExist

        /**
         * Checks if the scene is running or not
         * */
        private bool ActiveSceneExist(string key)
        {
            if (!ActiveScenes.ContainsKey(key))
            {
                //TODO throw exception or something
                return false;
            }
            return true;
        }

        #endregion

        #region SceneExist

        /**
         * Checks if the scene exist regardless if it is running
         * */
        private bool SceneExist(string key)
        {
            if (!AllScenes.ContainsKey(key))
            {
                //TODO throw exception or something
                return false;
            }
            return true;
        }

        #endregion

    }
}
