using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectMGG.Ingame.Script.Keywords.Renpy
{
    public class Stop : IStatement
    {
        public int Line { get; set; } = 0;
        public string Channel { get; set; }

        public void Interpret()
        {
            IngameManagerV2.Instance.LetsStop(Channel);
        }
    }
}