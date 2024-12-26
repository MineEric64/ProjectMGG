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
        SubImages = new Dictionary<string, string>();
    }

    public RpyAttributes(string mainImage)
    {
        MainImage = mainImage;
        SubImages = new Dictionary<string, string>();
    }
}
