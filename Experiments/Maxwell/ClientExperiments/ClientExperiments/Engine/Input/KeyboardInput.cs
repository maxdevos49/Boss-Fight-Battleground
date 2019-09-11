//C#
using System.Linq;
using System.Collections.Generic;

//Monogame
using Microsoft.Xna.Framework.Input;

//Engine
using ClientExperiments.Engine.Event;

namespace ClientExperiments.Engine.Input
{
    public class KeyboardInput
    {

        public readonly EventManager _eventManager;

        public List<Keys> PressedKeys;
        public KeyboardState KeyboardState;

        public KeyboardInput(EventManager eventManager)
        {
            _eventManager = eventManager;

            PressedKeys = new List<Keys>();

        }

        public void UpdateKeyboard()
        {
            KeyboardState = Keyboard.GetState();


            var keysPressed = KeyboardState.GetPressedKeys().OfType<Keys>().ToList();


            //key press
            for (int i = keysPressed.Count - 1; i >= 0; i--)
            {
                if (!PressedKeys.Contains(keysPressed[i]))
                {
                    EmitKeyPressEvent(CreateKeyboardEvent(keysPressed[i]));
                    PressedKeys.Add(keysPressed[i]);
                }
            }


            //key down
            for (int i = keysPressed.Count - 1; i >= 0; i--)
            {
                EmitKeyDownEvent(CreateKeyboardEvent(keysPressed[i]));
            }

            //key up
            for (int i = PressedKeys.Count - 1; i >= 0; i--)
            {
                if (!keysPressed.Contains(PressedKeys[i]))
                {
                    EmitKeyUpEvent(CreateKeyboardEvent(PressedKeys[i]));
                    PressedKeys.Remove(PressedKeys[i]);
                }
            }

        }

        public static string KeyToString(KeyboardEvent keyEvent, string outputString)
        {
            //currently pressed key
            Keys key = keyEvent.KeyEnum;

            //Backspace
            if (key == Keys.Back && outputString.Length > 0)
                return outputString.Remove(outputString.Length - 1, 1);

            //Space
            if (key == Keys.Space)
                return outputString.Insert(outputString.Length, " ");

            //Caps lock or shift
            if (key == Keys.LeftShift || key == Keys.RightShift || keyEvent.KeyboardState.CapsLock)
                //Is Letter and make Uppercase
                if (key.ToString().Length == 1)
                    return outputString.Insert(outputString.Length, key.ToString().ToUpper());

            //Is letter and make Lowercase
            if(key.ToString().Length == 1)
                return outputString.Insert(outputString.Length, key.ToString().ToLower());

            //Unknown character so lets return original string
            return outputString;
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
                    KeyboardState = KeyboardState
                }
            };
        }
    }
}
