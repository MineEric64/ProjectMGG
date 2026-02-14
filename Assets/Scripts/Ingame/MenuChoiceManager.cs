using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using TMPro;

using PrimeTween;

using ProjectMGG.Ingame.Script.Keywords.Renpy;
using ProjectMGG.UI;

namespace ProjectMGG.Ingame
{
    public class MenuChoiceManager : MonoBehaviour
    {
        public static MenuChoiceManager Instance { get; private set; } = null;

        public GameObject Prefab;
        public Vector3 Offset = new Vector3(0, 0, 0);
        public bool Active { get; private set; } = false;

        private MenuInput _menuInput;
        private Menu _currentMenu = null;
        private bool _ingameFocused = true;

        void Start()
        {
            Instance = this;
        }

        void Update()
        {
            if (_ingameFocused != IngameManagerV2.Instance.Focused) //toggle
            {
                _ingameFocused = IngameManagerV2.Instance.Focused;
                _menuInput?.SetButtonIgnoreEvent(!_ingameFocused);
            }
            if (_ingameFocused && Active) _menuInput?.UpdateForKeyboardInput();
        }

        public void CreateMenu(Menu menu)
        {
            Active = true;
            _currentMenu = menu;
            _menuInput = new MenuInput();
            _ingameFocused = IngameManagerV2.Instance.Focused;

            Vector3 position = new Vector3(0f, 360f);

            for (int i = 0; i < menu.Count; i++)
            {
                var prefab = Instantiate(Prefab, transform);
                var text = prefab.GetComponentInChildren<TextMeshProUGUI>();
                var buttonEvent = prefab.GetComponent<ButtonEvent>();
                var rectTransform = prefab.GetComponent<RectTransform>();
                float previousHeight = text.preferredHeight;

                //Prefab
                prefab.transform.localPosition = position;
                prefab.name = string.Concat("Menu", i.ToString());

                //Text
                text.text = menu.Names[i];

                if (previousHeight < text.preferredHeight)
                {
                    rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, text.preferredHeight);
                    position.y += 16f * (text.preferredHeight / previousHeight);
                }
                position.y -= rectTransform.sizeDelta.y;
                position += Offset;

                //Button Event
                buttonEvent.onClick.AddListener(() =>
                {
                    string name = prefab.name.Substring(4);
                    int index = int.Parse(name);
                    OnClick(index);
                });
                buttonEvent.onHover.AddListener((text) =>
                {
                    string name = prefab.name.Substring(4);
                    int index = int.Parse(name);
                    OnHover(index, rectTransform, text);
                });
                buttonEvent.onExit.AddListener((text) =>
                {
                    string name = prefab.name.Substring(4);
                    int index = int.Parse(name);
                    OnExit(index, rectTransform, text);
                });

                //Extra data
                _menuInput.AddMenuButton(buttonEvent);
            }

            //Dialog Text
            string head = menu.Head;
            if (string.IsNullOrEmpty(head)) head = "하나를 선택하세요."; //TODO: translation

            IngameManagerV2.Instance.LetsNarrationImmediate(head, true);
        }

        public void DeleteAllMenus()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
            _currentMenu = null;
            _menuInput = null;
            Active = false;
        }

        public void OnHover(int index, RectTransform rectTransform, TextMeshProUGUI textUI)
        {
            _menuInput.OnMouseHover(index);

            float widthStart = rectTransform.rect.width;
            float heightStart = rectTransform.rect.height;
            float fontSizeStart = textUI.fontSize;
            const float WIDTH_BIG = 1809.281f;
            const float HEIGHT_BIG = 120.1127f;
            const float FONT_SIZE_BIG = 73.2f;

            Tween.Custom(widthStart, WIDTH_BIG, 0.16f, x => { rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, x); }, Ease.OutQuad);
            Tween.Custom(heightStart, HEIGHT_BIG, 0.16f, x => { rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, x); }, Ease.OutQuint);
            Tween.Custom(fontSizeStart, FONT_SIZE_BIG, 0.16f, x => { textUI.fontSize = x; }, Ease.OutQuad);
        }

        public void OnExit(int index, RectTransform rectTransform, TextMeshProUGUI textUI)
        {
            _menuInput.OnMouseExit(index);

            float widthStart = rectTransform.rect.width;
            float heightStart = rectTransform.rect.height;
            float fontSizeStart = textUI.fontSize;
            const float WIDTH_DEFAULT = 1709.281f;
            const float HEIGHT_DEFAULT = 118.1127f;
            const float FONT_SIZE_DEFAULT = 72f;

            Tween.Custom(widthStart, WIDTH_DEFAULT, 0.16f, x => { rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, x); }, Ease.OutQuad);
            Tween.Custom(heightStart, HEIGHT_DEFAULT, 0.16f, x => { rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, x); }, Ease.OutQuint);
            Tween.Custom(fontSizeStart, FONT_SIZE_DEFAULT, 0.16f, x => { textUI.fontSize = x; }, Ease.OutQuad);
        }

        public void OnClick(int index)
        {
            _menuInput.OnMouseClick(index);

            IngameManagerV2.Instance.CallInteriorBlock(_currentMenu.Blocks[index]);
            PauseManager.Remove(true);
            DeleteAllMenus();
        }
    }
}