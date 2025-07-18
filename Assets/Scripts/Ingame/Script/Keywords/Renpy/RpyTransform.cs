using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectMGG.Ingame.Script.Keywords.Renpy
{
    public class RpyTransform : IStatement
    {
        public int Line { get; set; } = 0;
        public string Name { get; set; }
        public bool IsGlobal { get; set; } = false;

        public float xpos = -1f;
        public float ypos = -1f;
        public float xalign = -1f;
        public float yalign = -1f;
        public float xcenter = -1f;
        public float ycenter = -1f;

        public float zoom = 1f;

        public RpyTransform()
        {

        }

        public void Interpret()
        {
            var vars = IsGlobal ? IngameManagerV2.Global : IngameManagerV2.Local;
            vars.Transforms.Add(Name, this);
        }
    }
}