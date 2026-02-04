using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectMGG.Ingame.Script.Keywords
{
    public class NumberLiteral : IExpression
    {
        public float Value { get; set; } = 0.0f;
        public bool IsFloat { get; set; } = true;

        public static explicit operator int(NumberLiteral s) => (int)s.Value;
        public static implicit operator double(NumberLiteral s) => s.Value;
        public static implicit operator float(NumberLiteral s) => s.Value;

        public NumberLiteral()
        {

        }

        public NumberLiteral(float value, bool isFloat = true)
        {
            Value = value;
            IsFloat = isFloat;
        }

        public NumberLiteral(int value)
        {
            Value = value;
            IsFloat = false;
        }

        public object Interpret()
        {
            return Value;
        }
    }
}