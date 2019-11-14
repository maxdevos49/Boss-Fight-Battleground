//C#
using System.Linq;
using System.Collections.Generic;

//Monogame
using Microsoft.Xna.Framework.Input;

//Engine
using BFB.Engine.Event;

namespace BFB.Engine.Input
{
    /// <summary>
    /// Takes keyboard input and emits it to the server through events.
    /// </summary>
    public class KeyboardInput
    {
        private readonly EventManager<InputEvent> _eventManager;

        private readonly List<Keys> _pressedKeys;
        private KeyboardState _keyboardState;

        public KeyboardInput(EventManager<InputEvent> eventManager)
        {
            _eventManager = eventManager;
            _pressedKeys = new List<Keys>();
        }

        /// <summary>
        /// Updates the keyboard state and emits any new events based on keys pressed.
        /// </summary>
        public void UpdateKeyboard()
        {
            _keyboardState = Keyboard.GetState();

            List<Keys> keysPressed = _keyboardState.GetPressedKeys().ToList();

            //key press
            for (int i = keysPressed.Count - 1; i >= 0; i--)
            {
                if (_pressedKeys.Contains(keysPressed[i])) continue;
                
                EmitKeyPressEvent(CreateKeyboardEvent(keysPressed[i]));
                
                _pressedKeys.Add(keysPressed[i]);
            }


            //key down
            for (int i = keysPressed.Count - 1; i >= 0; i--)
            {
                EmitKeyDownEvent(CreateKeyboardEvent(keysPressed[i]));
            }

            //key up
            for (int i = _pressedKeys.Count - 1; i >= 0; i--)
            {
                if (keysPressed.Contains(_pressedKeys[i])) continue;
                
                EmitKeyUpEvent(CreateKeyboardEvent(_pressedKeys[i]));
                
                _pressedKeys.Remove(_pressedKeys[i]);
            }

        }

        /// <summary>
        /// Takes a certain keyboard key and returns it as a string.
        /// </summary>
        /// <param name="keyStatus"></param>
        /// <param name="outputString"></param>
        /// <returns></returns>
        public static string KeyToString(KeyboardStatus keyStatus, string outputString)
        {
            //currently pressed key
            Keys key = keyStatus.KeyEnum;

            switch (key)
            {
                //Backspace
                case Keys.Back when outputString.Length > 0:
                    return outputString.Remove(outputString.Length - 1, 1);
                //Space
                case Keys.Space:
                    return outputString.Insert(outputString.Length, " ");
            }

            //Caps lock or shift
            if (key == Keys.LeftShift || key == Keys.RightShift || keyStatus.KeyboardState.CapsLock)
                //Is Letter and make Uppercase
                if (key.ToString().Length == 1)
                    return outputString.Insert(outputString.Length, key.ToString().ToUpper());

            //Is letter and make Lowercase
            return key.ToString().Length == 1 ? outputString.Insert(outputString.Length, key.ToString().ToLower()) : outputString;

            //Unknown character so lets return original string
        }

        private void EmitKeyPressEvent(InputEvent eventObj)
        {
            _eventManager.Emit("keypress", eventObj);
        }

        private void EmitKeyDownEvent(InputEvent eventObj)
        {
            _eventManager.Emit("keydown", eventObj);
        }

        private void EmitKeyUpEvent(InputEvent eventObj)
        {
            _eventManager.Emit("keyup", eventObj);
        }

        private InputEvent CreateKeyboardEvent(Keys key)
        {
            //Fill in mouse event state
            MouseState mouseState = Mouse.GetState();
            
            return new InputEvent
            {
                Keyboard = new KeyboardStatus
                {
                    KeyEnum = key,
                    KeyboardState = _keyboardState
                },
                Mouse = new MouseStatus
                {
                    X = mouseState.X,
                    Y = mouseState.Y,
                    LeftButton = mouseState.LeftButton,
                    RightButton = mouseState.RightButton,
                    MiddleButton = mouseState.MiddleButton,
                    MouseState = mouseState
                }
            };
        }
    }
}
