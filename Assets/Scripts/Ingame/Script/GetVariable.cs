using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetVariable : IExpression
{
    public string Name { get; set; }
    public bool IsCommand { get; set; } = false;
    public static implicit operator string(GetVariable s) => s.Name;

    public void Print(int depth)
    {
        Debug.Log("Script/Print: " + new string(' ', depth * 2));
        Debug.Log("Script/Print: GET_VARIABLE: " + Name);
    }

    public object Interpret()
    {
        var glocal1 = IngameManagerV2.GetVariable(Name, ref IngameManagerV2.Local.Others, ref IngameManagerV2.Global.Others);
        var glocal2 = IngameManagerV2.GetVariable(Name, ref IngameManagerV2.Local.Transforms, ref IngameManagerV2.Global.Transforms);
        var glocal3 = IngameManagerV2.GetVariable(Name, ref IngameManagerV2.Local.Characters, ref IngameManagerV2.Global.Characters);
        var glocal4 = IngameManagerV2.GetVariable(Name, ref IngameManagerV2.Local.Images, ref IngameManagerV2.Global.Images);

        if (glocal1 != null) return glocal1;
        else if (glocal2 != null) return glocal2;
        else if (glocal3 != null) return glocal3;
        else if (glocal4 != null) return glocal4;

        if (Interpreter.FunctionTable.ContainsKey(Name))
        {
            return Interpreter.FunctionTable[Name];
        }

        if (IsCommand) return Name;
        return null;
    }
}
