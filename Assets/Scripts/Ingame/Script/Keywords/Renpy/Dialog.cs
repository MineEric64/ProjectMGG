using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectMGG.Ingame.Script.Keywords.Renpy
{
    public class Dialog : IStatement
    {
        public string CharacterName { get; set; }
        public string Content { get; set; }

        public void Print(int depth)
        {
            return;
        }

        public void Interpret()
        {
            IngameManagerV2.Instance.LetsDialog(CharacterName, Content);
        }
    }
}