using System.Collections.Generic;

using UnityEngine;
using TMPro;

using ProjectMGG.Ingame.Script.Keywords.Renpy;

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

            for (int i = 0; i < menu.Count; i++)
            {
                Debug.Log(Prefab.transform.localPosition);
                //Vector3 position = Prefab.transform.localPosition + i * Offset;
                var prefab = Instantiate(Prefab, transform);
                var rectTransform = prefab.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = Offset * i;
                var text = prefab.GetComponentInChildren<TextMeshProUGUI>();

                prefab.name = string.Concat("Menu", i.ToString());
                text.text = menu.Names[i];
            }
        }

        public void OnHover()
        {
            Debug.Log("HOVER");
        }

        public void OnExit()
        {
            Debug.Log("EXIT");
        }

        public void OnClick()
        {
            Debug.Log("CLICK");
        }
    }
}