using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RpyPlay : IStatement
{
    public string Channel { get; set; }
    public string Path { get; set; }

    public void Print(int depth)
    {

    }

    public void Interpret()
    {
        IngameManagerV2.Instance.LetsPlay(Channel, Path);
    }
}
