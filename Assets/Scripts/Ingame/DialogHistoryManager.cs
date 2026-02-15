using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectMGG.Ingame
{
    public class DialogHistoryManager : MonoBehaviour
    {
        public GameObject Prefab;
        public RawImage Background;

        public int Count { get; private set; } = 0;
        private List<GameObject> _dialogs = new List<GameObject>();

        //Legacy: Instantiate + Position (with Offset)
        //Updated: Vertical Layout Group

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        public void Add(string characterName, string dialogText)
        {
            var child = Instantiate(Prefab, Background.transform.Find("HistoryDialogs"));
            var childRectTransform = child.GetComponent<RectTransform>();
            var nameUI = child.transform.Find("Name").GetComponent<TextMeshProUGUI>();
            var contentUI = child.transform.Find("Content").GetComponent<TextMeshProUGUI>();

            if (!string.IsNullOrWhiteSpace(characterName)) nameUI.text = characterName;
            else nameUI.gameObject.SetActive(false);

            contentUI.text = dialogText;
            
            //deprecated (legacy code), but you can use preferredheight from this code
            Vector2 preferredSize = contentUI.GetPreferredValues(dialogText, contentUI.rectTransform.rect.width, Mathf.Infinity);
            if (contentUI.rectTransform.rect.height < preferredSize.y) childRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, preferredSize.y);
            //child.transform.localPosition = Position;
            //Position -= new Vector3(0, preferredSize.y, 0);

            _dialogs.Add(child);
            Count++;
        }
    }
}