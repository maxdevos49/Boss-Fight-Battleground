using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BFB.Engine.TileMap
{

    public class Camera
    {
        public Vector2 Position
        {
            get => _position;
            private set => _position = value;
        }
        
        /// <summary>
        /// The focus point.
        /// </summary>
        public Vector2 Focus { get; set; }
        public float Rotation { get; set; }
        /// <summary>
        /// The origin location.
        /// </summary>
        public Vector2 Origin { get; set; }
        /// <summary>
        /// The zoom value.
        /// </summary>
        public float Zoom { get; set; }
        /// <summary>
        /// The center of the screen location.
        /// </summary>
        public Vector2 ScreenCenter { get; protected set; }
        /// <summary>
        /// The matrix to transform the camera.
        /// </summary>
        public Matrix Transform { get; set; }
        /// <summary>
        /// The speed value of the movement.
        /// </summary>
        public float MoveSpeed { get; set; }

        public int Left => (int) (Position.X - Origin.X);
        public int Top => (int) (Position.Y - Origin.Y);
        public int Right => (int) (Position.X + Origin.X);
        public int Bottom => (int) (Position.Y + Origin.Y);
        
        
        private Vector2 _position;
        private readonly GraphicsDevice _graphicsDevice;
        private readonly int _worldWidth;
        private readonly int _worldHeight;
        public readonly int ViewWidth;
        public readonly int ViewHeight;

        public Camera(GraphicsDevice graphicsDevice,int worldWidth, int worldHeight, int viewWidth = 800, int viewHeight = 450)
        {
            _graphicsDevice = graphicsDevice;
            _worldWidth = worldWidth;
            _worldHeight = worldHeight;
            ViewWidth = viewWidth;
            ViewHeight = viewHeight;
            
            Position = Vector2.Zero;
            Origin = Vector2.Zero;
            ScreenCenter = Vector2.Zero;
            Zoom = 1f-0.4f;
            MoveSpeed = 10.25f;
            Rotation = 0;
        }

        public Vector3 GetScale()
        {
            float screenScale = (float)_graphicsDevice.Viewport.Width / ViewWidth;
            return new Vector3(Zoom * screenScale, Zoom * screenScale, 0);
        }
        
        public void Update(GameTime gameTime)
        {
            ScreenCenter = new Vector2((float)_graphicsDevice.Viewport.Width / 2, (float)_graphicsDevice.Viewport.Height / 2);
            
            
            // Create the Transform
            Transform = Matrix.Identity *
                        Matrix.CreateTranslation(-Position.X, -Position.Y, 0) *
                        Matrix.CreateRotationZ(Rotation) *
                        Matrix.CreateTranslation(Origin.X, Origin.Y, 0) *
                        Matrix.CreateScale(GetScale());

            
            Origin = ScreenCenter / GetScale().X;

            // Move the Camera to the position that it needs to go
            float delta = (float) gameTime.ElapsedGameTime.TotalSeconds ;

            _position.X += (Focus.X - Position.X) * MoveSpeed * delta;
            _position.Y += (Focus.Y - Position.Y) * MoveSpeed * delta;

            //Keep camera within bounds of the world
            if (_position.X < Origin.X)
                _position.X = Origin.X;
            else if(_position.X > _worldWidth - Origin.X)
                _position.X = _worldWidth - Origin.X;
           
        }

    }

}