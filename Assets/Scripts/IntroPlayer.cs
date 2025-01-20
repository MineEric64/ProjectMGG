using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

using PrimeTween;
using SmartFormat.Extensions;
using SmartFormat;

using ProjectMGG.Ingame;
using ProjectMGG.Ingame.Script.Commands;
using ProjectMGG.Settings;
using ProjectMGG.UI;

namespace ProjectMGG
{
    public class IntroPlayer : MonoBehaviour
    {
        CanvasGroup canvasGroup;
        public GameObject canvasMain;
        public AudioSource bgm;
        public TMP_InputField nameInput;
        public TMP_InputField commandInput;
        public GameObject menu;
        public GameObject textMenu;
        public GameObject seperators;

        public static IntroPlayer Instance { get; private set; } = null;

        public bool needToFadeIn = false;
        public bool needToFadeOut = false;
        public bool changeToMainAfterFadeOut = true;

        private float _currentTime = 0.00f;
        private float _currentAlpha = 0.00f;
        private float _intensity = 1.4f;
        private bool _isSkipped = false;

        // Start is called before the first frame update
        void Start()
        {
            Instance = this;
            Smart.Default.AddExtensions(new KoreanFormatter(Smart.Default));
            SettingsManager.ApplySettings();

            canvasMain.GetComponent<CanvasGroup>().alpha = 0.0f;
            canvasGroup = GetComponent<CanvasGroup>();
        }

        // Update is called once per frame
        void Update()
        {
            if (needToFadeIn)
            {
                FadeIn(canvasGroup, _intensity);
            }
            if (needToFadeOut)
            {
                FadeOut(canvasGroup, _intensity, () =>
                {
                    needToFadeOut = false;
                    if (changeToMainAfterFadeOut)
                    {
                        needToFadeIn = true;
                        canvasGroup = canvasMain.GetComponent<CanvasGroup>();
                        changeToMainAfterFadeOut = false;
                        _intensity = 2.4f;

                        AnimateUI();
                    }
                });
            }

            if (_currentTime > 3.0f && !_isSkipped)
            {
                needToFadeOut = true;
                _isSkipped = true;
            }
            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                canvasGroup.alpha = 0f;
                _currentAlpha = 0f;
                needToFadeIn = true;
                needToFadeOut = false;
                canvasGroup = canvasMain.GetComponent<CanvasGroup>();
                _intensity = 2.4f;
                _isSkipped = true;

                AnimateUI();
            }
            if (SettingsManager.Settings.Debug && Input.GetKeyDown(KeyCode.Slash))
            {
                bool active = commandInput.gameObject.activeSelf;

                commandInput.gameObject.SetActive(!active);
                if (!active) commandInput.ActivateInputField();
            }

            _currentTime += Time.deltaTime;
        }

        void FadeIn(CanvasGroup canvas, float intensity)
        {
            _currentAlpha = Mathf.MoveTowards(_currentAlpha, 1.0f, intensity * Time.deltaTime);
            canvas.alpha = _currentAlpha;

            if (_currentAlpha == 1.0f) needToFadeIn = false;
        }

        void FadeOut(CanvasGroup canvas, float intensity, Action action)
        {
            _currentAlpha = Mathf.MoveTowards(_currentAlpha, 0.0f, intensity * Time.deltaTime);
            canvas.alpha = _currentAlpha;

            if (_currentAlpha == 0.0f) action();
        }

        public void GoDay(string fileName)
        {
            IngameManagerV2.ScriptPath = @$"{Application.dataPath}/Ingame/scripts/day/{fileName}";
            ApplyPlayerName();
            SceneManager.LoadScene("Ingame");
        }

        void UseDynamicDLL()
        {
            var DLL = Assembly.LoadFile(@$"{Application.dataPath}\DLLs\TestLibrary.dll");

            var theType = DLL.GetType("TestLibrary.Class1");
            var c = Activator.CreateInstance(theType);
            var method = theType.GetMethod("Test");
            method.Invoke(c, new object[] { @"Hello" });
        }

        void ApplyPlayerName()
        {
            string defaultName = SettingsManager.Settings.Debug ? "남주" : "이주용";

            IngameManagerV2.PlayerName = string.IsNullOrWhiteSpace(nameInput.text) ? defaultName : nameInput.text;
            IngameManagerV2.PlayerName2 = GetPlayerName2(IngameManagerV2.PlayerName);
        }

        string GetPlayerName2(string playerName)
        {
            string[] database = new string[] { "김", "이", "박", "최", "정", "강", "조", "윤", "장", "임", "한", "오", "서", "신", "권", "황", "안", "송", "전", "홍", "유", "고", "문", "양", "손", "배", "백", "허", "남", "심", "노", "하", "곽", "성", "차", "주", "연", "방", "위", "표", "명", "기", "반", "라", "왕", "금", "옥", "육", "인", "맹", "제", "모", "장", "탁", "국", "여", "진", "어", "남궁", "독고", "선우", "제갈" };

            if (playerName.Length == 2) //이름 or 성(1글자), 이름(1글자)
            {
                //select: [0]이 성인가요?
            }
            else if (playerName.Length == 3) //성(1글자), 이름(2글자) or 성(2글자), 이름(1글자)
            {
                string familyName = database.Where(x => playerName.StartsWith(x)).FirstOrDefault();

                if (!string.IsNullOrEmpty(familyName))
                {
                    if (familyName.Length == 1) return playerName.Substring(1);
                    else return playerName.Substring(2);
                }
            }
            else if (playerName.Length == 4) //성(2글자), 이름(2글자)
            {
                return playerName.Substring(2);
            }
            return playerName;
        }

        public void ProcessCommand(TMP_InputField inputField)
        {
            string input = inputField.text;
            if (string.IsNullOrWhiteSpace(input) || input == "/") return;

            var scanner = new CmdScanner();
            List<CmdToken> tokenList = scanner.Scan(input);

            CmdParser parser = new CmdParser(ref tokenList);
            var syntaxTree = parser.Parse();

            CmdInterpreter interpreter = new CmdInterpreter();
            interpreter.Interpret(syntaxTree);
        }

        #region UI: Button Events
        public void Play()
        {
            var lowpass = bgm.GetComponent<AudioLowPassFilter>();

            Tween.Custom(1f, 0f, 3f, x => canvasGroup.alpha = x, Ease.OutQuad).OnComplete(this, x =>
            {
                x.GoDay("1.rpy");
            });
            //TODO: Blur?

            lowpass.enabled = true;
            Tween.AudioVolume(bgm, 1f, 0f, 3f, Ease.InSine);
            Tween.Custom(15000f, 300f, 3f, x => lowpass.cutoffFrequency = x, Ease.OutQuart);
        }

        public void Load()
        {
            Debug.Log("Load");
        }

        public void Gallery()
        {
            Debug.Log("Gallery");
        }

        public void Settings()
        {
            Debug.Log("Settings");
        }

        public void Exit()
        {
            Application.Quit();
        }
        #endregion
        #region UI: Animation
        void AnimateUI()
        {
            //Initialize
            textMenu.GetComponent<CanvasGroup>().alpha = 0f;
            menu.transform.localPosition = new Vector3(1740f, 0f, 0f);

            var repeater = seperators.GetComponent<ObjectRepeater>();
            repeater.Prefab.transform.localPosition = new Vector3(678f, 0f, 0f);
            repeater.ApplyOffsetChanges();

            //Animation
            Invoke("FadeInMainMenu1", 0.26f);
            Invoke("FadeInMainMenu2", 0.18f);
        }

        void FadeInMainMenu1()
        {
            var canvas = textMenu.GetComponent<CanvasGroup>();

            //TextMenu: Opacity
            Tween.Custom(0f, 1f, 1.67f, x =>
            {
                canvas.alpha = x;
            }, Ease.OutSine);
        }

        void FadeInMainMenu2()
        {
            //Menu: Position
            Tween.PositionX(menu.transform, 2188.46f, 1f, Ease.OutQuart);

            //Seperator: Position 66.53
            var repeater = seperators.GetComponent<ObjectRepeater>();

            Tween.LocalPositionX(repeater.Prefab.transform, 66.53f, 1.3f, Ease.OutQuart);
            Tween.Custom(237f, 0f, 1.3f, x =>
            {
                repeater.Offset = new Vector3(x, repeater.Offset.y, repeater.Offset.z);
                repeater.ApplyOffsetChanges();
            }, Ease.OutSine);
        }

        public void ButtonHoverAnimation(TextMeshProUGUI obj)
        {
            Tween.LocalPositionX(obj.transform, obj.transform.localPosition.x - 20f, 0.2f, Ease.OutQuart);
            //Tween.Scale(obj.transform, 1.05f, 0.2f, Ease.OutQuart); //uncomment this if you want to make a game design more intuitive
        }

        public void ButtonExitAnimation(TextMeshProUGUI obj)
        {
            Tween.LocalPositionX(obj.transform, obj.transform.localPosition.x + 20f, 0.2f, Ease.OutQuart);
            //Tween.Scale(obj.transform, 1.0f, 0.2f, Ease.OutQuart); //uncomment this if you want to make a game design more intuitive
        }
        #endregion
    }
}