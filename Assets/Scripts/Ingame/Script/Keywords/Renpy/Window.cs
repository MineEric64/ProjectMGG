using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ProjectMGG.Ingame.Script.Keywords.Renpy.Transitions;
using ProjectMGG.Settings;

namespace ProjectMGG.Ingame.Script.Keywords.Renpy
{
    public class Window : IStatement
    {
        public int Line { get; set; } = 0;
        public int Method { get; set; } = 0; //0: Show, 1: Hide, 2: Auto True, 3: Auto False
        public IPause Transition { get; set; } = null;

        public void Interpret()
        {
            if (Method == 0) IngameManagerV2.Instance.LetsWindow(true, Transition);
            else if (Method == 1) IngameManagerV2.Instance.LetsWindow(false, Transition);
            else if (Method == 2) IngameManagerV2.Instance.WindowAuto = true;
            else if (Method == 3) IngameManagerV2.Instance.WindowAuto = false;
        }
    }
}