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
            if (IsScene)
            {
                var canvasImage = IngameManagerV2.Instance.transform.Find("CanvasImage");

                foreach (Transform child in canvasImage)
                {
                    if (child.gameObject.name == Tag)
                    {
                        GameObject.DestroyImmediate(child.gameObject);
                        continue;
                    }

                    GameObject.Destroy(child.gameObject);
                }
            }

            IngameManagerV2.Instance.LetsShow(this);
        }
    }
}