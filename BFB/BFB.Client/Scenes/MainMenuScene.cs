using System;
using BFB.Engine.Scene;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BFB.Client.Scenes
{
    public class MainMenuScene : Scene
    {
        
        public MainMenuScene(): base(nameof(MainMenuScene))
        { }

        protected override void Init()
        {

        }
        
        protected override void Load()
        {
          
        }

        protected override void Unload()
        {

           Console.WriteLine("Stop Complaining jetbrains");
            base.Unload();
        }

        public override void Update(GameTime gameTime)
        {
            Console.WriteLine("Stop Complaining jetbrains");
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch graphics)
        {
            Console.WriteLine("Stop Complaining jetbrains");
            base.Draw(gameTime, graphics);
        }
    }
    
}