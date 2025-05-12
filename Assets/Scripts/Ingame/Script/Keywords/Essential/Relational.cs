using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectMGG.Ingame.Script.Keywords
{
    public class Relational : IExpression
    {
        public ArgumentKind Kind { get; set; }
        public IExpression Lhs { get; set; }
        public IExpression Rhs { get; set; }

        public object Interpret()
        {
            object lValue = Lhs.Interpret();
            object rValue = Rhs.Interpret();

            if (Kind == ArgumentKind.Equal && lValue is bool && rValue is bool)
            {
                return (bool)lValue == (bool)rValue;
            }
            if (Kind == ArgumentKind.Equal && lValue is float && rValue is float)
            {
                return (float)lValue == (float)rValue;
            }
            if (Kind == ArgumentKind.Equal && lValue is string && rValue is string)
            {
                return (string)lValue == (string)rValue;
            }

            if (Kind == ArgumentKind.NotEqual && lValue is bool && rValue is bool)
            {
                return (bool)lValue != (bool)rValue;
            }
            if (Kind == ArgumentKind.NotEqual && lValue is float && rValue is float)
            {
                return (float)lValue != (float)rValue;
            }
            if (Kind == ArgumentKind.NotEqual && lValue is string && rValue is string)
            {
                return (string)lValue != (string)rValue;
            }

            if (Kind == ArgumentKind.LessThan && lValue is float && rValue is float)
            {
                return (float)lValue < (float)rValue;
            }
            if (Kind == ArgumentKind.GreaterThan && lValue is float && rValue is float)
            {
                return (float)lValue > (float)rValue;
            }
            if (Kind == ArgumentKind.LessOrEqual && lValue is float && rValue is float)
            {
                return (float)lValue <= (float)rValue;
            }
            if (Kind == ArgumentKind.GreaterOrEqual && lValue is float && rValue is float)
            {
                return (float)lValue >= (float)rValue;
            }

            return false;
        }
    }
}