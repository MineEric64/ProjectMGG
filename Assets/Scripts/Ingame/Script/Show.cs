using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Show : IStatement
{
    public string Argument { get; set; }

    public void Print(int depth)
    {
        Debug.Log("Script/Print: " + new string(' ', depth * 2));
        Debug.Log("Script/Print: SHOW");
    }

    public void Interpret()
    {
        
    }
}
