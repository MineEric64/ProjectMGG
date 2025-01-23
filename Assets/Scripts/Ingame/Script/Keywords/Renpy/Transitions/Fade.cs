using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProjectMGG.Ingame.Script.Keywords.Renpy.Transitions
{
    public class Fade : IPause
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
            OutTime = new NumberLiteral(outTime);
            HoldTime = new NumberLiteral(holdTime);
            InTime = new NumberLiteral(inTime);
        }

        public object Interpret()
        {
            return this;
        }

        public float GetPauseTime()
        {
            float outTime = OutTime?.Interpret() as float? ?? 0f;
            float holdTime = HoldTime?.Interpret() as float? ?? 0f;
            float inTime = InTime?.Interpret() as float? ?? 0f;
            return outTime + holdTime + inTime;
        }
    }
}