using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace BFB.Engine.Entity.Components.Graphics
{
    public class AnimationComponent
    {
        private int _currentFrame;
        private int _totalFrames;
        private int _lowerFrameBound;
        private int _upperFrameBound;
        private readonly Dictionary<string, object> _animationStates;
        public string CurrentState { get; private set; }

        public bool IsPlaying { get; private set; }
        public bool IsLooping { get; private set; }
        public bool IsPaused { get; private set; }

        public int Rows { get; set; }
        public int Columns { get; set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        private readonly int _framesPerSecond; // Technically it isn't FramesPerSecond. The formula in the update function with timeLeft needs to be altered.
        private int _timeLeft;                 // It is misleading because a lower FramesPerSecond variable leads to an actually faster cycle through the animation.

        public AnimationComponent()
        {
            Rows = 5; // Hard coded values for the sake of not reading in from JSON. Will be changed later.
            Columns = 9;

            _framesPerSecond = 2;
            _timeLeft = 0;

            _currentFrame = 0;
            _totalFrames = Rows * Columns;

            _lowerFrameBound = 0;
            _upperFrameBound = 8;

            _animationStates = null;

            IsLooping = true;
            IsPaused = false;

            Play("DEFAULT");
        }

        public void Play(string key)
        {
            if (IsPaused)
            {
                IsPaused = false;
                return;
            }

            IsPlaying = true;
            CurrentState = key;
        }


        public void Stop()
        {
            IsPlaying = false;
            _currentFrame = 0;
        }

        public void Pause()
        {
            IsPaused = true;
        }

        public void Update(Entity entity)
        {
            _timeLeft -= 1;
            if (IsPlaying && !IsPaused && _timeLeft <= 0)
            {
                _currentFrame += 1;
                if (_currentFrame >= _upperFrameBound)
                    _currentFrame = _lowerFrameBound;
                _timeLeft = _framesPerSecond;
            }
        }

        public void Draw(Entity entity)
        {
            if (IsPlaying && !IsPaused)
            {
                Width = (int) (entity.Position.X / Columns);
                Height = (int) (entity.Position.Y / Rows);

                int currentRow = (int) ((float) _currentFrame / (float) Columns);
                int currentCol = _currentFrame % Columns;

                entity.DrawRectangle =
                    new Rectangle(Width * currentCol, Height * currentRow + 1, Width - 1, Height - 1);
            }
        }
    }
}
