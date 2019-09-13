using BFB.Engine.Event;

namespace BFB.Engine.Input
{
    public class InputManager
    {
        private readonly EventManager _eventManager;
        private readonly InputConfig _configuration;

        private readonly MouseInput MouseInput;
        private readonly KeyboardInput KeyboardInput;

        public InputManager(EventManager eventManager, InputConfig configuration)
        {
            _eventManager = eventManager;
            _configuration = configuration;

            if (_configuration.CaptureMouse)
                MouseInput = new MouseInput(_eventManager);

            if (_configuration.CaptureKeyboard)
                KeyboardInput = new KeyboardInput(_eventManager);

        }

        public void CheckInputs()
        {
            //Mouse input
            if (_configuration.CaptureMouse)
                MouseInput.UpdateMouse();

            //Keyboard input
            if (_configuration.CaptureKeyboard)
                KeyboardInput.UpdateKeyboard();
        }
    }
}
