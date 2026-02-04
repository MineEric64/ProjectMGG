using System;
using System.Drawing;

using UnityEngine;
using UnityEngine.UI;

using PrimeTween;

namespace ProjectMGG.Ingame.Script.Keywords.Renpy.ATL
{
    public interface IATL : IStatement
    {
        public RawImage Texture { get; set; }
        public SizeF TextureSize { get; set; }
        public SizeF TextureSizeScaled { get; set; }

        public Ease EaseKind { get; set; }
        public float EaseDuration { get; set; }
        [Obsolete("If not used, delete it")]
        public float StartDelay { get; set; }
    }
}
