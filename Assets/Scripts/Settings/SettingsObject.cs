using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MessagePack;

[MessagePackObject]
public class SettingsObject
{
    [Key("ui")]
    public SettingUI UI { get; set; }

    [Key("is_debug")]
    public bool IsDebug { get; set; } = false;
}
