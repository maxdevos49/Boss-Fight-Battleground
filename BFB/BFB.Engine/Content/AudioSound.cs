using System;
using System.Collections.Generic;
using System.Text;

namespace BFB.Engine.Content
{
    public class AudioSound
    {
        public AudioType SoundType { get; set; }

        public string Location { get; set; }
    }

    public enum AudioType
    {
        SoundEffect,
        Song
    }
}
