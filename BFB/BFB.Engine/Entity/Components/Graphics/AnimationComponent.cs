//C#
using System.Collections.Generic;
//Monogame
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace BFB.Engine.Entity.Components.Graphics
{
    public class AnimationComponent : IGraphicsComponent
    {
        private int _currentFrame;
        private int _totalFrames;
        
        private readonly int _lowerFrameBound;
        private readonly int _upperFrameBound;
        
        private readonly Dictionary<string, object> _animationStates;
        private string _currentState;

        private bool _isPlaying;
        private bool _isLooping;
        private bool _isPaused;
        private readonly int _rows;
        private readonly int _columns;
        
        private Rectangle _drawRectangle;
        private readonly Texture2D _texture;

        private readonly int _framesPerSecond; // Technically it isn't FramesPerSecond. The formula in the update function with timeLeft needs to be altered.
        private int _timeLeft;                 // It is misleading because a lower FramesPerSecond variable leads to an actually faster cycle through the animation.

        public AnimationComponent(Texture2D texture)
        {
            _texture = texture;//should be reference
            
            _rows = 1; // Hard coded values for the sake of not reading in from JSON. Will be changed later.
            _columns = 1;
            
            _timeLeft = 0;
            _framesPerSecond = 10;
            _currentFrame = 0;
            
            _totalFrames = _rows * _columns;

            _animationStates = null;
            _isLooping = true;
            _isPaused = false;
            
            //rectangle selection size
            _drawRectangle.Width = _texture.Width / _columns;
            _drawRectangle.Height = _texture.Height / _rows;
            
            //temp default sprite bounds
            _lowerFrameBound = 0;
            _upperFrameBound = 8;
            
            Play("DEFAULT");
        }

        private void Play(string key)
        {
            if (_isPaused)
            {
                _isPaused = false;
                return;
            }

            _isPlaying = true;
            _currentState = key;
        }


        private void Stop()
        {
            _isPlaying = false;
            _currentFrame = 0;
        }

        private void Pause()
        {
            _isPaused = true;
        }

        public void Update(ClientEntity entity)
        {
            //check if we are animating or not
            if (!_isPlaying || _isPaused) return;

            //count down until next frame
            _timeLeft -= 1;
            
            //Check if ready to switch frame
            if (_timeLeft > 0) return;
            
            //reset countdown counter
            _timeLeft = _framesPerSecond;

            //increment frame number
            _currentFrame += 1;
            
            //check frame number bounds
            if (_currentFrame >= _upperFrameBound)
                _currentFrame = _lowerFrameBound;
            
            //Calculate the frame row and column to select
            int currentRow = _currentFrame /_columns;
            int currentCol = _currentFrame % _columns;

            //Move rectangle select box to correct frame
            _drawRectangle.X = _drawRectangle.Width * currentCol;
            _drawRectangle.Y = _drawRectangle.Height * currentRow;
        }

        public void Draw(ClientEntity entity, SpriteBatch graphics)
        {
            graphics.Draw(_texture,
                entity.Position.ToVector2(),
                _drawRectangle,
                Color.White,
                entity.Rotation,
                new Vector2((float)_drawRectangle.Width/2, (float)_drawRectangle.Width/2),
                3.0f,
                SpriteEffects.None,
                1);
        }
    }
}
