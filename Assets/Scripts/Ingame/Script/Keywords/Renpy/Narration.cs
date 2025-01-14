using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectMGG.Ingame.Script.Keywords.Renpy
{
    public class Narration : IStatement
    {
        public string Argument { get; set; }

        public void Print(int depth)
        {
            return;
        }

        public void Interpret()
        {
            IngameManagerV2.Instance.LetsNarration(Argument);
        }
    }
}