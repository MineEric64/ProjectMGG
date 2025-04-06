using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace ProjectMGG.Ingame.Script.Keywords.Renpy
{
    public class Reeverb : IStatement
    {
        public int Line { get; set; } = 0;
        public IExpression Intervals { get; set; } = null;

        public void Interpret()
        {
            IngameManagerV2.Instance.IsReeverb = true;
            IngameManagerV2.Instance.ReeverbIntervals.Clear();

            if (Intervals != null)
            {
                var v = Intervals.Interpret() as List<object>;
                if (v != null) IngameManagerV2.Instance.ReeverbIntervals.AddRange(v.Select(x => (float)x).OrderBy(x => x));

                IngameManagerV2.Instance.EndReverbTime =
                (float)IngameManagerV2.Instance.ReeverbIntervals.FirstOrDefault(x => x >= (IngameManagerV2.Instance.MusicPlayer.time + 2));
            }
        }
    }
}