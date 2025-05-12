using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectMGG.Ingame.Script.Keywords
{
    public class Arithmetic : IExpression
    {
        public ArgumentKind Kind { get; set; }
        public IExpression Lhs { get; set; }
        public IExpression Rhs { get; set; }

        public object Interpret()
        {
            object lValue = Lhs.Interpret();
            object rValue = Rhs.Interpret();

            if (Kind == ArgumentKind.Add && lValue is float && rValue is float)
            {
                return (float)lValue + (float)rValue;
            }
            if (Kind == ArgumentKind.Add && lValue is string && rValue is string)
            {
                return (string)lValue + (string)rValue;
            }
            if (Kind == ArgumentKind.Subtract && lValue is float && rValue is float)
            {
                return (float)lValue - (float)rValue;
            }
            if (Kind == ArgumentKind.Multiply && lValue is float && rValue is float)
            {
                return (float)lValue * (float)rValue;
            }
            if (Kind == ArgumentKind.Divide && lValue is float && rValue is float)
            {
                if ((float)rValue == 0f) return float.PositiveInfinity;
                return (float)lValue / (float)rValue;
            }
            if (Kind == ArgumentKind.Modulo && lValue is float && rValue is float)
            {
                if ((float)rValue == 0f) return float.NaN;
                return (float)lValue % (float) rValue;
            }

            ExceptionManager.Throw($"Invalid Arithmethic Expression '{lValue} {Kind.GetContent()} {rValue}'.", "Script/Arithmethic");
            return null;
        }
    }
}