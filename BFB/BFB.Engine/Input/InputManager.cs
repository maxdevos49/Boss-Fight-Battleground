using BFB.Engine.Event;

namespace BFB.Engine.Input
{
    public class InputManager
    {

        private readonly MouseInput _mouseInput;
        private readonly KeyboardInput _keyboardInput;

        public InputManager(EventManager<InputEvent> eventManager)
        {
            _mouseInput = new MouseInput(eventManager);

            _keyboardInput = new KeyboardInput(eventManager);
        }

        public void CheckInputs()
        {
            //Mouse input
            _mouseInput.UpdateMouse();

            //Keyboard input
            _keyboardInput.UpdateKeyboard();
        }
    }
}
