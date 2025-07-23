using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MessagePack;

namespace ProjectMGG.Settings
{
    [MessagePackObject]
    public class SettingUI
    {
        [Key("cps")]
        public float CPS { get; set; } = 25f;

        [Key("text_ease")]
        public string TextEase { get; set; } = "Linear";
    }
}
