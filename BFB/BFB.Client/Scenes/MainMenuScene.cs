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
            UIManager.StartLayer(nameof(MainMenuUI),this);
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