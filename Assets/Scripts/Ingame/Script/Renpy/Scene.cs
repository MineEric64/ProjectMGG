using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

[Obsolete("DON'T USE IT.")]
public class Scene : IStatement
{
    public string Tag { get; set; }
    public string Attributes { get; set; } = string.Empty;

    public void Print(int depth)
    {
        Debug.Log("Script/Print: " + new string(' ', depth * 2));
        Debug.Log("Script/Print: SCENE " + Tag);
    }

    public void Interpret()
    {
        var attributes = IngameManagerV2.GetVariable(Tag, ref IngameManagerV2.Local.Images, ref IngameManagerV2.Global.Images);

        if (attributes == null)
        {
            ExceptionManager.Throw($"Couldn't load the scene '{Tag}'. the variable doesn't exists.", "Scene/Interpret");
            return;
        }
        

        string pathRaw = attributes.MainImage;
        if (!string.IsNullOrEmpty(Attributes) && !attributes.SubImages.TryGetValue(Attributes, out pathRaw))
        {
            ExceptionManager.Throw($"Couldn't load the scene '{Tag}'. The attribute '{Attributes}' on image doesn't exists.");
            return;
        }
        Texture2D texture = IngameManagerV2.LoadResource<Texture2D>(pathRaw);

        if (texture != null)
        {
            IngameManagerV2.Instance.Background.color = Color.white;
            IngameManagerV2.Instance.Background.texture = texture;
        }
    }
}
