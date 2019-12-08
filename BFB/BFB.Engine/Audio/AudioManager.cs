using BFB.Engine.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace BFB.Engine.Audio
{
    class AudioManager
    {
        private BFBContentManager _content;
        private Song curSong;
        private SoundEffect curSoundEffect;

        public AudioManager(BFBContentManager audio)
        {
            _content = audio;
        }

        public void playSong(string song)
        {
            curSong = _content.GetSongAudio(song);
            MediaPlayer.Play(curSong);
            MediaPlayer.IsRepeating = true;
        }

        public void playSoundEffect(string soundEffect)
        {
            curSoundEffect = _content.GetSoundEffectAudio(soundEffect);
            curSoundEffect.Play();
        }

        public void stopSong(string song)
        {
            MediaPlayer.Stop();
        }

        public void stopSoundEffect(string soundEffect)
        {

        }


    }
}
