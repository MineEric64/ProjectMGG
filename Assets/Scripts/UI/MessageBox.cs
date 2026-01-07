using UnityEngine;
using UnityEngine.Events;

using TMPro;

namespace ProjectMGG.UI
{
    public class MessageBox : MonoBehaviour
    {
        public static MessageBox Instance { get; private set; } = null;
        public GameObject Prefab;
        public UnityEvent<MessageResult> OnSubmit;

        void Awake()
        {
            Instance = this;
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        public void Info(string content, MessageButton button = MessageButton.OK, string name = "", UnityAction<MessageResult> onSubmit = null)
        {
            if (onSubmit != null) OnSubmit.AddListener(onSubmit);
            ShowInternal(0, content, button, name);
        }

        public void Warn(string content, MessageButton button = MessageButton.OK, string name = "", UnityAction<MessageResult> onSubmit = null)
        {
            if (onSubmit != null) OnSubmit.AddListener(onSubmit);
            ShowInternal(1, content, button, name);
        }

        public void Error(string content, MessageButton button = MessageButton.OK, string name = "", UnityAction<MessageResult> onSubmit = null)
        {
            if (onSubmit != null) OnSubmit.AddListener(onSubmit);
            ShowInternal(2, content, button, name);
        }

        private void ShowInternal(int image, string content, MessageButton button, string name)
        {
            Vector3 position = Prefab.transform.localPosition;
            var prefab = Instantiate(Prefab, transform);
            var text = prefab.transform.Find("Content").GetComponentInChildren<TextMeshProUGUI>();
            var ok = prefab.transform.Find("ButtonOK").GetComponentInChildren<ButtonEvent>();

            bool isYesNo = button == MessageButton.YesNo;

            if (!isYesNo) {
                var result = MessageResult.OK;
                if (button == MessageButton.YesNoCancel) result = MessageResult.Cancel;

                ok.onClick.AddListener(() =>
                {
                    Destroy(prefab);
                    OnSubmit.Invoke(result);
                    OnSubmit.RemoveAllListeners();
                });
            }

            if (isYesNo || button == MessageButton.YesNoCancel)
            {
                var yes = prefab.transform.Find("ButtonYes").GetComponentInChildren<ButtonEvent>();
                var no = prefab.transform.Find("ButtonNo").GetComponentInChildren<ButtonEvent>();

                yes.gameObject.SetActive(true);
                no.gameObject.SetActive(true);
                if (isYesNo) ok.gameObject.SetActive(false);
                else ok.GetComponentInChildren<TextMeshProUGUI>().text = "Ãë¼Ò";

                yes.onClick.AddListener(() =>
                {
                    Destroy(prefab);
                    OnSubmit.Invoke(MessageResult.Yes);
                    OnSubmit.RemoveAllListeners();
                });

                no.onClick.AddListener(() =>
                {
                    Destroy(prefab);
                    OnSubmit.Invoke(MessageResult.No);
                    OnSubmit.RemoveAllListeners();
                });
            }

            prefab.transform.localPosition = position;
            text.text = content;
        }
    }

    public enum MessageButton
    {
        OK,
        YesNo,
        YesNoCancel
    }

    public enum MessageResult
    {
        OK,
        Yes,
        No,
        Cancel
    }
}