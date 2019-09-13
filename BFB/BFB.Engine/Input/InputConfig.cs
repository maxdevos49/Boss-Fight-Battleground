namespace BFB.Engine.Input
{
    public class InputConfig
    {
        public InputConfig()
        {
            //Defaults
            CaptureMouse = true;
            CaptureKeyboard = true;
        }
        /**
         * Either emit all mouse events or not
         * */
        public bool CaptureMouse;

        /**
         * Either emit all keyboard events or not
         * */
        public bool CaptureKeyboard;


    }
}
