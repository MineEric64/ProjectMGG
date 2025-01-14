using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectMGG.Ingame.Script.Keywords
{
    public class BooleanLiteral : IExpression
    {
        public bool Value { get; set; } = false;

        public void Print(int depth)
        {
            Debug.Log("Script/Print: " + new string(' ', depth * 2));
            Debug.Log("Script/Print: " + (Value ? "true" : "false"));
        }

        public object Interpret()
        {
            return Value;
        }
    }
}