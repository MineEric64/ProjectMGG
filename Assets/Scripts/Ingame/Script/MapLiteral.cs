using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLiteral : IExpression
{
    public Dictionary<string, IExpression> Values { get; set; } = new Dictionary<string, IExpression>();

    public void Print(int depth)
    {
        Debug.Log("Script/Print: " + new string(' ', depth * 2));
        Debug.Log("Script/Print: {");

        foreach (string key in Values.Keys)
        {
            Debug.Log("Script/Print: " + key + ": ");
            Values[key].Print(depth + 1);
        }
        Debug.Log("Script/Print: " + new string(' ', depth * 2));
        Debug.Log("Script/Print: }");
    }

    public object Interpret()
    {
        Dictionary<string, object> result = new Dictionary<string, object>();
        foreach (string key in Values.Keys)
        {
            result[key] = Values[key].Interpret();
        }
        return result;
    }
}
