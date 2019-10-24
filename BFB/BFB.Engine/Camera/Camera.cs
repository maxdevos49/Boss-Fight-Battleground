using Microsoft.Xna.Framework;

namespace BFB.Engine.Camera
{
    public class Camera
    {
        private Vector2 _playerPos;
        
        public Camera(Vector2 playerPos)
        {
            _playerPos = playerPos;
        }
    }
}