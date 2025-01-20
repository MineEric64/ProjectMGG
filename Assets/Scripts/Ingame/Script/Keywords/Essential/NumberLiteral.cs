using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectMGG.Ingame.Script.Keywords
{
    public class NumberLiteral : IExpression
    {
        public float Value { get; set; } = 0.0f;
        public static implicit operator double(NumberLiteral s) => s.Value;
        public static implicit operator float(NumberLiteral s) => s.Value;

        public object Interpret()
        {
            return Value;
        }
    }
}