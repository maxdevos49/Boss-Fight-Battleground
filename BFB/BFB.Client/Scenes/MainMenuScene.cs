using BFB.Client.UI;
using BFB.Engine.Scene;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BFB.Client.Scenes
{
    public class MainMenuScene : Scene
    {
        
        public MainMenuScene() : base(nameof(MainMenuScene))
        {
        }

        #region Init

        protected override void Init()
        {
<<<<<<< HEAD
            UIManager.StartLayer(nameof(MainMenuUI));
=======
            UIManager.Start(nameof(MainMenuUI),this);
>>>>>>> 25eb9767d2ff3738c9e8f07fa547477d11c8cf6b
        }

        #endregion
        
        #region Load
        
        protected override void Load()
        {
        }

        #endregion
        
        #region Update
        
        public override void Update(GameTime gameTime)
        {
        }
        
        #endregion

        #region Draw
        
        public override void Draw(GameTime gameTime, SpriteBatch graphics)
        {

        }
        
        #endregion

    }
    
   
}