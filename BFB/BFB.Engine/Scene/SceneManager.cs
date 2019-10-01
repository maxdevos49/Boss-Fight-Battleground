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
        private readonly Dictionary<string, Scene> AllScenes;
        private readonly List<Scene> ActiveScenes;

        #region constructor

        public SceneManager(ContentManager contentManager, GraphicsDeviceManager graphicsManager, EventManager eventManager)
        {
            _contentManager = contentManager;
            _graphicsManager = graphicsManager;
            _eventManager = eventManager;

            AllScenes = new Dictionary<string, Scene>();
            ActiveScenes = new List<Scene>();
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
            AllScenes.Add(scene.Key, scene);
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
            foreach (var scene in ActiveScenes)
            {
                //stop the scene
                StopScene(scene.Key);
            }

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
            ActiveScenes.Add(AllScenes[key]);

            //update that newly added scene's status
            ActiveScenes[ActiveScenes.Count-1].Start();
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

            foreach (var scene in ActiveScenes.Where(scene => key == scene.Key))
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
            
            foreach (var scene in ActiveScenes.Where(scene => key == scene.Key))
            {
                //Change the scene status/call any methods that may helpful for shutting down the scene
                scene.Stop();

                //remove the scene from active scenes
                ActiveScenes.Remove(scene);
                break;
            }
        }

        #endregion

        #region DrawScenes(GameTime gameTime, SpriteBatch graphics)

        /**
         * Draws all active scenes
         * */
        public void DrawScenes(GameTime gameTime, SpriteBatch graphics)
        {
            foreach (Scene scene in ActiveScenes)
                scene.Draw(gameTime, graphics);
        }

        #endregion

        #region UpdateScene(GameTime gameTime)

        /**
         * Updates all active scenes
         * */
        public void UpdateScenes(GameTime gameTime)
        {
            foreach (var scene in ActiveScenes.Where(scene => scene.GetStatus() == SceneStatus.ACTIVE))
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
            return ActiveScenes.Any(scene => key == scene.Key);
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
            int index = ActiveScenes.TakeWhile(scene => key != scene.Key).Count();

            if (ActiveScenes[index].Key != key)
                return;

            if (index + 1 >= ActiveScenes.Count) return;
            
            Scene temp = ActiveScenes[index+1];
            ActiveScenes[index+1] = ActiveScenes[index];
            ActiveScenes[index] = temp;
        }
        
        #endregion
        
        #region MoveSceneDown(string key)
        
        public void MoveSceneDown(string key)
        {
            int index = ActiveScenes.TakeWhile(scene => key != scene.Key).Count();

            if (ActiveScenes[index].Key != key)
                return;

            if (index <= 0) return;
            
            Scene temp = ActiveScenes[index-1];
            ActiveScenes[index-1] = ActiveScenes[index];
            ActiveScenes[index] = temp;
        }
        
        #endregion

    }
}
