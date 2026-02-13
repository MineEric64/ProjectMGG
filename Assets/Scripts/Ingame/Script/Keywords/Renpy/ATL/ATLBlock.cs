using System;
using System.Collections.Generic;

using UnityEngine;

using PrimeTween;

namespace ProjectMGG.Ingame.Script.Keywords.Renpy.ATL
{
    public class ATLBlock
    {
        public List<IATL> Interior { get; set; } = new List<IATL>();

        public bool EaseEnabled { get; set; } = false;
        public Ease EaseKind { get; set; } = Ease.Default;
        public float EaseDuration { get; set; } = 0f; //replace to 0.001f if startDelay is not working with zero duration on Tween
        public IExpression EaseDurationAsExpression { get; set; } = null;

        public void ApplyExpression()
        {
            if (EaseDurationAsExpression != null && EaseDurationAsExpression.Interpret() is float value) EaseDuration = value;
        }

        //These smart tween methods can be solved about one frame issue

        public static void SmartTweenCustom(float startValue, float endValue, float duration, Action<float> onValueChange, Ease ease, float startDelay = 0, UpdateType updateType = default)
        {
            if (duration > 0 || startDelay > 0)
            {
                var settings = new TweenSettings<float>(startValue, endValue, duration, ease, startDelay: startDelay, updateType: updateType);
                Tween.Custom(settings, onValueChange);
            }
            else onValueChange.Invoke(endValue);
        }

        public static void SmartTweenCustom(Vector3 startValue, Vector3 endValue, float duration, Action<Vector3> onValueChange, Ease ease, float startDelay = 0, UpdateType updateType = default)
        {
            if (duration > 0 || startDelay > 0)
            {
                var settings = new TweenSettings<Vector3>(startValue, endValue, duration, ease, startDelay: startDelay, updateType: updateType);
                Tween.Custom(settings, onValueChange);
            }
            else onValueChange.Invoke(endValue);
        }

        public static void SmartTweenCustom(Color startValue, Color endValue, float duration, Action<Color> onValueChange, Ease ease, float startDelay = 0, UpdateType updateType = default)
        {
            if (duration > 0 || startDelay > 0)
            {
                var settings = new TweenSettings<Color>(startValue, endValue, duration, ease, startDelay: startDelay, updateType: updateType);
                Tween.Custom(settings, onValueChange);
            }
            else onValueChange.Invoke(endValue);
        }

        public static void SmartTweenLocalPositionX(Transform target, float endValue, float duration, Ease ease, float startDelay = 0, UpdateType updateType = default)
        {
            if (duration > 0 || startDelay > 0)
            {
                var settings = new TweenSettings<float>(endValue, duration, ease, startDelay: startDelay, updateType: updateType);
                Tween.LocalPositionX(target, settings);
            }
            else target.localPosition = new Vector3(endValue, target.localPosition.y);
        }

        public static void SmartTweenLocalPositionY(Transform target, float endValue, float duration, Ease ease, float startDelay = 0, UpdateType updateType = default)
        {
            if (duration > 0 || startDelay > 0)
            {
                var settings = new TweenSettings<float>(endValue, duration, ease, startDelay: startDelay, updateType: updateType);
                Tween.LocalPositionY(target, settings);
            }
            else target.localPosition = new Vector3(target.localPosition.x, endValue);
        }
    }
}