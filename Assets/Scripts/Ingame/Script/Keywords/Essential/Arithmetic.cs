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

            if (Kind == ArgumentKind.Add && Datatype.IsNumber(lValue) && Datatype.IsNumber(rValue))
            {
                return Datatype.ToNumber(lValue) + Datatype.ToNumber(rValue);
            }
            if (Kind == ArgumentKind.Add && Datatype.IsString(lValue) && Datatype.IsString(rValue))
            {
                return Datatype.ToString(lValue) + Datatype.ToString(rValue);
            }
            if (Kind == ArgumentKind.Subtract && Datatype.IsNumber(lValue) && Datatype.IsNumber(rValue))
            {
                return Datatype.ToNumber(lValue) - Datatype.ToNumber(rValue);
            }
            if (Kind == ArgumentKind.Multiply && Datatype.IsNumber(lValue) && Datatype.IsNumber(rValue))
            {
                return Datatype.ToNumber(lValue) * Datatype.ToNumber(rValue);
            }
            if (Kind == ArgumentKind.Divide && Datatype.IsNumber(lValue) && Datatype.IsNumber(rValue))
            {
                return Datatype.ToNumber(rValue) == 0 ? "INF" : Datatype.ToNumber(lValue) / Datatype.ToNumber(rValue);
            }
            if (Kind == ArgumentKind.Modulo && Datatype.IsNumber(lValue) && Datatype.IsNumber(rValue))
            {
                return Datatype.ToNumber(rValue) == 0 ? "NaN" : Datatype.ToNumber(lValue) % Datatype.ToNumber(rValue);
            }

            ExceptionManager.Throw($"Invalid Arithmethic Expression '{lValue} {Kind.GetContent()} {rValue}'.", "Script/Arithmethic");
            return null;
        }
    }
}