using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringLiteral : IExpression
{
    public string Value { get; set; }

    public void Print(int depth)
    {
        Debug.Log("Script/Print: " + new string(' ', depth * 2));
        Debug.Log("Script/Print: \"" + Value + "\"");
    }

    public object Interpret()
    {
        return Value;
    }
}
