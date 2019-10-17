//Monogame
using Microsoft.Xna.Framework.Input;

//Engine
using BFB.Engine.Event;

namespace BFB.Engine.Input
{
    public class MouseInput
    {

        private readonly EventManager<InputEvent> _eventManager;

        private readonly MouseStatus _mouseStatus;

        public MouseInput(EventManager<InputEvent> eventManager)
        {
            _eventManager = eventManager;

            _mouseStatus = new MouseStatus
            {
                X = 0,
                Y = 0,
                LeftButton = ButtonState.Released,
                RightButton = ButtonState.Released,
                MiddleButton = ButtonState.Released
            };
            //Defaults
        }

        public void UpdateMouse()
        {
            MouseState mouseState = Mouse.GetState();

            //Mouse move
            if (_mouseStatus.X != mouseState.X || _mouseStatus.Y != mouseState.Y)
                EmitMouseMovedEvent(mouseState);

            //Mouse click
            if ((_mouseStatus.LeftButton == ButtonState.Released && mouseState.LeftButton == ButtonState.Pressed)
                || (_mouseStatus.RightButton == ButtonState.Released && mouseState.RightButton == ButtonState.Pressed)
                || (_mouseStatus.MiddleButton == ButtonState.Released && mouseState.MiddleButton == ButtonState.Pressed))
                EmitMouseClick(mouseState);

            //Mouse down
            if (mouseState.RightButton == ButtonState.Pressed
                || mouseState.LeftButton == ButtonState.Pressed
                || mouseState.MiddleButton == ButtonState.Pressed)
                EmitMousePressed(mouseState);

            //Mouse released
            if ((_mouseStatus.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released)
                || (_mouseStatus.RightButton == ButtonState.Pressed && mouseState.RightButton == ButtonState.Released)
                || (_mouseStatus.MiddleButton == ButtonState.Pressed && mouseState.MiddleButton == ButtonState.Released))
                EmitMouseReleased(mouseState);

            //Save value so we know when changes happened
            _mouseStatus.X = mouseState.X;
            _mouseStatus.Y = mouseState.Y;
            _mouseStatus.LeftButton = mouseState.LeftButton;
            _mouseStatus.RightButton = mouseState.RightButton;
            _mouseStatus.MiddleButton = mouseState.MiddleButton;

        }

        /**
         * Emitted whenever the mouse position is changed
         * */
        private void EmitMouseMovedEvent(MouseState mouseState)
        {
            _eventManager.Emit("mousemove", GetMouseEventPayload(mouseState));
        }

        /**
         * Emitted whenever any mouse button is down
         * */
        private void EmitMousePressed(MouseState mouseState)
        {
            _eventManager.Emit("mousedown", GetMouseEventPayload(mouseState));
        }

        /**
         * Emitted whenever any mouse button is released
         * */
        private void EmitMouseReleased(MouseState mouseState)
        {
            _eventManager.Emit("mouseup", GetMouseEventPayload(mouseState));
        }

        /**
         * Emitted whenever any button is pressed
         * */
        private void EmitMouseClick(MouseState mouseState)
        {
            _eventManager.Emit("mouseclick", GetMouseEventPayload(mouseState));
        }

        private static InputEvent GetMouseEventPayload(MouseState mouseState)
        {

            return new InputEvent()
            {
                Mouse = new MouseStatus
                {
                    X = mouseState.X,
                    Y = mouseState.Y,
                    LeftButton = mouseState.LeftButton,
                    RightButton = mouseState.RightButton,
                    MiddleButton = mouseState.MiddleButton,
                    MouseState = mouseState
                },
                Keyboard = new KeyboardStatus
                {
                    KeyEnum = Keys.None,
                    KeyboardState = Keyboard.GetState()
                }
            };

        }

    }
}
