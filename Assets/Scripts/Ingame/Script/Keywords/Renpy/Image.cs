using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

namespace ProjectMGG.Ingame.Script.Keywords.Renpy
{
    public class Image : IStatement
    {
        public int Line { get; set; } = 0;
        public string Tag { get; set; }
        public string Attributes { get; set; } = string.Empty;
        public IExpression Data { get; set; }
        public bool IsGlobal { get; set; } = false;

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

                vars.Images[Tag].SubImages.Add(Attributes, ConvertDataToTexture());
            }
            else
            {
                var attributes = new Attributes();
                if (string.IsNullOrEmpty(Attributes)) attributes.MainImage = ConvertDataToTexture();
                else attributes.SubImages.Add(Attributes, ConvertDataToTexture());

                vars.Images.Add(Tag, attributes);
            }
        }

        public Texture2D ConvertDataToTexture()
        {
            if (Data != null)
            {
                if (Data is StringLiteral path)
                {
                    var texture = IngameManagerV2.LoadResource<Texture2D>(path);
                    return texture;
                }
                else if (Data is Solid solid)
                {
                    var texture = new Texture2D(Screen.width, Screen.height);
                    UnityEngine.Color[] pixels = Enumerable.Repeat(solid.Colour, Screen.width * Screen.height).ToArray();
                    texture.SetPixels(pixels);
                    texture.Apply();

                    return texture;
                }
            }

            return Texture2D.grayTexture;
        }
    }
}