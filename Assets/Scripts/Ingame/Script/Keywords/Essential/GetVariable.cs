using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectMGG.Ingame.Script.Keywords
{
    public class GetVariable : Renpy.Transitions.IPause
    {
        public string Name { get; set; }
        public bool IsCommand { get; set; } = false;
        public static implicit operator string(GetVariable s) => s.Name;

        public object Interpret()
        {
            var glocal1 = IngameManagerV2.GetVariable(Name, ref IngameManagerV2.Local.Others, ref IngameManagerV2.Global.Others);
            if (glocal1 != null) return glocal1;

            var glocal2 = IngameManagerV2.GetVariable(Name, ref IngameManagerV2.Local.Transforms, ref IngameManagerV2.Global.Transforms);
            if (glocal2 != null) return glocal2;

            var glocal3 = IngameManagerV2.GetVariable(Name, ref IngameManagerV2.Local.Characters, ref IngameManagerV2.Global.Characters);
            if (glocal3 != null) return glocal3;

            var glocal4 = IngameManagerV2.GetVariable(Name, ref IngameManagerV2.Local.Images, ref IngameManagerV2.Global.Images);
            if (glocal4 != null) return glocal4;

            if (IsCommand) return Name;
            return null;
        }

        public float GetPauseTime()
        {
            return 0f;
        }
    }
}