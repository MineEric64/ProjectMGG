using System.Drawing;

using UnityEngine;
using UnityEngine.UI;

using PrimeTween;

namespace ProjectMGG.Ingame.Script.Keywords.Renpy.ATL
{
    public class RpyBlur : IATL
    {
        public int Line { get; set; } = 0;
        public RawImage Texture { get; set; } = null;
        public SizeF TextureSize { get; set; } = SizeF.Empty;
        public SizeF TextureSizeScaled { get; set; } = SizeF.Empty;

        public Ease EaseKind { get; set; } = Ease.Default;
        public float EaseDuration { get; set; } = 0f;
        public float StartDelay { get; set; } = 0f;

        public IExpression Value { get; set; } = null;

        public RpyBlur()
        {

        }

        public RpyBlur(float value)
        {
            Value = new NumberLiteral(value);
        }

        public void Interpret()
        {
            if (Value != null && Value.Interpret() is float value)
            {
                float fxBlurStart = IngameManagerV2.Instance.FxBlur.focusDistance.value; //default: 10f
                float fxBlurEnd = value != 0 ? (1 / value) : 10;
                if (fxBlurEnd < 0 || fxBlurEnd >= 10) fxBlurEnd = 10f;

                ATLBlock.SmartTweenCustom(fxBlurStart, fxBlurEnd, EaseDuration, x =>
                    IngameManagerV2.Instance.FxBlur.focusDistance.value = x, EaseKind, StartDelay, UpdateType.LateUpdate);
            }
            else ExceptionManager.Throw("Failed to interpret 'blur' attribute in transform. The value is null or not a number.", "Script/RpyBlur", Line);
        }
    }
}