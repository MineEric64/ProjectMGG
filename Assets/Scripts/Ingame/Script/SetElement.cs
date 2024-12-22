using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetElement : IExpression
{
    public IExpression Sub { get; set; }
    public IExpression Index { get; set; }
    public IExpression Value { get; set; }

    public void Print(int depth)
    {
        Debug.Log("Script/Print: " + new string(' ', depth * 2));
        Debug.Log("Script/Print: SET_ELEMENT:");

        Debug.Log("Script/Print: " + new string(' ', (depth + 1) * 2));
        Debug.Log("Script/Print: SUB:");
        Sub.Print(depth + 2);

        Debug.Log("Script/Print: " + new string(' ', (depth + 1) * 2));
        Debug.Log("Script/Print: INDEX:");
        Index.Print(depth + 2);

        Debug.Log("Script/Print: " + new string(' ', (depth + 1) * 2));
        Debug.Log("Script/Print: VALUE:");
        Value.Print(depth + 2);
    }

    public object Interpret()
    {
        object obj = Sub.Interpret();
        object index = Index.Interpret();
        object value = Value.Interpret();

        if (Datatype.IsArray(obj) && Datatype.IsNumber(index))
        {
            return Datatype.SetValueOfArray(obj, index, value);
        }
        if (Datatype.IsMap(obj) && Datatype.IsString(index))
        {
            return Datatype.SetValueOfMap(obj, index, value);
        }
        return null;
    }
}
