using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectMGG.Ingame.Script.Keywords
{
    public class Unary : IExpression
    {
        public ArgumentKind Kind { get; set; }
        public IExpression Sub { get; set; }

        public object Interpret()
        {
            object value = Sub.Interpret();

            if (Kind == ArgumentKind.Add && Datatype.IsNumber(value))
            {
                return Math.Abs(Datatype.ToNumber(value));
            }
            if (Kind == ArgumentKind.Subtract && Datatype.IsNumber(value))
            {
                return Datatype.ToNumber(value) * -1;
            }
            return 0.0;
        }
    }
}