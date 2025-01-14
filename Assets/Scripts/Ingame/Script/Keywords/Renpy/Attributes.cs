using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectMGG.Ingame.Script.Keywords.Renpy
{
    public class Attributes
    {
        public string MainImage { get; set; }
        public Dictionary<string, string> SubImages { get; set; }

        public Attributes()
        {
            MainImage = string.Empty;
            SubImages = new Dictionary<string, string>();
        }

        public Attributes(string mainImage)
        {
            MainImage = mainImage;
            SubImages = new Dictionary<string, string>();
        }
    }
}