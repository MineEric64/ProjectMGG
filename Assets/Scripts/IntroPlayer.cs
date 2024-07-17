using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroManager : MonoBehaviour
{
    CanvasGroup canvasGroup;
    public GameObject canvasMain;
    public Button day2_demo1;
    public Button day2_demo2;
    public AudioSource bgm;

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
        SceneManager.LoadScene("Ingame");
    }

    void GoDay2_Demo2()
    {
        Application.Quit();
    }

    void UseDynamicDLL()
    {
        var DLL = Assembly.LoadFile(@$"{Application.dataPath}\DLLs\TestLibrary.dll");

        var theType = DLL.GetType("TestLibrary.Class1");
        var c = Activator.CreateInstance(theType);
        var method = theType.GetMethod("Test");
        method.Invoke(c, new object[] { @"Hello" });
    }
}
