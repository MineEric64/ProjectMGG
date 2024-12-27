using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using SmartFormat.Extensions;
using SmartFormat;

public class IntroManager : MonoBehaviour
{
    CanvasGroup canvasGroup;
    public GameObject canvasMain;
    public Button day2_demo1;
    public Button day2_demo2;
    public AudioSource bgm;
    public TMP_InputField nameInput;

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
        Smart.Default.AddExtensions(new KoreanFormatter(Smart.Default));
        SettingsManager.ApplySettings();

        canvasMain.GetComponent<CanvasGroup>().alpha = 0.0f;
        canvasGroup = gameObject.GetComponent<CanvasGroup>();

        day2_demo1.onClick.AddListener(GoDay2_Demo1);
        day2_demo2.onClick.AddListener(GoDay2_Demo2);
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
            needToFadeIn = true;
            needToFadeOut = false;
            canvasGroup = canvasMain.GetComponent<CanvasGroup>();
            _intensity = 2.4f;
            _isSkipped = true;
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

    void GoDay2_Demo1()
    {
        ParamManager.ScriptPath = @$"{Application.dataPath}\Ingame\scripts\day\1.rpy";
        ApplyPlayerName();
        SceneManager.LoadScene("Ingame");
    }

    void GoDay2_Demo2()
    {
        ParamManager.ScriptPath = @$"{Application.dataPath}\Ingame\scripts\day\2.rpy";
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
        string defaultName = SettingsManager.Settings.IsDebug ? "남주" : "이주용";

        ParamManager.PlayerName = string.IsNullOrWhiteSpace(nameInput.text) ? defaultName : nameInput.text;
        ParamManager.PlayerName2 = GetPlayerName2(ParamManager.PlayerName);
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
}
