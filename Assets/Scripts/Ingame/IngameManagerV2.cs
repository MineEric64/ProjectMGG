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
using PrimeTween;

using Path = System.IO.Path;
using RpyTransform = transform;
using System.Linq;

public class IngameManagerV2 : MonoBehaviour
{
    public static IngameManagerV2 Instance { get; private set; } = null;
    public static VariableCollection Local { get; private set; } = new VariableCollection(); //TODO: every label (using stack)
    public static VariableCollection Global { get; private set; } = new VariableCollection();

    public CanvasGroup CanvasDefault;

    private List<Token> _tokens;
    [SerializeField] private List<string> _tokensDebug;
    public Interpreter Interpreter;

    public RawImage Background;
    public TextMeshProUGUI NameUI;
    public TextMeshProUGUI ContentUI;
    public RawImage CharacterSample;
    public float TextAnimationMultiplier = 0.04f;

    public AudioSource MusicPlayer;
    public bool IsReeverb = false;
    public List<double> ReeverbIntervals = new List<double>();
    public float EndReverbTime = 0.0f;

    private float _preservedMusicTime = 0.0f;
    private string _currentPlayingMusic = "";
    private AudioReverbFilter _reverbFilter;
    private float _currentDecayTime = 0.1f;

    private GraphicRaycaster _raycaster;
    private bool _goToNext = true;
    private bool _readAll = false;

    void Awake()
    {
        // Get both of the components we need to do this
        _raycaster = GetComponent<GraphicRaycaster>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //Initialize
        Instance = this;
        Local = new VariableCollection();
        Global = new VariableCollection();

        //Script
        var scanner = new Scanner();
        Parser parser;
        Interpreter = new Interpreter();
        
        if (!File.Exists(ParamManager.ScriptPath))
        {
            ExceptionManager.Throw($"Can't read the script because file doesn't exists.\n(File Path: '{ParamManager.ScriptPath}')", "IngameManagerV2/Script");
            return;
        }

        string sourceCode = File.ReadAllText(ParamManager.ScriptPath);
        _tokens = scanner.Scan(sourceCode);
        //_tokensDebug = _tokens.Select(x => x.ToString()).ToList();
        parser = new Parser(ref _tokens);

        Program syntaxTree = parser.Parse();
        Interpreter.Interpret(syntaxTree);

        //Audio
        _reverbFilter = MusicPlayer.GetComponent<AudioReverbFilter>();

        //UI
        NameUI.text = "";
        ContentUI.text = "";

        CanvasDefault = GetComponent<CanvasGroup>();
        CanvasDefault.alpha = 0f;
        Tween.Custom(0f, 1f, 1f, x => CanvasDefault.alpha = x, Ease.InSine);
    }

    // Update is called once per frame
    void Update()
    {
        if (ContentUI.text.Length == 0) _readAll = true;
        else if (!_readAll) _readAll = ContentUI.maxVisibleCharacters == ContentUI.text.Length;

        if (IsClickedDialogUI() == 1)
        {
            if (!_readAll)
            {
                _readAll = true;
                ContentUI.maxVisibleCharacters = ContentUI.text.Length;
            }
            else
            {
                _goToNext = true;
                _readAll = false;
            }
        }

        if (_goToNext)
        {
            if (Interpreter.CurrentPoint == null) //error occured while scanning-parsing-interpreting
            {
                ExceptionManager.Throw("The error has occured while interpreting the script.", "IngameManagerV2/Script");
            }

            var script = Interpreter.CurrentPoint?.GetCurrentBlock();
            //var scriptNext = Interpreter.CurrentPoint?.GetNextBlock();

            if (script != null)
            {
                Interpreter.CurrentPoint.Interpret();

                if (script is Reeverb)
                {
                    _currentDecayTime = 0.1f;
                    _reverbFilter.decayTime = 0.1f;
                    _reverbFilter.enabled = true;
                }
            }
            else
            {
                //Story End
                SceneManager.LoadScene("MainMenu");
            }
        }
        
        if (IsReeverb) Reeverb();
    }

    /// <summary>
    /// 1: DialogUI
    /// 2: other (TODO)
    /// </summary>
    int IsClickedDialogUI()
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
                //Debug.Log(result.gameObject.name);
                return 1;
                //TODO: if other button is touched, return 2~
                //if (result.gameObject.name == "DialogUI") return true;
            }
        }

        return 0;
    }

    public void LetsNarration(string content)
    {
        NameUI.text = "";
        ContentUI.text = content;
        TMPDOText(ContentUI, TextAnimationMultiplier * ContentUI.text.Length);

        _goToNext = false;
    }

    public void LetsDialog(string chrName, string content)
    {
        var chr = GetVariable(chrName, ref Local.Characters, ref Global.Characters);

        if (chr == null)
        {
            ExceptionManager.Throw($"Invalid character argument '{chrName}' on dialog.", "IngameManagerV2");
            return;
        }

        NameUI.text = chr.Name;
        NameUI.color = chr.Colour;
        ContentUI.text = content;
        TMPDOText(ContentUI, TextAnimationMultiplier * ContentUI.text.Length);

        _goToNext = false;
    }

    public void LetsShow(string variableName, string attributes = "", string transformName = "")
    {
        var image = GetVariable(variableName, ref Local.Images, ref Global.Images);
        string resource = "";

        if (image == null)
        {
            ExceptionManager.Throw($"The image '{variableName}' variable doesn't exists while interpreting 'show' statement.", "IngameManagerV2");
            return;
        }
        if (string.IsNullOrEmpty(attributes)) resource = image.MainImage;
        else
        {
            if (!image.SubImages.TryGetValue(attributes, out var subPath))
            {
                ExceptionManager.Throw($"The image '{variableName}' that has a attribute '{attributes}' variable doesn't exists.", "IngameManagerV2");
                return;
            }
            resource = subPath;
        }

        Texture2D texture = LoadResource<Texture2D>(resource);
        RawImage prefab = GameObject.Find(variableName)?.GetComponent<RawImage>();

        if (prefab == null)
        {
            prefab = Instantiate(CharacterSample, this.transform.Find("CanvasImage"));
            prefab.transform.SetSiblingIndex(1);
        }

        if (texture != null)
        {
            prefab.texture = texture;
            prefab.name = variableName;
            prefab.rectTransform.sizeDelta = new Vector3(texture.width, texture.height);

            if (!string.IsNullOrEmpty(transformName))
            {
                var transform = GetVariable(transformName, ref Local.Transforms, ref Global.Transforms);
                if (transform == null)
                {
                    ExceptionManager.Throw($"The transform '{transformName}' variable doesn't exists while interpreting 'show' statement.", "IngameManagerV2");
                    return;
                }

                if (transform.zoom != 1f)
                {
                    float width = texture.width * transform.zoom;
                    float height = texture.height * transform.zoom;

                    prefab.transform.localScale = new Vector3(transform.zoom, transform.zoom);
                    prefab.transform.localPosition = new Vector3(0f, -(720 - height / 2));
                }
                //TODO: xpos, ypos, xalign, yalign
                if (transform.xcenter != -1f) prefab.transform.localPosition = new Vector3(1280 * (transform.xcenter - 0.5f) * 2, prefab.transform.localPosition.y); //TODO: 0~1: ratio, 1~: absolute value
                if (transform.ycenter != -1f) prefab.transform.localPosition = new Vector3(prefab.transform.localPosition.x, -(720 * (transform.ycenter - 0.5f) * 2));
            }
            else
            {
                prefab.transform.localPosition = new Vector3(0f, -(720 - texture.height / 2));
            }
        }
    }

    public void LetsPlay(string channel, string path)
    {
        switch (channel)
        {
            case "music":
            {
                    if (!string.IsNullOrWhiteSpace(_currentPlayingMusic) && _currentPlayingMusic == path) //reeverbed
                    {
                        _reverbFilter.enabled = false;
                        MusicPlayer.time = _preservedMusicTime;
                        MusicPlayer.mute = false;
                    }
                    else
                    {
                        AudioClip audio = LoadResource<AudioClip>(path);
                        if (audio != null)
                        {
                            MusicPlayer.clip = audio;
                            MusicPlayer.Play();
                            _currentPlayingMusic = path;
                        }
                    }
                    break;
            }

            default:
                ExceptionManager.Throw("TODO: support channel on play keyword", "IngameManagerV2");
                break;
        }
    }

    public static T LoadResource<T>(string pathRaw) where T : UnityEngine.Object
    {
        string path = pathRaw.Replace("/", @"\");
        string fileName = Path.GetFileName(path);
        T t = null;

        if (pathRaw.StartsWith(@"$/"))
        {
            t = Resources.Load<T>(ToResourcePath(path, pathRaw));
            if (t == null) ExceptionManager.Throw($"Couldn't load the file '{fileName}'. the file doesn't exists.", "IngameManagerV2");
        }

        return t;
    }

    public static string ToResourcePath(string path, string pathRaw)
    {
        return @$"assets/{pathRaw.Substring(2, pathRaw.LastIndexOf(Path.GetExtension(path)) - 2)}";
    }

    void Reeverb()
    {
        _currentDecayTime = Mathf.MoveTowards(_currentDecayTime, 7.0f, 2f * Time.deltaTime);
        _reverbFilter.decayTime = _currentDecayTime;

        bool available = ReeverbIntervals.Count > 0;

        if (available) available = MusicPlayer.time >= EndReverbTime;
        else available = _currentDecayTime == 7.0f;

        if (available)
        {
            MusicPlayer.mute = true;
            _preservedMusicTime = MusicPlayer.time;
            IsReeverb = false;
        }
    }

    public void TMPDOText(TextMeshProUGUI text, float duration)
    {
        _readAll = false;
        text.maxVisibleCharacters = 0;

        var ease = Ease.Linear;
        Enum.TryParse(SettingsManager.Settings.UI.TextEase, out ease);

        Tween.Custom(0f, text.text.Length, duration, x => {
            if (!_readAll) text.maxVisibleCharacters = (int)x;
        }, ease);
    }

    public static T GetVariable<T>(string name, ref Dictionary<string, T> local, ref Dictionary<string, T> global)
    {
        if (local.ContainsKey(name)) return local[name];
        if (global.ContainsKey(name)) return global[name];
        return default;
    }

    public static List<T> CombineValues<T>(ref Dictionary<string, T> local, ref Dictionary<string, T> global, Func<T, List<T>, bool> compare = null)
    {
        var list = new List<T>(local.Values);
        
        foreach (var g in global.Values)
        {
            if (compare == null || !compare(g, list))
            {
                list.Add(g);
            }
        }

        return list;
    }
}
