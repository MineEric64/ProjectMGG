using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Variable : IStatement
{
    public string Name { get; set; }
    public IExpression Expression { get; set; }

    public void Print(int depth)
    {
        Debug.Log("Script/Print: " + new string(' ', depth * 2));
        Debug.Log("Script/Print: VAR " + Name + ":");
        Expression.Print(depth + 1);
    }

    public void Interpret()
    {
        Interpreter.Local[Interpreter.Local.Count - 1][0][Name] = Expression.Interpret();
    }
}
