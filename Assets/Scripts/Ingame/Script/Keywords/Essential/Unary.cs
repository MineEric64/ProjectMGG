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

            if (Kind == ArgumentKind.Add && value is float)
            {
                return Math.Abs((float)value);
            }
            if (Kind == ArgumentKind.Subtract && value is float)
            {
                return (float)value * -1;
            }
            return 0.0;
        }
    }
}