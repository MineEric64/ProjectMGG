using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;

using TMPro;
using DG.Tweening;

using RpyTransform = transform;
using Unity.Burst.Intrinsics;

public class IngameManager : MonoBehaviour
{
    public ScriptInterpreter Interpreter;

    public RawImage Background;
    public TextMeshProUGUI NameUI;
    public TextMeshProUGUI ContentUI;
    public AudioSource MusicPlayer;
    public RawImage CharacterSample;
    public float TextAnimationMultiplier = 0.04f;

    private GraphicRaycaster _raycaster;
    private bool _goToNext = true;

    private float _preservedMusicTime = 0.0f;
    private string _currentPlayingMusic = "";
    private AudioReverbFilter _reverbFilter;
    private bool _isReeverb = false;
    private float _currentDecayTime = 0.1f;

    void Awake()
    {
        // Get both of the components we need to do this
        _raycaster = GetComponent<GraphicRaycaster>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Interpreter = new ScriptInterpreter(ParamManager.ScriptPath);
        _reverbFilter = MusicPlayer.GetComponent<AudioReverbFilter>();

        NameUI.text = "";
        ContentUI.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (IsClickedDialogUI())
        {
            //tempoary
            _goToNext = true;
        }

        if (_goToNext)
        {
            //tempoary
            var script = Interpreter.GetNumerator();
            var scriptNext = Interpreter.Peek();

            if (script != null)
            {
                switch (script.EssentialSyntax)
                {
                    case "scene":
                        string varName = script.Arguments[0];

                        if (!Interpreter.Images.TryGetValue(varName, out var pathRaw))
                        {
                            Debug.LogError($"IngameManager : Couldn't load the scene '{varName}'. the variable doesn't exists.");
                            break;
                        }
                        Texture2D texture = LoadResource<Texture2D>(pathRaw);

                        if (texture != null)
                        {
                            Background.color = Color.white;
                            Background.texture = texture;
                        }
                        break;

                    case "show":
                        Character chr2 = Interpreter.Characters[script.Arguments[0]];
                        string resource = script.FindArgument("image:", "default");
                        Texture2D texture2 = LoadResource<Texture2D>(chr2.Images[resource]);
                        RawImage prefab = GameObject.Find(chr2.NameVar)?.GetComponent<RawImage>();

                        if (prefab == null)
                        {
                            prefab = Instantiate(CharacterSample, this.transform);
                            prefab.transform.SetSiblingIndex(1);
                        }
                        
                        if (texture2 != null)
                        {
                            prefab.texture = texture2;
                            prefab.name = chr2.NameVar;
                            prefab.rectTransform.sizeDelta = new Vector3(texture2.width, texture2.height);

                            RpyTransform transform_ = null;

                            string at = script.FindArgument("at:");

                            if (!string.IsNullOrEmpty(at))
                            {
                                transform_ = Interpreter.Transforms[at];
                                float value = -1;

                                if (transform_.Options.TryGetValue("zoom", out value))
                                {
                                    float width = texture2.width * value;
                                    float height = texture2.height * value;

                                    prefab.transform.localScale = new Vector3(value, value);
                                    prefab.transform.localPosition = new Vector3(0f, -(720 - height / 2));
                                }
                                if (transform_.Options.TryGetValue("xalign", out value)) prefab.transform.localPosition = new Vector3(1280 * (value - 0.5f) * 2, prefab.transform.localPosition.y);
                                if (transform_.Options.TryGetValue("yalign", out value)) prefab.transform.localPosition = new Vector3(prefab.transform.localPosition.x, -(720 * (value - 0.5f) * 2));
                            }
                            else
                            {
                                prefab.transform.localPosition = new Vector3(0f, -(720 - texture2.height / 2));
                            }
                        }
                        break;

                    case "$narration":
                        NameUI.text = "";
                        ContentUI.text = script.Arguments[0];
                        TMPDOText(ContentUI, TextAnimationMultiplier * ContentUI.text.Length);

                        _goToNext = false;
                        break;

                    case "$dialog":
                        var chr = Interpreter.Characters[script.Arguments[0]];

                        NameUI.text = chr.Name;
                        NameUI.color = chr.Colour;
                        ContentUI.text = script.Arguments[1];
                        TMPDOText(ContentUI, TextAnimationMultiplier * ContentUI.text.Length);

                        _goToNext = false;
                        break;

                    case "#":
                        Debug.Log(script.Arguments[0]);
                        break;

                    case "reeverb":
                        _isReeverb = true;
                        break;

                    case "play":
                        if (script.Arguments[0] == "music")
                        {
                            if (!string.IsNullOrWhiteSpace(_currentPlayingMusic) && _currentPlayingMusic == script.Arguments[1]) //reeverbed
                            {
                                _reverbFilter.enabled = false;
                                MusicPlayer.time = _preservedMusicTime;
                                MusicPlayer.mute = false;
                            }
                            else
                            {
                                AudioClip audio = LoadResource<AudioClip>(script.Arguments[1]);
                                if (audio != null)
                                {
                                    MusicPlayer.clip = audio;
                                    MusicPlayer.Play();
                                    _currentPlayingMusic = script.Arguments[1];
                                }
                            }
                        }
                        break;
                }
            }
            else
            {
                //Story End
                SceneManager.LoadScene("MainMenu");
            }
            if (scriptNext != null)
            {
                if (scriptNext.EssentialSyntax == "reeverb")
                {
                    _currentDecayTime = 0.1f;
                    _reverbFilter.decayTime = 0.1f;
                    _reverbFilter.enabled = true;
                }
            }
        }
        
        if (_isReeverb) Reeverb();
    }

    bool IsClickedDialogUI()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //Set up the new Pointer Event
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            List<RaycastResult> results = new List<RaycastResult>();

            //Raycast using the Graphics Raycaster and mouse click position
            pointerData.position = Input.mousePosition;
            _raycaster.Raycast(pointerData, results);

            //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
            foreach (RaycastResult result in results)
            {
                if (result.gameObject.name == "DialogUI") return true;
            }
        }

        return false;
    }

    T LoadResource<T>(string pathRaw) where T : UnityEngine.Object
    {
        string path = pathRaw.Replace("/", @"\");
        string fileName = Path.GetFileName(path);
        T t = null;

        if (pathRaw.StartsWith(@"$/"))
        {
            t = Resources.Load<T>(ToResourcePath(path, pathRaw));
            if (t == null) Debug.LogError($"IngameManager : Couldn't load the file '{fileName}'. the file doesn't exists.");
        }

        return t;
    }

    string ToResourcePath(string path, string pathRaw)
    {
        return @$"assets/{pathRaw.Substring(2, pathRaw.LastIndexOf(Path.GetExtension(path)) - 2)}";
    }

    void Reeverb()
    {
        _currentDecayTime = Mathf.MoveTowards(_currentDecayTime, 7.0f, 3f * Time.deltaTime);
        _reverbFilter.decayTime = _currentDecayTime;

        if (_currentDecayTime == 7.0f)
        {
            MusicPlayer.mute = true;
            _preservedMusicTime = MusicPlayer.time;
            _isReeverb = false;
        }
    }

    public static void TMPDOText(TextMeshProUGUI text, float duration)
    {
        text.maxVisibleCharacters = 0;
        DOTween.To(x => text.maxVisibleCharacters = (int)x, 0f, text.text.Length, duration).SetEase(Ease.Linear);
    }
}
