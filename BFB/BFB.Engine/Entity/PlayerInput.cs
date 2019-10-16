using System.Reflection.Metadata;
using Microsoft.Xna.Framework.Input;

namespace BFB.Engine.Entity
{
    public class PlayerInput
    {
        /*
         * This class will handle the input that the player gives
         * transforming mouse & keyboard input to game controls.
         */
        private PlayerState _playerState;
        
        public PlayerInput(Scene.Scene scene)
        {
            _playerState = new PlayerState();
            scene.AddEventListener("keypress", (e) =>
            {
                switch (e.Keyboard.KeyEnum)
                {
                    case Keys.Left:
                    case Keys.A:
                        _playerState.Left = true;
                        break;
                    case Keys.Right:
                    case Keys.D:
                        _playerState.Right = true;
                        break;
                    case Keys.Up:
                    case Keys.W:
                        _playerState.Jump = true;
                        break;
                    default:
                        break;
                }
            });
            
            scene.AddEventListener("keyup", (e) =>
            {
                switch (e.Keyboard.KeyEnum)
                {
                    case Keys.Left:
                    case Keys.A:
                        _playerState.Left = false;
                        break;
                    case Keys.Right:
                    case Keys.D:
                        _playerState.Right = false;
                        break;
                    case Keys.Up:
                    case Keys.W:
                        _playerState.Jump = false;
                        break;
                    default:
                        break;
                }
            });
            
            scene.AddEventListener("mousemove", (e) =>
            {
                _playerState.Mouse.X = e.Mouse.X;
                _playerState.Mouse.Y = e.Mouse.Y;
            });
            
            scene.AddEventListener("mouseclick", (e) =>
            {
                _playerState.LeftClick = e.Mouse.LeftButton == ButtonState.Pressed;
                _playerState.RightClick = e.Mouse.RightButton == ButtonState.Pressed;
            });
            
            scene.AddEventListener("mouseup", (e) =>
            {
                _playerState.LeftClick = e.Mouse.LeftButton == ButtonState.Released;
                _playerState.RightClick = e.Mouse.RightButton == ButtonState.Released;
            });
        }

        public PlayerState GetPlayerState()
        {
            return _playerState;
        }
    }
}