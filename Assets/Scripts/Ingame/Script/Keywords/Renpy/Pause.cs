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
        public int Line { get; set; } = 0;
        public float Delay { get; set; }
        public bool Hard { get; set; } = false;

        public Pause()
        {

        }

        public Pause(float delay, bool hard = false)
        {
            Delay = delay;
            Hard = hard;
        }

        public void Interpret()
        {
            
        }
    }
}