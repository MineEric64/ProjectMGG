using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MessagePack;

[MessagePackObject]
public class SettingsObject
{
    [Key("ui")]
    public SettingUI UI { get; set; }

    [Key("debug")]
    public bool Debug { get; set; } = false;
}
