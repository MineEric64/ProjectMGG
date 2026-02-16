using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using UnityEngine.UI;

using TMPro;
using MessagePack;
using PrimeTween;

using ProjectMGG.Ingame;
using ProjectMGG.UI;

namespace ProjectMGG.Settings
{
    public class SettingsManager : MonoBehaviour
    {
        /// <summary>
        /// LZ4 Compression for MessagePack
        /// </summary>
        public static MessagePackSerializerOptions LZ4_OPTIONS = MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4BlockArray);

        public static SettingsObject Settings { get; set; } = new SettingsObject();

        public Toggle Fullscreen;
        public Slider CPS;
        public TextMeshProUGUI CPSValue;

        public CanvasGroup CanvasSettingsGroup;

        public static string GetSettingsPath()
        {
            string path = $@"{Application.dataPath}\settings.json";

            if (!File.Exists(path))
            {
                ExceptionManager.Throw("The settings file 'settings.json' doesn't exists in Ingame directory. Please reinstall the game and try again.", "SettingsManager");
                return string.Empty;
            }

            return path;
        }

        public static void ApplySettings()
        {
            string path = GetSettingsPath();
            string json = File.ReadAllText(path);
            byte[] buffer = MessagePackSerializer.ConvertFromJson(json);
            Settings = MessagePackSerializer.Deserialize<SettingsObject>(buffer);
        }

        public static void SaveSettings()
        {
            string path = GetSettingsPath();
            string json = MessagePackSerializer.SerializeToJson(Settings);
            string jsonPretty = JSONBeautifier.Beautify(json);
            File.WriteAllText(path, jsonPretty);
        }

        public void ApplyToUI()
        {
            //UI
            Fullscreen.isOn = Settings.UI.FullScreen;
            CPS.value = Settings.UI.CPS;
            CPSValue.text = $": {Settings.UI.CPS:n1}";
        }

        public void ApplySettingsFromUI()
        {
            //UI
            Settings.UI.FullScreen = Fullscreen.isOn;
            Settings.UI.CPS = Mathf.Round(CPS.value * 10) / 10; //rounding to 1 decimal place
        }

        // Start is called before the first frame update
        void Start()
        {
            ApplySettings();
            ApplyToUI();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnButtonSaveClick()
        {
            ApplySettingsFromUI();
            SaveSettings();

            OnButtonCloseClick();
        }

        public void OnButtonCloseClick()
        {
            Tween.Custom(1f, 0f, 0.3f, x => { CanvasSettingsGroup.alpha = x; }, Ease.OutSine)
                .OnComplete(() => { Destroy(this.gameObject); });
        }

        public void FullscreenOnValueChanged()
        {
            if (Fullscreen.isOn) Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            else Screen.fullScreenMode = FullScreenMode.Windowed;
        }

        public void CPSOnValueChanged()
        {
            CPSValue.text = $": {CPS.value:n1}";
        }
    }
}