using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Obsolete()]
public class transform
{
    public Dictionary<string, float> Options { get; set; }

    public transform()
    {
        Options = new Dictionary<string, float>();
    }
}
