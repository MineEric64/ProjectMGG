using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RpyComment : IStatement
{
    public string Content { get; set; } = string.Empty;

    public void Print(int depth)
    {

    }

    public void Interpret()
    {
        Debug.Log("Script/Interpret: # " + Content);
    }
}
