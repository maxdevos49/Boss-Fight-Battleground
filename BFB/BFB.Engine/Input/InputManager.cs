using BFB.Engine.Event;

namespace BFB.Engine.Input
{
    public class InputManager
    {
        private readonly InputConfig _configuration;

        private readonly MouseInput _mouseInput;
        private readonly KeyboardInput _keyboardInput;

        public InputManager(EventManager<InputEvent> eventManager, InputConfig configuration)
        {
            _configuration = configuration;

            if (_configuration.CaptureMouse)
                _mouseInput = new MouseInput(eventManager);

            if (_configuration.CaptureKeyboard)
                _keyboardInput = new KeyboardInput(eventManager);
        }

        public void CheckInputs()
        {
            //Mouse input
            if (_configuration.CaptureMouse)
                _mouseInput.UpdateMouse();

            //Keyboard input
            if (_configuration.CaptureKeyboard)
                _keyboardInput.UpdateKeyboard();
        }
    }
}
