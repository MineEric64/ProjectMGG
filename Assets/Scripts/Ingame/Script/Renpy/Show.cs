using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

            foreach (Transform child in canvasImage)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
        IngameManagerV2.Instance.LetsShow(Tag, Attributes, At);
    }
}
