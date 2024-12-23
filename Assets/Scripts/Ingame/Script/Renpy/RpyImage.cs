using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class RpyImage : IStatement
{
    public string Name { get; set; }
    public string Argument { get; set; }
    public bool IsGlobal { get; set; } = false;

    public void Print(int depth)
    {
        Debug.Log("Script/Print: " + new string(' ', depth * 2));
        Debug.Log("Script/Print: IMAGE " + Argument);
    }

    public void Interpret()
    {
        var vars = IsGlobal ? IngameManagerV2.Global : IngameManagerV2.Local;
        vars.Images.Add(Name, Argument);
    }
}
