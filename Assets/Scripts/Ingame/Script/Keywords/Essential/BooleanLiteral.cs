using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectMGG.Ingame.Script.Keywords
{
    public class BooleanLiteral : IExpression
    {
        public bool Value { get; set; } = false;

        public object Interpret()
        {
            return Value;
        }
    }
}