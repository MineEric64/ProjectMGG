using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProjectMGG.Ingame.Script.Keywords.Renpy
{
    public class Show : IStatement
    {
        public string Tag { get; set; }
        public string Attributes { get; set; } = string.Empty;
        public string At { get; set; } = string.Empty; //Transform Name
        public With With { get; set; } = null;

        public bool IsScene { get; set; } = false;
        public bool IsHide { get; set; } = false;

        public void Interpret()
        {
            IngameManagerV2.Instance.StartCoroutine(IngameManagerV2.Instance.LetsShow(this));
        }
    }
}