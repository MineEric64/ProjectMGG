using UnityEngine;
using UnityEngine.UI;
using PrimeTween;

using Color = UnityEngine.Color;
using SizeF = System.Drawing.SizeF;

namespace ProjectMGG.Ingame.Script.Keywords.Renpy.ATL
{
    public class RpyColour : IATL
    {
        public int Line { get; set; } = 0;
        public RawImage Texture { get; set; } = null;
        public SizeF TextureSize { get; set; } = SizeF.Empty;
        public SizeF TextureSizeScaled { get; set; } = SizeF.Empty;

        public Ease EaseKind { get; set; } = Ease.Default;
        public float EaseDuration { get; set; } = 0f;
        public float StartDelay { get; set; } = 0f;

        public IExpression Value { get; set; } = null;

        public RpyColour()
        {

        }

        public RpyColour(string value)
        {
            Value = new StringLiteral(value);
        }

        public void Interpret()
        {
            if (Texture == null)
            {
                ExceptionManager.Throw("Failed to interpret 'colour' attribute in transform. The texture is null.", "Script/RpyColour", Line);
                return;
            }
            if (Value != null && Value.Interpret() is string value)
            {
                if (ConvertHexToColor(value, out Color color))
                {
                    Color start = Texture.color;
                    ATLBlock.SmartTweenCustom(start, color, EaseDuration, x => { Texture.color = x; }, EaseKind, StartDelay, UpdateType.LateUpdate);
                }
                else ExceptionManager.Throw($"Color Hex '{value}' parsing failed while interpreting 'colour' attribute in transform.", "Script/RpyColour", Line);
            }
            else ExceptionManager.Throw("Failed to interpret 'colour' attribute in transform. The value is null or not a number.", "Script/RpyColour", Line);
        }

        public static bool ConvertHexToColor(string value, out Color color)
        {
            color = Color.white;
            bool converted = false;

            converted = ColorUtility.TryParseHtmlString(value, out color);
            if (!converted && !value.StartsWith('#')) converted = ColorUtility.TryParseHtmlString(string.Concat("#", value), out color); //Concat #

            return converted;
        }
    }
}