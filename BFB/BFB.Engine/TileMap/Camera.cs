using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BFB.Engine.TileMap
{

    public class Camera2D
    {
        private Vector2 _position;
        
        private readonly WorldManager _worldManager;
        private readonly GraphicsDevice _graphicsDevice;
        
        private const int CameraWidth = 800;
        private const int CameraHeight = 450;
            
        
        
        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }
        
        public Vector2 Focus { get; set; }

        public float Rotation { get; set; }
        public Vector2 Origin { get; set; }
        public float Zoom { get; set; }
        public Vector2 ScreenCenter { get; protected set; }
        public Matrix Transform { get; set; }
        public float MoveSpeed { get; set; }

        public Camera2D(WorldManager worldManager, GraphicsDevice graphicsDevice)
        {
            _worldManager = worldManager;
            _graphicsDevice = graphicsDevice;
            
            Position = Vector2.Zero;
            Origin = Vector2.Zero;
            ScreenCenter = Vector2.Zero;
            
            Zoom = 1.0f;
            MoveSpeed = 5.25f;
            Rotation = 0;
        }

        private Vector3 CheckScreenScale()
        {
            //Player is positioned wrong after this
            float scaleX = (float)_graphicsDevice.Viewport.Width / CameraWidth;
            float scaleY = (float)_graphicsDevice.Viewport.Height / CameraHeight;
            return new Vector3(scaleX, scaleY, 1.0f);
        }
        
        

        public void Update(GameTime gameTime)
        {
            ScreenCenter = new Vector2((float)_graphicsDevice.Viewport.Width / 2, (float)_graphicsDevice.Viewport.Height / 2);

            // Create the Transform
            Transform = Matrix.Identity *
                        Matrix.CreateTranslation(-Position.X, -Position.Y, 0) *
                        Matrix.CreateRotationZ(Rotation) *
                        Matrix.CreateTranslation(Origin.X, Origin.Y, 0) *
                        Matrix.CreateScale(new Vector3(Zoom, Zoom, Zoom));
//                       * Matrix.CreateScale(CheckScreenScale());

            Origin = ScreenCenter / Zoom;

            // Move the Camera to the position that it needs to go
            float delta = (float) gameTime.ElapsedGameTime.TotalSeconds;

            _position.X += (Focus.X - Position.X) * MoveSpeed * delta;
            _position.Y += (Focus.Y - Position.Y) * MoveSpeed * delta;

            //Keep camera within bounds of the world
            if (_position.X < Origin.X)
                _position.X = Origin.X;
            else if(_position.X > _worldManager.WorldOptions.WorldChunkWidth * _worldManager.WorldOptions.ChunkSize * _worldManager.WorldOptions.WorldScale - Origin.X)
                _position.X = _worldManager.WorldOptions.WorldChunkWidth * _worldManager.WorldOptions.ChunkSize * _worldManager.WorldOptions.WorldScale - Origin.X;
           
//            if (_position.Y < Origin.Y)
//                _position.Y = Origin.Y;
//            else if(_position.Y > _worldManager.WorldOptions.WorldChunkHeight * _worldManager.WorldOptions.ChunkSize * _worldManager.WorldOptions.WorldScale - Origin.Y)
//                _position.Y = _worldManager.WorldOptions.WorldChunkHeight * _worldManager.WorldOptions.ChunkSize * _worldManager.WorldOptions.WorldScale - Origin.Y;
////                
        }
        
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