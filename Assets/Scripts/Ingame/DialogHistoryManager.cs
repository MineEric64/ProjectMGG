using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ProjectMGG.Ingame
{
    public class DialogHistoryManager : MonoBehaviour
    {
        public GameObject Prefab;
        public RawImage Background;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        public void Add(string characterName, string dialogText)
        {
            var child = Instantiate(Prefab, Background.transform);
            var nameUI = child.transform.Find("Name").GetComponent<TextMeshProUGUI>();
            var contentUI = child.transform.Find("Content").GetComponent<TextMeshProUGUI>();

            if (!string.IsNullOrWhiteSpace(characterName)) nameUI.text = characterName;
            else nameUI.gameObject.SetActive(false);

            contentUI.text = dialogText;
        }
    }
}