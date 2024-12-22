using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetVariable : IExpression
{
    public string Name { get; set; }
    public IExpression Value { get; set; }

    public void Print(int depth)
    {
        Debug.Log("Script/Print: " + new string(' ', depth * 2));
        Debug.Log("Script/Print: SET_VARIABLE: " + Name);
        Value.Print(depth + 1);
    }

    public object Interpret()
    {
        foreach (Dictionary<string, object> variables in Interpreter.Local[Interpreter.Local.Count - 1])
        {
            if (variables.ContainsKey(Name))
            {
                variables[Name] = Value.Interpret();
                return variables[Name];
            }
        }

        Interpreter.Global[Name] = Value.Interpret();
        return Interpreter.Global[Name];
    }

}
