using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectMGG.Ingame.Script.Keywords.Renpy.Transitions
{
    public class Dissolve : IPause
    {
        public IExpression Time { get; set; }

        public Dissolve()
        {

        }

        public Dissolve(float time)
        {
            Time = new NumberLiteral(time);
        }

        public object Interpret()
        {
            return this;
        }

        public float GetPauseTime()
        {
            return Time?.Interpret() as float? ?? 0f;
        }
    }
}