using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Show : IStatement
{
    public List<IExpression> Arguments { get; set; } = new List<IExpression>();

    public void Print(int depth)
    {
        Debug.Log("Script/Print: " + new string(' ', depth * 2));
        Debug.Log("Script/Print: SHOW");

        foreach (IExpression node in Arguments)
        {
            node.Print(depth + 1);
        }
    }

    public void Interpret()
    {
        foreach (IExpression node in Arguments)
        {
            object value = node.Interpret();
            Debug.Log("Script/Print: " + value);
        }
    }
}
