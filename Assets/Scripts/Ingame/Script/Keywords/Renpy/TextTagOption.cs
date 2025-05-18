using PrimeTween;

namespace ProjectMGG.Ingame.Script.Keywords.Renpy
{
    public class TextTagOption
    {
        public Ease Ease { get; set; } = Ease.Linear;
        public int StartIndex { get; set; } = 0;
        public float CPS { get; set; } = 25f;

        public TextTagOption()
        {

        }
    }
}