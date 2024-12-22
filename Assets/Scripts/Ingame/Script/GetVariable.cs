using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetVariable : IExpression
{
    public string Name { get; set; }

    public void Print(int depth)
    {
        Debug.Log("Script/Print: " + new string(' ', depth * 2));
        Debug.Log("Script/Print: GET_VARIABLE: " + Name);
    }

    public object Interpret()
    {
        foreach (Dictionary<string, object> variables in Interpreter.Local[Interpreter.Local.Count - 1])
        {
            if (variables.ContainsKey(Name))
            {
                return variables[Name];
            }
        }
        if (Interpreter.Global.ContainsKey(Name))
        {
            return Interpreter.Global[Name];
        }
        if (Interpreter.FunctionTable.ContainsKey(Name))
        {
            return Interpreter.FunctionTable[Name];
        }
        return null;
    }
}
