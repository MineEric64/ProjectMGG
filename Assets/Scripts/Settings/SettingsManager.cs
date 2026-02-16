using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

using TMPro;
using MessagePack;
using PrimeTween;

using ProjectMGG.Ingame;

namespace ProjectMGG.Settings
{
    public class SettingsManager : MonoBehaviour
    {
        /// <summary>
        /// LZ4 Compression for MessagePack
        /// </summary>
        public static MessagePackSerializerOptions LZ4_OPTIONS = MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4BlockArray);

        public static SettingsObject Settings { get; set; } = new SettingsObject();

        public static bool IsIngame { get; set; } = false;

        public List<string> Resolutions = new List<string>()
        {
            "1280x720",
            "1366x768",
            "1600x900",
            "1920x1080",
            "2560x1440",
            "3840x2160"
        };

        public Toggle UIFullscreen;
        public TMP_Dropdown UIResolution;
        public Slider UICPS;
        public TextMeshProUGUI UICPSValue;

        public Slider AudioMasterVolume;
        public TextMeshProUGUI AudioMasterVolumeValue;
        public Slider AudioMusicVolume;
        public TextMeshProUGUI AudioMusicVolumeValue;
        public Slider AudioSoundVolume;
        public TextMeshProUGUI AudioSoundVolumeValue;

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
            UIFullscreen.isOn = Settings.UI.FullScreen;
            UIResolution.value = Resolutions.FindIndex(x => Settings.UI.Resolution == x);
            UICPS.value = Settings.UI.CPS;
            UICPSValue.text = $": {Settings.UI.CPS:n1}";

            //Audio
            AudioMasterVolume.value = Settings.Audio.MasterVolume;
            AudioMusicVolume.value = Settings.Audio.MusicVolume;
            AudioSoundVolume.value = Settings.Audio.SoundVolume;
        }

        public void ApplySettingsFromUI()
        {
            //UI
            Settings.UI.FullScreen = UIFullscreen.isOn;
            Settings.UI.Resolution = Resolutions[UIResolution.value];
            Settings.UI.CPS = Mathf.Round(UICPS.value * 10) / 10; //rounding to 1 decimal place

            //Audio
            Settings.Audio.MasterVolume = Mathf.Round(AudioMasterVolume.value * 1000) / 1000; //rounding to 3 decimal places
            Settings.Audio.MusicVolume = Mathf.Round(AudioMusicVolume.value * 1000) / 1000;
            Settings.Audio.SoundVolume = Mathf.Round(AudioSoundVolume.value * 1000) / 1000;
            if (IsIngame && IngameManagerV2.Instance != null) IngameManagerV2.Instance.ApplyAudioVolume();
        }

        // Start is called before the first frame update
        void Start()
        {
            ApplySettings();
            ApplyToUI();

            //UI: Resolution
            string currentResolution = $"{Screen.width}x{Screen.height}";

            if (!Resolutions.Contains(currentResolution))
            {
                Resolutions.Add(currentResolution);
                Resolutions = Resolutions.OrderBy(x => x).ToList();
            }

            UIResolution.ClearOptions();
            UIResolution.AddOptions(Resolutions);
            UIResolution.value = Resolutions.FindIndex(x => currentResolution == x);
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

        //TODO: Apply the value immediately!!!
        public void UIFullscreenOnValueChanged()
        {
            if (UIFullscreen.isOn) Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            else Screen.fullScreenMode = FullScreenMode.Windowed;
        }

        public void UIResolutionOnValueChanged()
        {
            if (UIResolution.value < 0 || UIResolution.value >= Resolutions.Count) return; //out of bounds

            string resolution = Resolutions[UIResolution.value];
            string[] temp = resolution.Split('x');

            if (temp.Length < 2) return; //out of bounds, something went wrong about resolution format

            int width = int.Parse(temp[0]);
            int height = int.Parse(temp[1]);

            Screen.SetResolution(width, height, UIFullscreen.isOn);
        }

        public void UICPSOnValueChanged()
        {
            UICPSValue.text = $": {UICPS.value:n1}";
        }

        public void AudioMasterVolumeOnValueChanged()
        {
            int percent = Mathf.RoundToInt(AudioMasterVolume.value * 100);
            AudioMasterVolumeValue.text = $": {percent}%";

            Settings.Audio.MasterVolume = Mathf.Round(AudioMasterVolume.value * 1000) / 1000; //rounding to 3 decimal places
            if (IsIngame && IngameManagerV2.Instance != null) IngameManagerV2.Instance.ApplyAudioVolume();
        }

        public void AudioMusicVolumeOnValueChanged()
        {
            int percent = Mathf.RoundToInt(AudioMusicVolume.value * 100);
            AudioMusicVolumeValue.text = $": {percent}%";

            Settings.Audio.MusicVolume = Mathf.Round(AudioMusicVolume.value * 1000) / 1000;
            if (IsIngame && IngameManagerV2.Instance != null) IngameManagerV2.Instance.ApplyAudioVolume();
        }

        public void AudioSoundVolumeOnValueChanged()
        {
            int percent = Mathf.RoundToInt(AudioSoundVolume.value * 100);
            AudioSoundVolumeValue.text = $": {percent}%";

            Settings.Audio.SoundVolume = Mathf.Round(AudioSoundVolume.value * 1000) / 1000;
            if (IsIngame && IngameManagerV2.Instance != null) IngameManagerV2.Instance.ApplyAudioVolume();
        }
    }
}