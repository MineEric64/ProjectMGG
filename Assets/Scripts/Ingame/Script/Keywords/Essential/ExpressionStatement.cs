using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectMGG.Ingame.Script.Keywords
{
    public class ExpressionStatement : IStatement
    {
        public IExpression Expression { get; set; }

        public void Interpret()
        {
            Expression.Interpret();
        }
    }
}