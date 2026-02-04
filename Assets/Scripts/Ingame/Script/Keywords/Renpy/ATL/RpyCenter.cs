using System.Drawing;

using UnityEngine;
using UnityEngine.UI;

using PrimeTween;

namespace ProjectMGG.Ingame.Script.Keywords.Renpy.ATL
{
    public class RpyCenter : IATL
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

        public RpyCenter()
        {

        }

        public RpyCenter(float value, bool isX)
        {
            Value = new NumberLiteral(value);
            IsX = isX;
        }

        public RpyCenter(int value, bool isX)
        {
            Value = new NumberLiteral(value);
            IsX = isX;
        }

        public void Interpret()
        {
            if (Texture == null)
            {
                ExceptionManager.Throw("Failed to interpret 'center' attribute in transform. The texture is null.", "Script/RpyCenter", Line);
                return;
            }
            if (Value != null && Value.Interpret() is float value)
            {
                bool isFloat = true;
                if (Value is NumberLiteral number) isFloat = number.IsFloat;

                if (IsX)
                {
                    float x = Texture.transform.localPosition.x;

                    //int: absolute, float: ratio
                    if (isFloat) x = 1280 * (value - 0.5f) * 2;
                    else x = value - 1280;

                    //Texture.transform.localPosition = new Vector3(x, Texture.transform.localPosition.y);
                    ATLBlock.SmartTweenLocalPositionX(Texture.transform, x, EaseDuration, EaseKind, StartDelay, UpdateType.LateUpdate);
                }
                else
                {
                    float y = Texture.transform.localPosition.y;

                    //int: absolute, float: ratio
                    if (isFloat) y = -(720 * (value - 0.5f) * 2);
                    else y = -(value - 720);

                    //Texture.transform.localPosition = new Vector3(Texture.transform.localPosition.x, y);
                    ATLBlock.SmartTweenLocalPositionY(Texture.transform, y, EaseDuration, EaseKind, StartDelay, UpdateType.LateUpdate);
                }
            }
            else ExceptionManager.Throw("Failed to interpret 'center' attribute in transform. The value is null or not a number.", "Script/RpyCenter", Line);
        }
    }
}