using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectMGG.Ingame.Script.Keywords.Renpy
{
    public class Play : IStatement
    {
        public string Channel { get; set; }
        public string Path { get; set; }

        public void Print(int depth)
        {

        }

        public void Interpret()
        {
            IngameManagerV2.Instance.LetsPlay(Channel, Path);
        }
    }
}