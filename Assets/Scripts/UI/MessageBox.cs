using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

using TMPro;

namespace ProjectMGG.UI
{
    public class MessageBox : MonoBehaviour
    {
        public static MessageBox Instance { get; private set; } = null;
        public GameObject Prefab;

        private static Dictionary<Guid, UnityEvent<MessageResult>> _onSubmitMap;

        void Awake()
        {
            Instance = this;
            _onSubmitMap = new Dictionary<Guid, UnityEvent<MessageResult>>();
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        public void Info(string content, MessageButton button = MessageButton.OK, string name = "", UnityAction<MessageResult> onSubmit = null)
        {
            Guid uuid = Guid.NewGuid();
            var unityEvent = new UnityEvent<MessageResult>();

            if (onSubmit != null) unityEvent.AddListener(onSubmit);
            _onSubmitMap.Add(uuid, unityEvent);
            ShowInternal(0, content, button, name, uuid, onSubmit);
        }

        public void Warn(string content, MessageButton button = MessageButton.OK, string name = "", UnityAction<MessageResult> onSubmit = null)
        {
            Guid uuid = Guid.NewGuid();
            var unityEvent = new UnityEvent<MessageResult>();

            if (onSubmit != null) unityEvent.AddListener(onSubmit);
            _onSubmitMap.Add(uuid, unityEvent);
            ShowInternal(1, content, button, name, uuid, onSubmit);
        }

        public void Error(string content, MessageButton button = MessageButton.OK, string name = "", UnityAction<MessageResult> onSubmit = null)
        {
            Guid uuid = Guid.NewGuid();
            var unityEvent = new UnityEvent<MessageResult>();

            if (onSubmit != null) unityEvent.AddListener(onSubmit);
            _onSubmitMap.Add(uuid, unityEvent);
            ShowInternal(2, content, button, name, uuid, onSubmit);
        }

        private void ShowInternal(int image, string content, MessageButton button, string name, Guid uuid, UnityAction<MessageResult> onSubmit)
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

                    var unityEvent = _onSubmitMap[uuid];
                    unityEvent.Invoke(result);
                    unityEvent.RemoveListener(onSubmit);
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

                    var unityEvent = _onSubmitMap[uuid];
                    unityEvent.Invoke(MessageResult.Yes);
                    unityEvent.RemoveListener(onSubmit);
                });

                no.onClick.AddListener(() =>
                {
                    Destroy(prefab);

                    var unityEvent = _onSubmitMap[uuid];
                    unityEvent.Invoke(MessageResult.No);
                    unityEvent.RemoveListener(onSubmit);
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