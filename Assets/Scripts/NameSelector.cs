using System.Linq;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

using TMPro;
using PrimeTween;

using SmartFormat;
using SmartFormat.Extensions;

using ProjectMGG.Ingame;
using ProjectMGG.Settings;
using ProjectMGG.UI;

namespace ProjectMGG
{
    public class NameSelector : MonoBehaviour
    {
        public TMP_InputField InputField;
        public UnityEvent<string> OnSubmit;
        public GameObject CanvasSettings;

        private static CanvasGroup canvasGroup;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();

            InputField.text = "";
            InputField.ActivateInputField();

            //temporary
            SettingsManager.ApplySettings();
            Smart.Default.AddExtensions(new KoreanFormatter(Smart.Default));
            PrimeTweenConfig.warnZeroDuration = false;
            IngameManagerV2.GameStatus = GameUIStatus.Main;
            InputField.onSubmit.AddListener((text) => LetsStartTheGame(text));
        }

        public static string GetPlayerName2Korean(string playerName)
        {
            //reference: https://namu.wiki/w/%ED%95%9C%EA%B5%AD%EC%9D%98%20%EC%84%B1%EC%94%A8%EB%B3%84%20%EC%9D%B8%EA%B5%AC%20%EB%B6%84%ED%8F%AC
            string[] database = new string[] {
                "김", "이", "박", "최", "정", "강", "조", "윤", "장", "임",
                "한", "오", "서", "신", "권", "황", "안", "송", "전", "홍",
                "유", "류", "고", "문", "양", "손", "배", "백", "허", "남",
                "심", "노", "하", "곽", "성", "차", "주", "연", "방", "위",
                "표", "명", "기", "반", "라", "왕", "금", "옥", "육", "인",
                "맹", "제", "모", "장", "탁", "국", "여", "진", "어", "은",
                "남궁", "독고", "선우", "제갈", "황보", "사공", "서문", "동방"
            };

            if (playerName.Length == 2) //이름 or 성(1글자), 이름(1글자)
            {
                string familyName = database.Where(x => playerName.StartsWith(x)).FirstOrDefault();

                if (!string.IsNullOrEmpty(familyName))
                {
                    //select: [0]이 성인가요?
                    MessageBox.Instance.Info("입력한 이름에 성이 포함되어 있나요?", MessageButton.YesNoCancel, "", (result) => IncludeFamilyName2(result));
                    return string.Empty;
                }
                else return playerName;
            }
            else if (playerName.Length >= 3) //성(n글자), 이름(n + 1글자) or 성(n + 1글자), 이름(n글자)
            {
                string familyName = database.Where(x => playerName.StartsWith(x)).FirstOrDefault();

                if (!string.IsNullOrEmpty(familyName))
                {
                    if (familyName.Length == 1) return playerName.Substring(1);
                    else return playerName.Substring(2);
                }

                return playerName.Substring(1);
                //return playerName; //uncomment this if you want to reveal the player's full name
            }

            return playerName;
        }

        private static void IncludeFamilyName2(MessageResult result)
        {
            if (result == MessageResult.Yes)
            {
                IngameManagerV2.PlayerName2 = IngameManagerV2.PlayerName.Substring(1);
                PlayGame();
            }
            else if (result == MessageResult.No)
            {
                IngameManagerV2.PlayerName2 = IngameManagerV2.PlayerName;
                PlayGame();
            }
        }

        private static void PlayGame()
        {
            //var lowpass = bgm.GetComponent<AudioLowPassFilter>();

            Tween.Custom(1f, 0f, 3f, x => canvasGroup.alpha = x, Ease.OutQuad).OnComplete(() =>
            {
                GoDayInternal(@$"{Application.dataPath}/Ingame/scripts/day/stellarhouse.rpy");
                //GoDayFromUrl("https://raw.githubusercontent.com/MineEric64/ProjectMGG/245fa0189b6666d3a27e30c83e11640028a53fd4/Assets/Ingame/scripts/day/stellarhouse.rpy");
            });
            //TODO: Blur?

            //lowpass.enabled = true;
            //Tween.AudioVolume(bgm, 1f, 0f, 3f, Ease.InSine);
            //Tween.Custom(15000f, 300f, 3f, x => lowpass.cutoffFrequency = x, Ease.OutQuart);
        }

        private static void GoDayFromUrl(string url)
        {
            GoDayInternal($"url:{url}");
        }

        private static void GoDayInternal(string scriptPath)
        {
            IngameManagerV2.ScriptPath = scriptPath;
            SceneManager.LoadScene("Ingame");
        }

        public void LetsStartTheGame(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Instance.Info("이름을 입력해주세요. (예: 이주용)");
                return;
            }

            if (SettingsManager.Settings.Debug && name == "main")
            {
                SceneManager.LoadScene("MainMenu");
                return;
            }

            string defaultName = SettingsManager.Settings.Debug ? "남주" : "이주용";

            IngameManagerV2.PlayerName = string.IsNullOrWhiteSpace(name) ? defaultName : name;
            IngameManagerV2.PlayerName = IngameManagerV2.PlayerName.Trim(); //excluding Whitespace
            IngameManagerV2.PlayerName2 = GetPlayerName2Korean(IngameManagerV2.PlayerName);

            if (!string.IsNullOrEmpty(IngameManagerV2.PlayerName2)) PlayGame();
        }

        public void GameStartButton()
        {
            string name = InputField.text;
            LetsStartTheGame(name);
        }

        public void Settings()
        {
            IngameManagerV2.Settings(CanvasSettings, false);
        }

        public void Exit()
        {
            Application.Quit();
        }
    }
}