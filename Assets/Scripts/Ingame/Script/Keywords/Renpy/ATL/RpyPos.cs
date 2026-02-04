using System.Drawing;

using UnityEngine;
using UnityEngine.UI;

using PrimeTween;

namespace ProjectMGG.Ingame.Script.Keywords.Renpy.ATL
{
    public class RpyPos : IATL
    {
        public int Line { get; set; } = 0;
        public RawImage Texture { get; set; } = null;
        public SizeF TextureSize { get; set; } = SizeF.Empty;
        public SizeF TextureSizeScaled { get; set; } = SizeF.Empty;

        public Ease EaseKind { get; set; } = Ease.Default;
        public float EaseDuration { get; set; } = 0f;
        public float StartDelay { get; set; } = 0f;

        public IExpression Value { get; set; } = null;
        /// <summary>
        /// (true: X, false: Y)
        /// </summary>
        public bool IsX { get; set; } = true;

        public RpyPos()
        {

        }

        public RpyPos(float value, bool isX)
        {
            Value = new NumberLiteral(value);
            IsX = isX;
        }


        public void Interpret()
        {
            if (Texture == null)
            {
                ExceptionManager.Throw("Failed to interpret 'pos' attribute in transform. The texture is null.", "Script/RpyPos", Line);
                return;
            }
            if (Value != null && Value.Interpret() is float value)
            {
                bool isFloat = true;
                if (Value is NumberLiteral number) isFloat = number.IsFloat;

                if (IsX)
                {
                    float x = Texture.transform.localPosition.x;
                    float offset = (RpyTransform.xanchor - 0.5f) * TextureSizeScaled.Width;
                    if (!isFloat) offset = RpyTransform.xanchor - (TextureSize.Width / 2);

                    //int: absolute, float: ratio
                    if (isFloat) x = (1280 * (value - 0.5f) * 2) - offset;
                    else x = value - offset - 1280;

                    ATLBlock.SmartTweenLocalPositionX(Texture.transform, x, EaseDuration, EaseKind, StartDelay, UpdateType.LateUpdate);
                }
                else
                {
                    float y = Texture.transform.localPosition.y;
                    float offset = (RpyTransform.yanchor - 0.5f) * TextureSizeScaled.Height;
                    if (!isFloat) offset = RpyTransform.yanchor - (TextureSize.Height / 2);

                    //int: absolute, float: ratio
                    if (isFloat) y = -(720 * (value - 0.5f) * 2) + offset;
                    else y = -(value - offset - 720);

                    ATLBlock.SmartTweenLocalPositionY(Texture.transform, y, EaseDuration, EaseKind, StartDelay, UpdateType.LateUpdate);
                }
            }
            else ExceptionManager.Throw("Failed to interpret 'pos' attribute in transform. The value is null or not a number.", "Script/RpyPos", Line);
        }
    }
}