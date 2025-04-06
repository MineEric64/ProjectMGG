using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ProjectMGG.Ingame.Script.Keywords.Renpy.Transitions;

namespace ProjectMGG.Ingame.Script.Keywords.Renpy
{
    public class With : IStatement
    {
        public int Line { get; set; } = 0;
        public IPause Transition { get; set; }
        public bool Alone { get; set; }

        public With(bool alone)
        {
            Alone = alone;
        }

        public void Interpret()
        {
            if (Alone)
            {
                IngameManagerV2.Instance.StartCoroutine(IngameManagerV2.Instance.LetsWithBefore(this, false));
                IngameManagerV2.Instance.StartCoroutine(IngameManagerV2.Instance.LetsWithAfter(this, false));
            }
        }
    }
}