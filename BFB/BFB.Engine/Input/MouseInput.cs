//Monogame
using Microsoft.Xna.Framework.Input;

//Engine
using BFB.Engine.Event;

namespace BFB.Engine.Input
{
    public class MouseInput
    {

        private readonly EventManager _eventManager;

        private int X;

        private int Y;

        private ButtonState LeftButton;

        private ButtonState RightButton;

        private ButtonState MiddleButton;

        public MouseInput(EventManager eventmanager)
        {
            _eventManager = eventmanager;

            //Defaults
            X = 0;
            Y = 0;
            LeftButton = ButtonState.Released;
            RightButton = ButtonState.Released;
            MiddleButton = ButtonState.Released;
        }

        public void UpdateMouse()
        {
            MouseState mouseState = Mouse.GetState();

            //Mouse move
            if (X != mouseState.X || Y != mouseState.Y)
                EmitMouseMovedEvent(mouseState);

            //Mouse click
            if ((LeftButton == ButtonState.Released && mouseState.LeftButton == ButtonState.Pressed)
                || (RightButton == ButtonState.Released && mouseState.RightButton == ButtonState.Pressed)
                || (MiddleButton == ButtonState.Released && mouseState.MiddleButton == ButtonState.Pressed))
                EmitMouseClick(mouseState);

            //Mouse down
            if (mouseState.RightButton == ButtonState.Pressed
                || mouseState.LeftButton == ButtonState.Pressed
                || mouseState.MiddleButton == ButtonState.Pressed)
                EmitMousePressed(mouseState);

            //Mouse released
            if ((LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released)
                || (RightButton == ButtonState.Pressed && mouseState.RightButton == ButtonState.Released)
                || (MiddleButton == ButtonState.Pressed && mouseState.MiddleButton == ButtonState.Released))
                EmitMouseReleased(mouseState);

            //Save value so we know when changes happened
            X = mouseState.X;
            Y = mouseState.Y;
            LeftButton = mouseState.LeftButton;
            RightButton = mouseState.RightButton;
            MiddleButton = mouseState.MiddleButton;

        }

        /**
         * Emited whenever the mouse position is changed
         * */
        private void EmitMouseMovedEvent(MouseState mouseState)
        {
            _eventManager.Emit("mousemove", GetMouseEventPayload(mouseState));
        }

        /**
         * Emited whenever any mouse button is down
         * */
        private void EmitMousePressed(MouseState mouseState)
        {
            _eventManager.Emit("mousedown", GetMouseEventPayload(mouseState));
        }

        /**
         * Emited whenever any mouse button is released
         * */
        private void EmitMouseReleased(MouseState mouseState)
        {
            _eventManager.Emit("mouseup", GetMouseEventPayload(mouseState));
        }

        /**
         * Emited whenever any button is pressed
         * */
        private void EmitMouseClick(MouseState mouseState)
        {
            _eventManager.Emit("mouseclick", GetMouseEventPayload(mouseState));
        }

        private Event.Event GetMouseEventPayload(MouseState mouseState)
        {

            Event.Event eventdata = new Event.Event
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

            return eventdata;
        }

    }
}
