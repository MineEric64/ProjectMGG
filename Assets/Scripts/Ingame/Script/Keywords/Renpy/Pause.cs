using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace ProjectMGG.Ingame.Script.Keywords.Renpy
{
    public class Pause : IStatement
    {
        public float Delay { get; set; }
        public bool Hard { get; set; } = false;

        public void Interpret()
        {
            
        }
    }
}