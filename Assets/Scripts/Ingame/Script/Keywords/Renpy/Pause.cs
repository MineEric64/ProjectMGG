using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectMGG.Ingame.Script.Keywords.Renpy
{
    public class Pause : IStatement, IComparable<Pause>
    {
        public int Line { get; set; } = 0;
        public Guid UUID { get; } = Guid.NewGuid();

        public float Delay { get; set; }
        public bool Hard { get; set; } = false;

        public IExpression DelayAsExpression { get; set; } = null;

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
            if (DelayAsExpression != null)
            {
                float? value = DelayAsExpression.Interpret() as float?;
                if (value != null && value.HasValue) Delay = value.Value;
                else ExceptionManager.Throw("Failed to interpret pause's delay value.", "Script/Pause", Line);
            }
        }

        public static Pause GetInfinity(bool hard = false)
        {
            return new Pause(9999f, hard);
        }

        public int CompareTo(Pause other)
        {
            return UUID.CompareTo(other.UUID);
        }
    }
}