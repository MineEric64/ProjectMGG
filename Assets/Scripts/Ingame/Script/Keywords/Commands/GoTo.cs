using UnityEngine;
using PrimeTween;

namespace ProjectMGG.Ingame.Script.Keywords.Commands
{
    public class GoTo : IStatement
    {
        public int Line { get; set; } = 0;
        public IExpression Value { get; set; } = null;

        private int _try = 0;

        public void Interpret()
        {
            string value = Value.Interpret() as string;

            if (value != null && int.TryParse(value, out int result))
            {
                if (IngameManagerV2.Instance != null) IngameManagerV2.Instance.StartCoroutine(IngameManagerV2.Instance.LetsGoTo(result));
                else
                {
                    _try++;
                    Debug.Log("Failed to interpret 'GoTo' because of Ingame not loaded yet.");
                    Tween.Delay(1f, () => { if (_try < 3) Interpret(); });
                }
            }
        }
    }
}