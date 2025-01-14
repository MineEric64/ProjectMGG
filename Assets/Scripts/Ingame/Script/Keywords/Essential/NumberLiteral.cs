using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectMGG.Ingame.Script.Keywords
{
    public class NumberLiteral : IExpression
    {
        public double Value { get; set; } = 0.0;

        public void Print(int depth)
        {
            Debug.Log("Script/Print: " + new string(' ', depth * 2));
            Debug.Log("Script/Print: " + Value.ToString());
        }

        public object Interpret()
        {
            return Value;
        }
    }
}