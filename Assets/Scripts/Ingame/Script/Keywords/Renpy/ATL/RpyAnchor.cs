using System.Drawing;

using UnityEngine;
using UnityEngine.UI;

using PrimeTween;

namespace ProjectMGG.Ingame.Script.Keywords.Renpy.ATL
{
    public class RpyAnchor : IATL
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

        public RpyAnchor()
        {

        }

        public RpyAnchor(float value, bool isX)
        {
            Value = new NumberLiteral(value);
            IsX = isX;
        }

        //uncomment this atl code if you want to interpret xanchor yanchor only
        public void Interpret()
        {
            if (Value != null && Value.Interpret() is float value)
            {
                //RpyPos atl = new RpyPos();
                //atl.Texture = Texture;
                //atl.TextureSize = TextureSize;
                //atl.TextureSizeScaled = TextureSizeScaled;
                //atl.EaseKind = EaseKind;
                //atl.EaseDuration = EaseDuration;
                //atl.StartDelay = StartDelay;

                if (IsX)
                {
                    RpyTransform.xanchor = value;

                    //atl.IsX = true;
                    //atl.Value = new NumberLiteral(0.5f);
                }
                else
                {
                    RpyTransform.yanchor = value;

                    //atl.IsX = false;
                    //atl.Value = new NumberLiteral(0.5f);
                }

                //atl.Interpret();
            }
            else ExceptionManager.Throw("Failed to interpret 'anchor' attribute in transform. The value is null or not a number.", "Script/RpyAnchor", Line);
        }
    }
}