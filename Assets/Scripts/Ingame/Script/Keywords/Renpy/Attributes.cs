using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectMGG.Ingame.Script.Keywords.Renpy
{
    public class Attributes
    {
        public Texture2D MainImage { get; set; }
        public Dictionary<string, Texture2D> SubImages { get; set; }

        public Attributes()
        {
            MainImage = Texture2D.blackTexture;
            SubImages = new Dictionary<string, Texture2D>();
        }

        public Attributes(Texture2D mainImage)
        {
            MainImage = mainImage;
            SubImages = new Dictionary<string, Texture2D>();
        }
    }
}