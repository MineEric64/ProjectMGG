using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    CanvasGroup canvasGroup;
    public GameObject canvasMain;

    public bool needToFadeIn = false;
    public bool needToFadeOut = false;
    public bool changeToMainAfterFadeOut = true;

    private float currentTime = 0.00f;
    private float currentAlpha = 0.00f;

    // Start is called before the first frame update
    void Start()
    {
        canvasMain.SetActive(false);
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (needToFadeIn)
        {
            currentAlpha = Mathf.MoveTowards(currentAlpha, 1.0f, 1.4f * Time.deltaTime);
            canvasGroup.alpha = currentAlpha;

            if (currentAlpha == 1.0f) needToFadeIn = false;
        }
        if (needToFadeOut)
        {
            currentAlpha = Mathf.MoveTowards(currentAlpha, 0.0f, 1.4f * Time.deltaTime);
            canvasGroup.alpha = currentAlpha;

            if (currentAlpha == 0.0f)
            {
                needToFadeOut = false;
                if (changeToMainAfterFadeOut)
                {
                    canvasMain.SetActive(true);
                }
            }
        }

        if (currentTime > 3.0f && !needToFadeOut && currentAlpha == 1.0f) needToFadeOut = true;
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            canvasGroup.alpha = 0f;
            needToFadeIn = false;
            needToFadeOut = false;
            canvasMain.SetActive(true);
        }

        currentTime += Time.deltaTime;
    }
}
