using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class transform : IStatement
{
    //deprecated
    [Obsolete()] public Dictionary<string, float> Options = new Dictionary<string, float>();

    public string Name { get; set; }
    public bool IsGlobal { get; set; } = false;

    public float xalign = 0.5f;
    public float yalign = 0.5f;
    public float zoom  = 1f;

    public transform()
    {
        
    }

    public void Print(int depth)
    {

    }

    public void Interpret()
    {
        var vars = IsGlobal ? IngameManagerV2.Global : IngameManagerV2.Local;
        vars.Transforms.Add(Name, this);
    }
}
