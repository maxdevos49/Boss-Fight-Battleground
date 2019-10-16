namespace BFB.Engine.Entity
{
    public class PlayerInput
    {
        /*
         * This class will handle the input that the player gives
         * Transform mouse & keyboard input to game controls
         */
        private PlayerState playerState;
        
        public PlayerInput()
        {
            
        }

        
        
        public PlayerState GetPlayerState()
        {
            return playerState;
        }
        //private getKey
    }
}