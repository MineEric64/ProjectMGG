using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProjectMGG.Ingame.Script.Keywords.Renpy
{
    public class With : IStatement
    {
        public IExpression Transition { get; set; }
        public bool Alone { get; set; }

        public With(bool alone)
        {
            Alone = alone;
        }

        public void Interpret()
        {
            if (Alone)
            {
                IngameManagerV2.Instance.LetsWithBefore(this, false, out _);
                IngameManagerV2.Instance.LetsWithAfter(this, false);
            }
        }
    }
}