using BFB.Engine.Content;
using BFB.Engine.Entity.Components.Graphics;
using BFB.Engine.Math;
using Microsoft.Xna.Framework;

namespace BFB.Engine.Entity.Components.Camera
{
    
    public interface ICamera
    {
        BfbVector Position { get; set; }

        float MoveSpeed { get; set; }

        BfbVector Origin { get; }
        
        BfbVector ScreenCenter { get; }

        Matrix Transform { get; }
    }
    public class Camera : IGraphicsComponent, ICamera
    {
        private GraphicsDeviceManager _graphicsDeviceManager;

        private BfbVector _position;
        private float _viewportWidth;
        private float _viewportHeight;

        public Camera(GraphicsDeviceManager graphicsDeviceManager)
        {
            _graphicsDeviceManager = graphicsDeviceManager;
        }
        
        public BfbVector Position
        {
            get => _position;
            set => _position = value;
        }
        public BfbVector Origin { get; set; }
        public BfbVector ScreenCenter { get; protected set; }
        public Matrix Transform { get; set; }
        public float MoveSpeed { get; set; }

        public void Init()
        {
            _viewportWidth = _graphicsDeviceManager.GraphicsDevice.Viewport.Width;
            _viewportHeight = _graphicsDeviceManager.GraphicsDevice.Viewport.Height;
            
            ScreenCenter = new BfbVector(_viewportWidth/2, _viewportHeight/2);
            MoveSpeed = 1.25f;
        }

        public void Update(GameTime gameTime)
        {
            Transform = Matrix.Identity *
                        Matrix.CreateTranslation(-Position.X, -Position.Y, 0) *
                        Matrix.CreateTranslation(Origin.X, Origin.Y, 0);

            Origin = ScreenCenter;

            // Move the Camera to the position that it needs to go
            var delta = (float) gameTime.ElapsedGameTime.TotalSeconds;

            _position.X += (Focus.Position.X - Position.X) * MoveSpeed * delta;
            _position.Y += (Focus.Position.Y - Position.Y) * MoveSpeed * delta;
        }
    }
}