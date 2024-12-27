using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reeverb : IStatement
{
    public void Print(int depth)
    {
        Debug.Log("Script/Print: " + new string(' ', depth * 2));
        Debug.Log("Script/Print: REEVERB");
    }

    public void Interpret()
    {
        IngameManagerV2.Instance.IsReeverb = true;
    }
}
