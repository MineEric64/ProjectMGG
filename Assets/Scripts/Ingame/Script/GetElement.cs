using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetElement : IExpression
{
    public IExpression Sub { get; set; }
    public IExpression Index { get; set; }

    public void Print(int depth)
    {
        Debug.Log("Script/Print: " + new string(' ', depth * 2));
        Debug.Log("Script/Print: GET_ELEMENT:");

        Debug.Log("Script/Print: " + new string(' ', (depth + 1) * 2));
        Debug.Log("Script/Print: SUB:");
        Sub.Print(depth + 2);

        Debug.Log("Script/Print: " + new string(' ', (depth + 1) * 2));
        Debug.Log("Script/Print: INDEX:");
        Index.Print(depth + 2);
    }

    public object Interpret()
    {
        object obj = Sub.Interpret();
        object index = Index.Interpret();

        if (Datatype.IsArray(obj) && Datatype.IsNumber(index))
        {
            return Datatype.GetValueOfArray(obj, index);
        }
        if (Datatype.IsMap(obj) && Datatype.IsString(index))
        {
            return Datatype.GetValueOfMap(obj, index);
        }
        return null;
    }
}
