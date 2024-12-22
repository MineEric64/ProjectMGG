using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene : IStatement
{
    public IExpression Argument { get; set; }

    public void Print(int depth)
    {
        Debug.Log("Script/Print: " + new string(' ', depth * 2));
        Debug.Log("Script/Print: SCENE");
        Argument.Print(depth + 1);
    }

    public void Interpret()
    {
        object value = Argument.Interpret();

        if (Datatype.IsString(value)) {
            string path = Datatype.ToString(value);

            Debug.Log("Script/Interpret: scene " + path);
            ///???
        }
    }
}
