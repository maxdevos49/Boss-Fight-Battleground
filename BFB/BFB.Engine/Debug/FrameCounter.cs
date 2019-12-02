//C#
using System.Collections.Generic;
using System.Linq;

//https://stackoverflow.com/questions/20676185/xna-monogame-getting-the-frames-per-second

namespace BFB.Engine.Debug
{
    /// <summary>
    /// FrameCounter has the ability to count the frames the BFB.Client is running at
    /// </summary>
    public class FrameCounter
    {
        private long TotalFrames { get; set; }
        private float TotalSeconds { get; set; }
        public float AverageFramesPerSecond { get; private set; }
        private float CurrentFramesPerSecond { get; set; }

        private const int MaximumSamples = 100;

        private readonly Queue<float> _sampleBuffer = new Queue<float>();

        /// <summary>
        /// Updates the framerate everyt
        /// </summary>
        /// <param name="deltaTime">The change of time used to calculate FPS</param>
        /// <returns></returns>
        public bool Update(float deltaTime)
        {
            CurrentFramesPerSecond = 1.0f / deltaTime;

            _sampleBuffer.Enqueue(CurrentFramesPerSecond);

            if (_sampleBuffer.Count > MaximumSamples)
            {
                _sampleBuffer.Dequeue();
                AverageFramesPerSecond = _sampleBuffer.Average(i => i);
            }
            else
            {
                AverageFramesPerSecond = CurrentFramesPerSecond;
            }

            TotalFrames++;
            TotalSeconds += deltaTime;
            return true;
        }
    }
}
