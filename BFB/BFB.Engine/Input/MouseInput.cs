//Monogame
using Microsoft.Xna.Framework.Input;

//Engine
using BFB.Engine.Event;

namespace BFB.Engine.Input
{
    /// <summary>
    /// Keeps track of the mouse input.
    /// </summary>
    public class MouseInput
    {

        private readonly MouseStatus _mouseStatus;
        
        private readonly EventManager<InputEvent> _eventManager;

        public MouseInput(EventManager<InputEvent> eventManager)
        {
            _eventManager = eventManager;

            //initial
            _mouseStatus = new MouseStatus
            {
                X = 0,
                Y = 0,
                LeftButton = ButtonState.Released,
                RightButton = ButtonState.Released,
                MiddleButton = ButtonState.Released,
                MouseState = Mouse.GetState()
            };
        }

        /// <summary>
        /// Updates the mouse state and emits any button states.
        /// </summary>
        public void UpdateMouse()
        {
            MouseState mouseState = Mouse.GetState();

            if(_mouseStatus.VerticalScroll != mouseState.ScrollWheelValue || _mouseStatus.HorizontalScroll != mouseState.HorizontalScrollWheelValue)
                EmitMouseScroll(mouseState);
            
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
            _mouseStatus.VerticalScroll = mouseState.ScrollWheelValue;
            _mouseStatus.VerticalScrollAmount = mouseState.ScrollWheelValue - _mouseStatus.VerticalScroll;
            _mouseStatus.HorizontalScroll = mouseState.HorizontalScrollWheelValue;
            _mouseStatus.HorizontalScrollAmount = mouseState.HorizontalScrollWheelValue - _mouseStatus.HorizontalScroll;
            _mouseStatus.MouseState = mouseState;

        }

        /// <summary>
        /// Emitted whenever the mouse position is changed
        /// </summary>
        /// <param name="mouseState"></param>
        private void EmitMouseMovedEvent(MouseState mouseState)
        {
            _eventManager.Emit("mousemove", GetMouseEventPayload(mouseState));
        }

        /// <summary>
        /// Emitted whenever any mouse button is down
        /// </summary>
        /// <param name="mouseState"></param>
        private void EmitMousePressed(MouseState mouseState)
        {
            _eventManager.Emit("mousedown", GetMouseEventPayload(mouseState));
        }

        /// <summary>
        /// Emitted whenever any mouse button is released
        /// </summary>
        /// <param name="mouseState"></param>
        private void EmitMouseReleased(MouseState mouseState)
        {
            _eventManager.Emit("mouseup", GetMouseEventPayload(mouseState));
        }

        /// <summary>
        ///  Emitted whenever any button is pressed
        /// </summary>
        /// <param name="mouseState"></param>
        private void EmitMouseClick(MouseState mouseState)
        {
            _eventManager.Emit("mouseclick", GetMouseEventPayload(mouseState));
        }

        private void EmitMouseScroll(MouseState mouseState)
        {
            _eventManager.Emit("mousescroll", GetMouseEventPayload(mouseState));
        }

        private InputEvent GetMouseEventPayload(MouseState mouseState)
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
                    VerticalScroll = mouseState.ScrollWheelValue,
                    VerticalScrollAmount = mouseState.ScrollWheelValue - _mouseStatus.VerticalScroll,
                    HorizontalScroll =  mouseState.HorizontalScrollWheelValue,
                    HorizontalScrollAmount = mouseState.ScrollWheelValue - _mouseStatus.HorizontalScroll,
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
