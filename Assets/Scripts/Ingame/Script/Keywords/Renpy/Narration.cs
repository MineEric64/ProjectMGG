using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectMGG.Ingame.Script.Keywords.Renpy
{
    public class Narration : IStatement
    {
        public int Line { get; set; } = 0;
        public string Argument { get; set; }

        public void Interpret()
        {
            Argument = StringLiteral.ApplyVariable(Argument);
            IngameManagerV2.Instance.LetsNarration(Argument);
        }
    }
}