using System;
using BFB.Engine.Content;
using JetBrains.Annotations;
using Microsoft.Xna.Framework.Audio; 
using Microsoft.Xna.Framework.Media;

namespace BFB.Engine.Audio
{
     public class AudioManager
    {
        private readonly BFBContentManager _content; 
        private Song _curSong;
        private SoundEffect _curSoundEffect;

        public AudioManager(BFBContentManager audio)
        {
            _content = audio;
        }

        public void PlaySong(string song)
        {
            _curSong = _content?.GetSongAudio(song);
            MediaPlayer.Play(_curSong);
            SetSongRepeating(true);
        }

        public void PlaySoundEffect(string soundEffect)
        {
            _curSoundEffect = _content.GetSoundEffectAudio(soundEffect);
//            _curSoundEffect.CreateInstance()
            _curSoundEffect.Play();
        }

        public void StopSong()
        {
            try
            {
                MediaPlayer.Stop();
            }
            catch (Exception)
            {
                //Do nothing
            }
        }

        [UsedImplicitly]
        public void SetSongRepeating(bool isRepeating)
        {
            MediaPlayer.IsRepeating = isRepeating;
        }

        public bool IsSongRepeating()
        {
            return MediaPlayer.IsRepeating;
        }

        public void SetSongVolume(float volume)
        {
            MediaPlayer.Volume = volume;
        }

        public float GetSongVolume()
        {
            return MediaPlayer.Volume;
        }

        public void SetSongMuted(bool isMuted)
        {
            MediaPlayer.IsMuted = isMuted;
        }

        public bool IsSongMuted()
        {
            return MediaPlayer.IsMuted;
        }

    }
}
