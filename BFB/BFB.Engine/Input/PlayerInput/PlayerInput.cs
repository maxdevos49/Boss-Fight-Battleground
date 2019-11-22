using System;
using BFB.Engine.Entity;
using BFB.Engine.Math;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BFB.Engine.Input.PlayerInput
{
    /// <summary>
    ///  This class will handle the input that the player gives
    /// transforming mouse & keyboard input to game controls.
    /// </summary>
    public class PlayerInput
    {

        private readonly ControlState _controlState;
        private bool _inputChange;

        /// <summary>
        /// Handles the input for each scene here.
        /// </summary>
        public PlayerInput()
        {
            _controlState = new ControlState();
            _inputChange = false;
        }

        public void Init(Scene.Scene scene)
        {
            scene.AddInputListener("keypress", (e) =>
            {
                switch (e.Keyboard.KeyEnum)
                {
                    case Keys.Left:
                    case Keys.A:
                        _controlState.Left = true;
                        _inputChange = true;
                        break;
                    case Keys.Right:
                    case Keys.D:
                        _controlState.Right = true;
                        _inputChange = true;
                        break;
                    case Keys.W:
                    case Keys.Space:
                        _controlState.Jump = true;
                        _inputChange = true;
                        break;
                }
                
                _controlState.Mouse.X = e.Mouse.X;
                _controlState.Mouse.Y = e.Mouse.Y;
            });
            
            scene.AddInputListener("keyup", (e) =>
            {
                switch (e.Keyboard.KeyEnum)
                {
                    case Keys.D1:
                        _controlState.HotBarLeft = true;
                        _inputChange = true;
                        break;
                    case Keys.D2:
                        _controlState.HotBarRight = true;
                        _inputChange = true;
                        break;
                    case Keys.Left:
                    case Keys.A:
                        _controlState.Left = false;
                        _inputChange = true;
                        break;
                    case Keys.Right:
                    case Keys.D:
                        _controlState.Right = false;
                        _inputChange = true;
                        break;
                    case Keys.W:
                    case Keys.Space:
                        _controlState.Jump = false;
                        _inputChange = true;
                        break;
                }
                
                _controlState.Mouse.X = e.Mouse.X;
                _controlState.Mouse.Y = e.Mouse.Y;
            });
            
            scene.AddInputListener("mousemove", (e) =>
            {
                _controlState.Mouse.X = e.Mouse.X;
                _controlState.Mouse.Y = e.Mouse.Y;
                
                _inputChange = true;
            });
            
            scene.AddInputListener("mouseclick", (e) =>
            {
                _controlState.LeftClick = e.Mouse.LeftButton == ButtonState.Pressed;
                _controlState.RightClick = e.Mouse.RightButton == ButtonState.Pressed;
                
                _controlState.Mouse.X = e.Mouse.X;
                _controlState.Mouse.Y = e.Mouse.Y;
                
                _inputChange = true;
            });
            
            scene.AddInputListener("mouseup", (e) =>
            {
                _controlState.LeftClick = e.Mouse.LeftButton == ButtonState.Pressed;
                _controlState.RightClick = e.Mouse.RightButton == ButtonState.Pressed;
                
                _controlState.Mouse.X = e.Mouse.X;
                _controlState.Mouse.Y = e.Mouse.Y;
                
                _inputChange = true;
            });
        }

        /// <summary>
        /// Gets the player state, shockingly enough.
        /// </summary>
        /// <returns></returns>
        public ControlState GetPlayerState()
        {
            if(!_controlState.HotBarLeft && !_controlState.HotBarRight)//We need to send on the tick for these otherwise we get super scrolling effect
                _inputChange = false;
            
            ControlState input = _controlState.Clone();
            _controlState.HotBarRight = false;
            _controlState.HotBarLeft = false;
            return input;
        }
        
        ///<summary>
        /// Allows viewing of the player state without indicating that the input had been updated
        /// </summary>
        /// <returns></returns>
        public ControlState PeekPlayerState()
        {
            return _controlState.Clone();
        }

        /// <summary>
        /// Returns if the input has changed from the last game update.
        /// </summary>
        /// <returns></returns>
        public bool InputChanged()
        {
            return _inputChange;
        }
    }
}