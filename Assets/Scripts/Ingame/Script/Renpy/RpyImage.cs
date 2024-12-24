using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class RpyImage : IStatement
{
    public string Tag { get; set; }
    public string Attributes { get; set; } = string.Empty;
    public string Path { get; set; }
    public bool IsGlobal { get; set; } = false;

    public void Print(int depth)
    {
        Debug.Log("Script/Print: " + new string(' ', depth * 2));
        Debug.Log("Script/Print: IMAGE " + Path);
    }

    public void Interpret()
    {
        var vars = IsGlobal ? IngameManagerV2.Global : IngameManagerV2.Local;

        if (vars.Images.ContainsKey(Tag))
        {
            if (vars.Images[Tag].SubImages.ContainsKey(Attributes))
            {
                ExceptionManager.Throw($"The image '{Tag}' that has a attribute '{Attributes}' variable already exists.", "Script/Interpret");
                return;
            }
            vars.Images[Tag].SubImages.Add(Attributes, Path);
        }
        else
        {
            var attributes = new RpyAttributes();
            if (string.IsNullOrEmpty(Attributes)) attributes.MainImage = Path;
            else attributes.SubImages.Add(Attributes, Path);

            vars.Images.Add(Tag, attributes);
        }
    }
}
