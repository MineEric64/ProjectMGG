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
        public bool IsScene { get; set; } = false;

        public void Print(int depth)
        {
            Debug.Log("Script/Print: " + new string(' ', depth * 2));
            Debug.Log("Script/Print: SHOW");
        }

        public void Interpret()
        {
            if (IsScene)
            {
                var canvasImage = IngameManagerV2.Instance.transform.Find("CanvasImage");

                foreach (UnityEngine.Transform child in canvasImage)
                {
                    if (child.gameObject.name == Tag)
                    {
                        GameObject.DestroyImmediate(child.gameObject);
                        continue;
                    }

                    GameObject.Destroy(child.gameObject);
                }
            }
            IngameManagerV2.Instance.LetsShow(Tag, Attributes, At);
        }
    }
}