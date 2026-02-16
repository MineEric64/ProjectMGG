using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MessagePack;

namespace ProjectMGG.Settings
{
    [MessagePackObject]
    public class SettingAudio
    {
        [Key("master_volume")]
        public float MasterVolume { get; set; } = 1f;

        [Key("music_volume")]
        public float MusicVolume { get; set; } = 1f;

        [Key("sound_volume")]
        public float SoundVolume { get; set; } = 1f;
    }
}
