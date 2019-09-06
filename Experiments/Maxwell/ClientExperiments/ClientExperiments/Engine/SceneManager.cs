using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace ClientExperiments.Engine
{
    public class SceneManager
    {

        public ContentManager _contentManager;

        private Dictionary<string, Scene> AllScenes;
        private Dictionary<string, Scene> ActiveScenes;

        public SceneManager(ContentManager contentManager)
        {
            _contentManager = contentManager;

            AllScenes = new Dictionary<string, Scene>();
            ActiveScenes = new Dictionary<string, Scene>();
        }

        /**
         * Starts the initial scene
         * */
        public void Start(string sceneKey)
        {
            if (AllScenes.ContainsKey(sceneKey))
            {
                //TODO add to active scenes and show inactive
            }
            else
            {
                //Error key does not exist so no scene to start
                //TODO Throw exception
            }

        }

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

        /**
         * Adds a single scene to the scene manager
         * */
        public void AddScene(Scene scene)
        {
            if (!AllScenes.ContainsKey(scene.Key))
            {
                scene.InjectDependencies(this, _contentManager);
                AllScenes.Add(scene.Key, scene);
            }
            else
            {
                //Error duplicate key
                //TODO Throw exception
            }
        }


        public void RemoveScene(string key)
        {
            //TODO
        }

        /**
         * Stops all active scenes and then starts the specified scene
         * */
        public void StartScene(string key)
        {
            //TODO
        }

        /**
         * Starts a scene in parallel to any already running scenes
         * */
        public void LaunchScene(string key)
        {
            //TODO
        }

        /**
         * Pauses the scene that is specified
         * */
        public void PauseScene(string key)
        {
            //TODO
        }

        /**
         * Shuts down the scene that is specified by the key
         * */
        public void StopScene(string key)
        {
            //TODO
        }

        /**
         * Draws all active scenes
         * */
        public void DrawScenes(GameTime gameTime)
        {
            foreach (var scene in ActiveScenes)
            {
                scene.Value.Draw(gameTime);
            }
        }

        /**
         * Updates all active scenes
         * */
        public void UpdateScenes(GameTime gameTime)
        {
            foreach (var scene in ActiveScenes)
            {
                scene.Value.Update(gameTime);
            }
        }

    }
}
