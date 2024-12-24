using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RpyAttributes
{
    public string MainImage { get; set; }
    public Dictionary<string, string> SubImages { get; set; }

    public RpyAttributes()
    {
        MainImage = string.Empty;
    }

    public RpyAttributes(string mainImage)
    {
        MainImage = mainImage;
    }
}
