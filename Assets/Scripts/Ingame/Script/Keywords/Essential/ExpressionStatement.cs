using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectMGG.Ingame.Script.Keywords
{
    public class ExpressionStatement : IStatement
    {
        public IExpression Expression { get; set; }

        public void Print(int depth)
        {
            Debug.Log("Script/Print: " + new string(' ', depth * 2));
            Debug.Log("Script/Print: EXPRESSION:");
            Expression.Print(depth + 1);
        }

        public void Interpret()
        {
            Expression.Interpret();
        }
    }
}