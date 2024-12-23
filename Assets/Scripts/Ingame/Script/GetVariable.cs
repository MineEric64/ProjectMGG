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
        var glocal = IngameManagerV2.GetVariable(Name, ref IngameManagerV2.Local.Others, ref IngameManagerV2.Global.Others);

        if (glocal != null) return glocal;
        if (Interpreter.FunctionTable.ContainsKey(Name))
        {
            return Interpreter.FunctionTable[Name];
        }
        return null;
    }
}
