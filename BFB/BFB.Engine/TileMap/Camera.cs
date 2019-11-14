using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BFB.Engine.TileMap
{

<<<<<<< HEAD
    public class Camera
    {
=======
    /// <summary>
    /// Used to give a camera on an object.
    /// </summary>
    public class Camera2D
    {
        private Vector2 _position;
        
        private readonly WorldManager _worldManager;
        private readonly GraphicsDevice _graphicsDevice;
        
        private const int CameraWidth = 800;
        private const int CameraHeight = 450;
            
        
        /// <summary>
        /// The vector of the current position.
        /// </summary>
>>>>>>> 5d8a6aa64e8cd5934e74929028a6f2a776a530a2
        public Vector2 Position
        {
            get => _position;
            private set => _position = value;
        }
        
        /// <summary>
        /// The focus point.
        /// </summary>
        public Vector2 Focus { get; set; }
<<<<<<< HEAD
=======

        /// <summary>
        /// The rotation value.
        /// </summary>
>>>>>>> 5d8a6aa64e8cd5934e74929028a6f2a776a530a2
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
        
        
        private Vector2 _position;
        private readonly GraphicsDevice _graphicsDevice;
        private readonly int _worldWidth;
        private readonly int _worldHeight;
        public readonly int ViewWidth;
        public readonly int ViewHeight;

<<<<<<< HEAD
        public Camera(GraphicsDevice graphicsDevice,int worldWidth, int worldHeight, int viewWidth = 800, int viewHeight = 450)
=======
        /// <summary>
        /// The camera configuration of the camera.
        /// </summary>
        /// <param name="worldManager">The manager of the world.</param>
        /// <param name="graphicsDevice">The graphics of the device.</param>
        public Camera2D(WorldManager worldManager, GraphicsDevice graphicsDevice)
>>>>>>> 5d8a6aa64e8cd5934e74929028a6f2a776a530a2
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
            MoveSpeed = 10.25f;
            Rotation = 0;
        }

<<<<<<< HEAD
        public Vector3 GetScale()
=======
        /// <summary>
        /// Checks the screen scale.
        /// </summary>
        /// <returns>Returns the Vector3 of the scale.</returns>
        private Vector3 CheckScreenScale()
>>>>>>> 5d8a6aa64e8cd5934e74929028a6f2a776a530a2
        {
            float screenScale = (float)_graphicsDevice.Viewport.Width / ViewWidth;
            return new Vector3(Zoom + screenScale, Zoom + screenScale, 0);
        }
        
<<<<<<< HEAD
=======
        
        /// <summary>
        /// Updates the camera.
        /// </summary>
        /// <param name="gameTime">The time of the game.</param>
>>>>>>> 5d8a6aa64e8cd5934e74929028a6f2a776a530a2
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
           
<<<<<<< HEAD
=======
//            if (_position.Y < Origin.Y)
//                _position.Y = Origin.Y;
//            else if(_position.Y > _worldManager.WorldOptions.WorldChunkHeight * _worldManager.WorldOptions.ChunkSize * _worldManager.WorldOptions.WorldScale - Origin.Y)
//                _position.Y = _worldManager.WorldOptions.WorldChunkHeight * _worldManager.WorldOptions.ChunkSize * _worldManager.WorldOptions.WorldScale - Origin.Y;
////                
        }
        
        /// <summary>
        /// Checks if the object is in view.
        /// </summary>
        /// <param name="position">The position of the object.</param>
        /// <param name="texture">The texture of the object.</param>
        /// <returns>Returns true if the object is in view.</returns>
        public bool IsInView(Vector2 position, Texture2D texture)
        {
            // If the object is not within the horizontal bounds of the screen
            if ( (position.X + texture.Width) < (Position.X - Origin.X - 100) || (position.X) > (Position.X + Origin.X + 100) )
                return false;

            // If the object is not within the vertical bounds of the screen
            return !((position.Y + texture.Height) < (Position.Y - Origin.Y - 100)) && !((position.Y) > (Position.Y + Origin.Y + 100));
>>>>>>> 5d8a6aa64e8cd5934e74929028a6f2a776a530a2
        }

    }

}