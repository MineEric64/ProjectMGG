using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProjectMGG.Ingame.Script.Keywords.Renpy.Transitions
{
    public class Fade : IExpression
    {
        public IExpression OutTime { get; set; }
        public IExpression HoldTime { get; set; }
        public IExpression InTime { get; set; }
        public Color Colour { get; set; }

        public Fade()
        {

        }

        public Fade(float outTime, float holdTime, float inTime)
        {
            OutTime = new NumberLiteral() { Value = outTime };
            HoldTime = new NumberLiteral() { Value = holdTime };
            InTime = new NumberLiteral() { Value = inTime };
        }

        public object Interpret()
        {
            return this;
        }
    }
}