//MonoGame
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

//Engine
using BFB.Engine.Scene;
using BFB.Engine.Debug;

namespace BFB.Client.Scenes
{
    public class DebugScene : Scene
    {
        private SpriteFont Font { get; set; }

        private string Fps { get; set; }
        private string MousePos { get; set; }
        private string KeysPressed { get; set; }

        private readonly FrameCounter _frameCounter;

        public DebugScene() : base(nameof(DebugScene))
        {

            _frameCounter = new FrameCounter();

            Fps = "0";
            MousePos = "Mouse Position - X: 0, Y: 0";
            KeysPressed = "Keys Pressed:";
        }

        protected override void Init()
        {
            EventManager.AddEventListener("keypress", (Event) =>
            {
                KeysPressed = $"Keys Pressed: {string.Join(", ", Event.Keyboard.KeyboardState.GetPressedKeys())}";
            });

            EventManager.AddEventListener("keyup", (Event) =>
            {
                KeysPressed = $"Keys Pressed: {string.Join(", ", Event.Keyboard.KeyboardState.GetPressedKeys())}";
            });

            EventManager.AddEventListener("mousemove", (Event) =>
            {
                MousePos = $"Mouse Position - X: {Event.Mouse.X}, Y: {Event.Mouse.Y}";
            });
        }

        protected override void Load()
        {
            Font = ContentManager.Load<SpriteFont>("Fonts\\Papyrus");
        }

        public override void Update(GameTime gameTime)
        {
            Fps = $"FPS: {_frameCounter.AverageFramesPerSecond}";
        }

        public override void Draw(GameTime gameTime, SpriteBatch graphics)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _frameCounter.Update(deltaTime);

            graphics.DrawString(Font, Fps, Vector2.Zero, Color.Black);
            graphics.DrawString(Font, MousePos, new Vector2(0, 13), Color.Black);
            graphics.DrawString(Font, KeysPressed, new Vector2(0, 25), Color.Black);

        }
    }
}
