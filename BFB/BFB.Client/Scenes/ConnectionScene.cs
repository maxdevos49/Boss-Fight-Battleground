using System;
using BFB.Engine.Scene;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BFB.Client.Scenes
{
    public class ConnectionScene : Scene
    {


        public ConnectionScene() : base(nameof(ConnectionScene))
        {

        }

        protected override void Init()
        {
            EventManager.AddEventListener("keydown", (Event) =>
            {

            });
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(GameTime gameTime, SpriteBatch graphics)
        {
        }


    }
}
