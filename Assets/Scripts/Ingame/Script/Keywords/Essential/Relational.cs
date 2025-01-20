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

            if (Kind == ArgumentKind.Equal && Datatype.IsBoolean(lValue) && Datatype.IsBoolean(rValue))
            {
                return Datatype.ToBoolean(lValue) == Datatype.ToBoolean(rValue);
            }
            if (Kind == ArgumentKind.Equal && Datatype.IsNumber(lValue) && Datatype.IsNumber(rValue))
            {
                return Datatype.ToNumber(lValue) == Datatype.ToNumber(rValue);
            }
            if (Kind == ArgumentKind.Equal && Datatype.IsString(lValue) && Datatype.IsString(rValue))
            {
                return Datatype.ToString(lValue) == Datatype.ToString(rValue);
            }

            if (Kind == ArgumentKind.NotEqual && Datatype.IsBoolean(lValue) && Datatype.IsBoolean(rValue))
            {
                return Datatype.ToBoolean(lValue) != Datatype.ToBoolean(rValue);
            }
            if (Kind == ArgumentKind.NotEqual && Datatype.IsNumber(lValue) && Datatype.IsNumber(rValue))
            {
                return Datatype.ToNumber(lValue) != Datatype.ToNumber(rValue);
            }
            if (Kind == ArgumentKind.NotEqual && Datatype.IsString(lValue) && Datatype.IsString(rValue))
            {
                return Datatype.ToString(lValue) != Datatype.ToString(rValue);
            }

            if (Kind == ArgumentKind.LessThan && Datatype.IsNumber(lValue) && Datatype.IsNumber(rValue))
            {
                return Datatype.ToNumber(lValue) < Datatype.ToNumber(rValue);
            }
            if (Kind == ArgumentKind.GreaterThan && Datatype.IsNumber(lValue) && Datatype.IsNumber(rValue))
            {
                return Datatype.ToNumber(lValue) > Datatype.ToNumber(rValue);
            }
            if (Kind == ArgumentKind.LessOrEqual && Datatype.IsNumber(lValue) && Datatype.IsNumber(rValue))
            {
                return Datatype.ToNumber(lValue) <= Datatype.ToNumber(rValue);
            }
            if (Kind == ArgumentKind.GreaterOrEqual && Datatype.IsNumber(lValue) && Datatype.IsNumber(rValue))
            {
                return Datatype.ToNumber(lValue) >= Datatype.ToNumber(rValue);
            }

            return false;
        }
    }
}