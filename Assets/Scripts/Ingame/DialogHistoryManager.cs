using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using TMPro;

using ProjectMGG.Ingame.Script.Keywords.Renpy;

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

        /// <summary>
        /// Dark to Light (key: Character Name)
        /// </summary>
        public Dictionary<string, Color> ColorTable = new Dictionary<string, Color>()
        {
            //Stellarhouse
            {  "player", Color.black },
            { "haeun", new Color(0.945f, 0.647f, 0.729f) },
            { "yunseo", new Color(0.463f, 0.463f, 0.463f) }
        };

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        public void Add(Character chr, string dialogText, bool useColorTable = true)
        {
            var child = Instantiate(Prefab, Background.transform.Find("HistoryDialogs"));
            var childRectTransform = child.GetComponent<RectTransform>();
            var nameUI = child.transform.Find("Name").GetComponent<TextMeshProUGUI>();
            var contentUI = child.transform.Find("Content").GetComponent<TextMeshProUGUI>();

            if (chr != null)
            {
                Character chr2 = chr;
                if (useColorTable && !string.IsNullOrWhiteSpace(chr.VariableName) && ColorTable.TryGetValue(chr.VariableName, out Color colour))
                {
                    chr2 = (Character)chr.Clone();
                    chr2.Colour = colour;
                }

                IngameManagerV2.Instance.ProcessDialogName(chr2, nameUI, false);
            }
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