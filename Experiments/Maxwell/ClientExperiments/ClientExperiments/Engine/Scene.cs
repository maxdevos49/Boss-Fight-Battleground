using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace ClientExperiments.Engine
{
    public abstract class Scene
    {
 
        public SceneManager _sceneManager;
        public ContentManager _contentManager;
        //State Manager
        //Event Manager


        private SceneStatus Status;
        public readonly string Key;

        private Scene(string key)
        {
            Key = key;
            Status = SceneStatus.INOPERABLE;
        }

        //Inject the scene dependencies
        public void InjectDependencies(SceneManager sceneManager, ContentManager contentManager)
        {
            _sceneManager = sceneManager;
            _contentManager = contentManager;

            //Indicate the scene is now in a operable state but inactive
            Status = SceneStatus.INACTIVE;
        }


        public virtual void Init() { }

        public virtual void Load() { }

        public virtual void Unload() { }

        public virtual void Update(GameTime gameTime) { }

        public virtual void Draw(GameTime gameTime) { }

    }
}
