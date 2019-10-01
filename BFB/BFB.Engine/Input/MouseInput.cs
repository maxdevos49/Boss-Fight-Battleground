//Monogame
using Microsoft.Xna.Framework.Input;

//Engine
using BFB.Engine.Event;

namespace BFB.Engine.Input
{
    public class MouseInput
    {

        private readonly EventManager _eventManager;

        private int _x;
        private int _y;
        private ButtonState _leftButton;
        private ButtonState _rightButton;
        private ButtonState _middleButton;

        public MouseInput(EventManager eventManager)
        {
            _eventManager = eventManager;

            //Defaults
            _x = 0;
            _y = 0;
            _leftButton = ButtonState.Released;
            _rightButton = ButtonState.Released;
            _middleButton = ButtonState.Released;
        }

        public void UpdateMouse()
        {
            MouseState mouseState = Mouse.GetState();

            //Mouse move
            if (_x != mouseState.X || _y != mouseState.Y)
                EmitMouseMovedEvent(mouseState);

            //Mouse click
            if ((_leftButton == ButtonState.Released && mouseState.LeftButton == ButtonState.Pressed)
                || (_rightButton == ButtonState.Released && mouseState.RightButton == ButtonState.Pressed)
                || (_middleButton == ButtonState.Released && mouseState.MiddleButton == ButtonState.Pressed))
                EmitMouseClick(mouseState);

            //Mouse down
            if (mouseState.RightButton == ButtonState.Pressed
                || mouseState.LeftButton == ButtonState.Pressed
                || mouseState.MiddleButton == ButtonState.Pressed)
                EmitMousePressed(mouseState);

            //Mouse released
            if ((_leftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released)
                || (_rightButton == ButtonState.Pressed && mouseState.RightButton == ButtonState.Released)
                || (_middleButton == ButtonState.Pressed && mouseState.MiddleButton == ButtonState.Released))
                EmitMouseReleased(mouseState);

            //Save value so we know when changes happened
            _x = mouseState.X;
            _y = mouseState.Y;
            _leftButton = mouseState.LeftButton;
            _rightButton = mouseState.RightButton;
            _middleButton = mouseState.MiddleButton;

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

        private static Event.Event GetMouseEventPayload(MouseState mouseState)
        {

            Event.Event eventData = new Event.Event
            {
                Mouse = new MouseEvent
                {
                    X = mouseState.X,
                    Y = mouseState.Y,
                    LeftButton = mouseState.LeftButton,
                    RightButton = mouseState.RightButton,
                    MiddleButton = mouseState.MiddleButton
                }
            };

            return eventData;
        }

    }
}
