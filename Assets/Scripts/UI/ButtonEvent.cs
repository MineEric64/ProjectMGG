using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using PrimeTween;
using TMPro;

using Random = UnityEngine.Random;

//TODO: Change to PrimeTween
namespace ProjectMGG.UI
{
    public class ButtonEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public Color hover = new Color(0.00f, 0.00f, 0.00f, 0.95f); //single color support
        public Color exit = new Color(0.00f, 0.00f, 0.00f, 0.85f);
        public Color pressed;

        public List<Color> colors = new List<Color>(); //multiple color support

        public float updateTime = 0.2f;
        public float updateStart = 0f;

        float fadeCurrentAlpha = 1f;
        public float fadeTime = 1f;
        public float fadeStart = 0f;

        public bool haveToFadeOut = true;
        public bool haveToTextGlow = true;

        public UnityEvent onClick;
        public UnityEvent<TextMeshProUGUI> onHover;
        public UnityEvent<TextMeshProUGUI> onExit;

        public CanvasGroup canvas;
        TextMeshProUGUI buttonText;

        Color desiredColor;
        Color currentTextGlowColor;
        Color desiredTextGlowColor;

        bool needToUpdate = false;
        bool needToFadeOut = false;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (haveToTextGlow)
            {
                currentTextGlowColor = new Color32(0, 0, 0, 100);
                desiredTextGlowColor = new Color32(255, 255, 255, 100);
            }

            if (colors.Count > 0)
            {
                int index = Random.Range(0, colors.Count);
                desiredColor = colors[index];
            }
            else
            {
                desiredColor = hover;
            }

            needToUpdate = true; //legacy support for Text Glow
            Tween.Color(buttonText, desiredColor, 0.2f, Ease.OutSine);
            onHover?.Invoke(buttonText);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (haveToTextGlow)
            {
                currentTextGlowColor = new Color32(255, 255, 255, 100);
                desiredTextGlowColor = new Color32(0, 0, 0, 100);
            }

            desiredColor = exit;
            needToUpdate = true; //legacy support for Text Glow
            Tween.Color(buttonText, desiredColor, 0.2f, Ease.OutSine);
            onExit?.Invoke(buttonText);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            desiredColor = pressed;
            needToUpdate = true;
            needToFadeOut = true;

            onClick?.Invoke();
        }

        void Start()
        {
            pressed = exit;
            buttonText = this.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        }

        void Update()
        {
            if (needToUpdate) //legacy support for Text Glow
            {
                updateStart += Time.deltaTime;
                float weight = 2 * (updateTime / 0.2f);

                if (haveToTextGlow)
                {
                    var glowColor = Color.Lerp(currentTextGlowColor, desiredTextGlowColor, updateStart / weight);
                    buttonText.fontSharedMaterial.SetColor(ShaderUtilities.ID_GlowColor, glowColor);
                }

                if (updateStart >= updateTime)
                {
                    updateStart = 0f;
                    needToUpdate = false;
                }
            }
            if (haveToFadeOut && needToFadeOut)
            {
                fadeCurrentAlpha = Mathf.MoveTowards(fadeCurrentAlpha, 0.0f, 1.3f * Time.deltaTime);
                float scaleValue = 2f - fadeCurrentAlpha;
                buttonText.transform.localScale = new Vector3(2f * scaleValue, scaleValue, scaleValue);
                canvas.alpha = fadeCurrentAlpha;

                if (fadeCurrentAlpha == 0.0f) needToFadeOut = false;
            }
        }
    }
}