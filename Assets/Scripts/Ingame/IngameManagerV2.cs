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
using DG.Tweening.Plugins.Core.PathCore;

using Path = System.IO.Path;

public class IngameManagerV2 : MonoBehaviour
{
    public static IngameManagerV2 Instance { get; private set; } = null;
    public static VariableCollection Local { get; private set; } = new VariableCollection(); //TODO: every label (using stack)
    public static VariableCollection Global { get; private set; } = new VariableCollection();

    public Interpreter Interpreter;

    public RawImage Background;
    public TextMeshProUGUI NameUI;
    public TextMeshProUGUI ContentUI;
    public AudioSource MusicPlayer;
    public RawImage CharacterSample;
    public float TextAnimationMultiplier = 0.04f;

    private GraphicRaycaster _raycaster;
    private bool _goToNext = true;
    private bool _readAll = false;

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
        List<Token> tokenList = scanner.Scan(sourceCode);
        parser = new Parser(ref tokenList);

        Program syntaxTree = parser.Parse();
        Interpreter.Interpret(syntaxTree);

        //Audio
        _reverbFilter = MusicPlayer.GetComponent<AudioReverbFilter>();

        //UI
        NameUI.text = "";
        ContentUI.text = "";
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
                ExceptionManager.Throw("Error occured while interpreting the script.", "IngameManagerV2/Script");
            }

            var script = Interpreter.CurrentPoint?.GetCurrentBlock();
            var scriptNext = Interpreter.CurrentPoint?.GetNextBlock();

            if (script != null)
            {
                Interpreter.CurrentPoint.Interpret();
            }
            else
            {
                //Story End
                SceneManager.LoadScene("MainMenu");
            }
            if (scriptNext != null)
            {
                //reeverb
            }
        }
        
        if (_isReeverb) Reeverb();
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
        _currentDecayTime = Mathf.MoveTowards(_currentDecayTime, 7.0f, 3f * Time.deltaTime);
        _reverbFilter.decayTime = _currentDecayTime;

        if (_currentDecayTime == 7.0f)
        {
            MusicPlayer.mute = true;
            _preservedMusicTime = MusicPlayer.time;
            _isReeverb = false;
        }
    }

    public void TMPDOText(TextMeshProUGUI text, float duration)
    {
        _readAll = false;
        text.maxVisibleCharacters = 0;

        DOTween.To(x => {
            if (!_readAll) text.maxVisibleCharacters = (int)x;
        }, 0f, text.text.Length, duration).SetEase(Ease.Linear);
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
