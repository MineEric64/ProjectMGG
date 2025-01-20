using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProjectMGG.Ingame.Script.Keywords.Renpy.Transitions
{
    public class Dissolve : IExpression
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
    }
}