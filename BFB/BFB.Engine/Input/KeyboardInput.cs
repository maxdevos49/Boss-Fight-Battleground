//C#
using System.Linq;
using System.Collections.Generic;

//Monogame
using Microsoft.Xna.Framework.Input;

//Engine
using BFB.Engine.Event;

namespace BFB.Engine.Input
{
    public class KeyboardInput
    {
        private readonly EventManager _eventManager;

        private readonly List<Keys> _pressedKeys;
        private KeyboardState _keyboardState;

        public KeyboardInput(EventManager eventManager)
        {
            _eventManager = eventManager;
            _pressedKeys = new List<Keys>();
        }

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

        public static string KeyToString(KeyboardEvent keyEvent, string outputString)
        {
            //currently pressed key
            Keys key = keyEvent.KeyEnum;

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
            if (key == Keys.LeftShift || key == Keys.RightShift || keyEvent.KeyboardState.CapsLock)
                //Is Letter and make Uppercase
                if (key.ToString().Length == 1)
                    return outputString.Insert(outputString.Length, key.ToString().ToUpper());

            //Is letter and make Lowercase
            return key.ToString().Length == 1 ? outputString.Insert(outputString.Length, key.ToString().ToLower()) : outputString;

            //Unknown character so lets return original string
        }

        private void EmitKeyPressEvent(Event.Event eventObj)
        {
            _eventManager.Emit("keypress", eventObj);
        }

        private void EmitKeyDownEvent(Event.Event eventObj)
        {
            _eventManager.Emit("keydown", eventObj);
        }

        private void EmitKeyUpEvent(Event.Event eventObj)
        {
            _eventManager.Emit("keyup", eventObj);
        }

        private Event.Event CreateKeyboardEvent(Keys key)
        {
            return new Event.Event
            {
                Keyboard = new KeyboardEvent
                {
                    Key = key.ToString(),
                    KeyEnum = key,
                    KeyboardState = _keyboardState
                }
            };
        }
    }
}
