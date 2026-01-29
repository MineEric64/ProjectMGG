using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectMGG.Ingame.Script.Keywords.Renpy
{
    public class Pause : IStatement
    {
        public int Line { get; set; } = 0;
        public Guid UUID { get; } = Guid.NewGuid();

        public float Delay { get; set; }
        public bool Hard { get; set; } = false;

        //used for PauseManager, not internal
        public float CurrentDelay { get; set; } = 0f;
        public Action ActionAfter { get; set; } = null;

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

        public static Pause GetInfinity(bool hard = false)
        {
            return new Pause(9999f, hard);
        }
    }
}