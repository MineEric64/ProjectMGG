using System.Drawing;

using UnityEngine;
using UnityEngine.UI;

using PrimeTween;

namespace ProjectMGG.Ingame.Script.Keywords.Renpy.ATL
{
    public class RpyZoom : IATL
    {
        public int Line { get; set; } = 0;
        public RawImage Texture { get; set; } = null;
        public SizeF TextureSize { get; set; } = SizeF.Empty;
        public SizeF TextureSizeScaled { get; set; } = SizeF.Empty;

        public Ease EaseKind { get; set; } = Ease.Default;
        public float EaseDuration { get; set; } = 0f;
        public float StartDelay { get; set; } = 0f;

        public IExpression Value { get; set; } = null;

        public RpyZoom()
        {

        }

        public RpyZoom(float value)
        {
            Value = new NumberLiteral(value);
        }

        public void Interpret()
        {
            if (Texture == null)
            {
                ExceptionManager.Throw("Failed to interpret 'zoom' attribute in transform. The texture is null.", "Script/RpyZoom", Line);
                return;
            }
            if (Value != null && Value.Interpret() is float value)
            {
                Vector3 start = Texture.transform.localScale;
                Vector3 end = new Vector3(value, value);

                //Texture.transform.localScale = end;
                ATLBlock.SmartTweenCustom(start, end, EaseDuration, (v) =>
                {
                    Texture.transform.localScale = v;
                    TextureSizeScaled = new SizeF(TextureSize.Width * v.x, TextureSize.Height * v.y);

                    //Texture.transform.localPosition = new Vector3(0f, -(720 - TextureSizeScaled.Height / 2)); //comment this if teleporting
                }, EaseKind, StartDelay, UpdateType.LateUpdate);
            }
            else ExceptionManager.Throw("Failed to interpret 'zoom' attribute in transform. The value is null or not a number.", "Script/RpyZoom", Line);
        }
    }
}