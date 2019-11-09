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
        
        public Vector2 Focus { get; set; }
        public float Rotation { get; set; }
        public Vector2 Origin { get; set; }
        public float Zoom { get; set; }
        public Vector2 ScreenCenter { get; protected set; }
        public Matrix Transform { get; set; }
        public float MoveSpeed { get; set; }
        
        
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
            Zoom = 0.3f;
            MoveSpeed = 5.25f;
            Rotation = 0;
        }

        public Vector3 GetScale()
        {
            float screenScale = (float)_graphicsDevice.Viewport.Width / ViewWidth;
            return new Vector3(Zoom + screenScale, Zoom + screenScale, 0);
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
        
        //TODO remove maybe because we will not need it with better renderer maybe
        public bool IsInView(Vector2 position, Texture2D texture)
        {
            // If the object is not within the horizontal bounds of the screen
            if ( (position.X + texture.Width) < (Position.X - Origin.X - 100) || (position.X) > (Position.X + Origin.X + 100) )
                return false;

            // If the object is not within the vertical bounds of the screen
            return !((position.Y + texture.Height) < (Position.Y - Origin.Y - 100)) && !((position.Y) > (Position.Y + Origin.Y + 100));
        }

    }

}