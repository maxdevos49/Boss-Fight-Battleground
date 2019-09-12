//C#
using System;

//MonoGame
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

//Engine
using ClientExperiments.Engine.Scene;
using ClientExperiments.Engine.Debug;

namespace ClientExperiments.Scenes
{
    public class DebugScene : Scene
    {

        public SpriteFont Font { get; set; }

        public string FPS { get; set; }
        public string MousePos { get; set; }
        public string KeysPressed { get; set; }

        private FrameCounter _frameCounter;

        public DebugScene() : base(nameof(DebugScene)) {

            //FPS Counter
            _frameCounter = new FrameCounter();
            FPS = "0";
            MousePos = "Mouse Position - X: 0, Y: 0";
            KeysPressed = "Keys Pressed:";
        }

        public override void Init()
        {
            //Key events
            _eventManager.AddEventListener("keypress", (Event) =>
            {
                KeysPressed = $"Keys Pressed: {string.Join(", ", Event.Keyboard.KeyboardState.GetPressedKeys())}";
            });

            //Key events
            _eventManager.AddEventListener("keyup", (Event) =>
            {
                KeysPressed = $"Keys Pressed: {string.Join(", ", Event.Keyboard.KeyboardState.GetPressedKeys())}";
            });

            //Mouse events
            var handlerid = _eventManager.AddEventListener("mousemove", (Event) =>
            {
                MousePos = $"Mouse Position - X: {Event.Mouse.X}, Y: {Event.Mouse.Y}";
            });
        }

        public override void Load()
        {
            Font = _contentManager.Load<SpriteFont>("Fonts\\Papyrus");
        }

        public override void Unload()
        {


            //Currently unused
        }

        public override void Update(GameTime gameTime)
        {
            FPS = $"FPS: {_frameCounter.AverageFramesPerSecond}";
        }

        public override void Draw(GameTime gameTime, SpriteBatch graphics)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _frameCounter.Update(deltaTime);

            graphics.DrawString(Font, FPS, Vector2.Zero, Color.Black);
            graphics.DrawString(Font, MousePos, new Vector2(0, 13), Color.Black);
            graphics.DrawString(Font, KeysPressed, new Vector2(0, 25), Color.Black);

        }
    }
}
