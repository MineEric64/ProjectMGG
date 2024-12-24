using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class transform
{
    //deprecated
    [Obsolete()] public Dictionary<string, float> Options = new Dictionary<string, float>();

    public float xalign = 0.5f;
    public float yalign = 0.5f;
    public float zoom  = 1f;

    public transform()
    {
        
    }
}
