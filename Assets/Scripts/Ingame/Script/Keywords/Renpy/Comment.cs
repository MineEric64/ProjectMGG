using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectMGG.Ingame.Script.Keywords.Renpy
{
    public class Comment : IStatement
    {
        public int Line { get; set; } = 0;
        public string Content { get; set; } = string.Empty;

        public void Interpret()
        {
            Debug.Log("Script/Interpret: # " + Content);
        }
    }
}