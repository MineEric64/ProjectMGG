using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectMGG.Ingame.Script.Keywords.Renpy
{
    public class Comment : IStatement
    {
        public string Content { get; set; } = string.Empty;

        public void Print(int depth)
        {

        }

        public void Interpret()
        {
            Debug.Log("Script/Interpret: # " + Content);
        }
    }
}