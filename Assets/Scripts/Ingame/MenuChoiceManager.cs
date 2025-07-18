using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using TMPro;

using ProjectMGG.Ingame.Script;
using ProjectMGG.Ingame.Script.Keywords;
using ProjectMGG.Ingame.Script.Keywords.Renpy;
using ProjectMGG.UI;

namespace ProjectMGG.Ingame
{
    public class MenuChoiceManager : MonoBehaviour
    {
        public static MenuChoiceManager Instance { get; private set; } = null;

        public GameObject Prefab;
        public Vector3 Offset = new Vector3(0, 0, 0);

        private Menu _currentMenu = null;

        void Start()
        {
            Instance = this;
        }

        public void CreateMenu(Menu menu)
        {
            _currentMenu = menu;
            Vector3 position = Prefab.transform.localPosition;

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
                    OnHover(index);
                });
                buttonEvent.onExit.AddListener((text) =>
                {
                    string name = prefab.name.Substring(4);
                    int index = int.Parse(name);
                    OnExit(index);
                });
            }

            //Dialog Text
            string head = menu.Head;
            if (string.IsNullOrEmpty(head)) head = "하나를 선택하세요."; //TODO: translation

            IngameManagerV2.Instance.LetsNarrationImmediate(head);
        }

        public void DeleteAllMenus()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
            _currentMenu = null;
        }

        public void OnHover(int index)
        {

        }

        public void OnExit(int index)
        {

        }

        public void OnClick(int index)
        {
            IngameManagerV2.Instance.CallInteriorBlock(_currentMenu.Blocks[index]);
            IngameManagerV2.Instance.StopPause();
            DeleteAllMenus();
        }
    }
}