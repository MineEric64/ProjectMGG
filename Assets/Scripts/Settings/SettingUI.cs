using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MessagePack;

[MessagePackObject]
public class SettingUI
{
    [Key("text_ease")]
    public string TextEase { get; set; } = "Linear";
}
