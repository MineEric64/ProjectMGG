using System.Drawing;

using UnityEngine;
using UnityEngine.UI;

using PrimeTween;

namespace ProjectMGG.Ingame.Script.Keywords.Renpy.ATL
{
    public class RpyRepeat : IATL
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
        /// used in Internal
        /// </summary>
        public int CurrentCount { get; set; } = 0;

        public RpyRepeat()
        {

        }

        public RpyRepeat(float value)
        {
            Value = new NumberLiteral(value);
        }

        public void Interpret()
        {

        }
    }
}