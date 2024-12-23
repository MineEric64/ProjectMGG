using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class Scene : IStatement
{
    public string Argument { get; set; }

    public void Print(int depth)
    {
        Debug.Log("Script/Print: " + new string(' ', depth * 2));
        Debug.Log("Script/Print: SCENE " + Argument);
    }

    public void Interpret()
    {
        var pathRaw = IngameManagerV2.GetVariable(Argument, ref IngameManagerV2.Local.Images, ref IngameManagerV2.Global.Images);
        var characters = IngameManagerV2.CombineValues(ref IngameManagerV2.Local.Characters, ref IngameManagerV2.Global.Characters, (x, v) => v.Any(xx => xx.NameVar == x.NameVar));

        if (string.IsNullOrEmpty(pathRaw))
        {
            ExceptionManager.Throw($"Couldn't load the scene '{Argument}'. the variable doesn't exists.", "Scene/Interpret");
            return;
        }
        if (characters.Count > 0) //remove existing characters
        {
            foreach (Character chr in characters)
            {
                var obj = GameObject.Find(chr.NameVar);
                if (obj != null) GameObject.Destroy(obj);
            }
        }

        Texture2D texture = IngameManagerV2.LoadResource<Texture2D>(pathRaw);

        if (texture != null)
        {
            IngameManagerV2.Instance.Background.color = Color.white;
            IngameManagerV2.Instance.Background.texture = texture;
        }
    }
}
