using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using TMPro;
using PrimeTween;

using ProjectMGG.Ingame.Script;
using ProjectMGG.Ingame.Script.Keywords.Renpy;
using ProjectMGG.Ingame.Script.Keywords.Renpy.ATL;
using ProjectMGG.Ingame.Script.Keywords.Renpy.Transitions;
using ProjectMGG.Settings;

using Path = System.IO.Path;
using SizeF = System.Drawing.SizeF;

namespace ProjectMGG.Ingame
{
    public class IngameManagerV2 : MonoBehaviour
    {
        public static IngameManagerV2 Instance { get; private set; } = null;

        public static string ScriptPath { get; set; }
        public static string PlayerName { get; set; } //Full name (성이름, ex: 한세아)
        public static string PlayerName2 { get; set; } //First name (이름, ex: 세아)

        public static Texture2D TextureDefault { get; private set; } = null;

        #region Script & TextTag
        private List<Token> _tokens;
        [SerializeField] private List<string> _tokensDebug;

        public static Ease DefaultEase { get; private set; } = Ease.Linear;
        public static bool IsSkipping { get; private set; } = false; //Because of goto command

        public static Dictionary<string, VariableCollection> Locals { get; private set; } //Key: FunctionName
        public static VariableCollection Local => Locals?.GetValueOrDefault(Interpreter.CurrentPoint?.Name ?? string.Empty) ?? Global;
        public static VariableCollection Global { get; private set; } = new VariableCollection();
        public Interpreter Interpreter;

        public Dictionary<string, RawImage> ImageChild { get; private set; } = new Dictionary<string, RawImage>(); //key: gameobject's name, cached & using on show
        public List<Tuple<string, string>> Histories = new List<Tuple<string, string>>(); //same as Text Log (first: character name, second: dialog text)

        private List<TextTag> _textTags = new List<TextTag>();
        [SerializeField] private List<string> _textTagsDebug;
        private int _tagIndex = 0;
        private bool _noWait = false;
        #endregion
        #region Audio
        public AudioSource MusicPlayer;
        public RpyAudio CurrentMusic { get; private set; } = null;

        public bool IsReeverb = false;
        public List<float> ReeverbIntervals = new List<float>();
        public float EndReverbTime = 0.0f;

        private float _preservedMusicTime = 0.0f;
        private AudioReverbFilter _reverbFilter;
        private float _currentDecayTime = 0.1f;

        public AudioSource SoundPlayer; //deprecated
        #endregion
        #region UI
        //based on QHD (refers to issue #20)
        public const int SCREEN_WIDTH = 2560;
        public const int SCREEN_HEIGHT = 1440;

        public GameObject CanvasDefault;
        public GameObject CanvasMenu; //Pause Menu

        public GameObject MenuUI;

        public CanvasGroup CanvasDefaultGroup; ///Screen
        public CanvasGroup CanvasDialogUIGroup;
        public CanvasGroup MenuUIGroup;

        public TextMeshProUGUI NameUI;
        public RawImage NameBackgroundUI;
        public TextMeshProUGUI ContentUI;
        public RawImage CharacterSample;
        public RawImage DownArrow;

        public bool WindowAuto = true;

        #region FX
        public PostProcessVolume FxVolume;
        private DepthOfField FxBlur;
        private ColorGrading FxColorGrading;
        #endregion
        #endregion
        #region Text & UI
        private GraphicRaycaster _raycaster;
        /// <summary>
        /// If false, Click event doesn't appear
        /// </summary>
        public bool Focused { get; set; } = true;
        
        private bool _goToNext = true;
        private bool _readAll = false;
        private int _maxAllTextLength = 0; //used on set _readAll to true
        private int _maxTextLength = 0; //used on get _readAll
        #endregion

        void Awake()
        {
            // Get both of the components we need to do this
            _raycaster = CanvasDefault.GetComponent<GraphicRaycaster>();
            if (TextureDefault == null) TextureDefault = Texture2D.grayTexture;
        }

        // Start is called before the first frame update
        void Start()
        {
            //Initialize
            if (Instance == null) //Initialize Once only
            {
                //Default Texture
                TextureDefault = new Texture2D(634, 1636);
                var colour = new Color(0.5f, 0.5f, 0.5f); //Gray
                Color[] pixels = Enumerable.Repeat(colour, TextureDefault.width * TextureDefault.height).ToArray();
                TextureDefault.SetPixels(pixels);
                TextureDefault.Apply();

                //Pause Manager
                PauseManager.OnCompleted += (_, _) => { _goToNext = true; }; //blacklist (if something went wrong, please consider change to whitelist)
                StartCoroutine(PauseManager.Loop());

                //Audio Manager
                StartCoroutine(AudioManager.Loop(MusicPlayer));
            }

            Instance = this;
            Locals = new Dictionary<string, VariableCollection>();
            Global = new VariableCollection();
            DefaultEase = ParseEaseFromString(SettingsManager.Settings.UI.TextEase);
            Histories.Clear();

            //Script
            PauseManager.Clear();
            PauseManager.Add(new Pause(15f, true));
            StartCoroutine(InitializeScript()); //Pause will automatically removed after init completed

            RpyTransform.Init();

            //Audio
            AudioManager.Clear();
            _reverbFilter = MusicPlayer.GetComponent<AudioReverbFilter>();

            //UI
            NameUI.text = "";
            NameBackgroundUI.enabled = false;
            ContentUI.text = "";
            DownArrow.enabled = false;
            CanvasDefaultGroup.alpha = 0f;
            Tween.Custom(0f, 1f, 1f, x => CanvasDefaultGroup.alpha = x, Ease.InSine);

            //UI: FX
            FxVolume.profile.TryGetSettings(out FxBlur);
            FxVolume.profile.TryGetSettings(out FxColorGrading);
        }

        private IEnumerator InitializeScript()
        {
            var scanner = new Scanner();
            Parser parser;
            Interpreter = new Interpreter();
            Interpreter.Initialize();

            int scriptType = 0; //0: file, 1: url
            string sourceCode = string.Empty;

            if (ScriptPath.StartsWith("url:"))
            {
                scriptType = 1;
                ScriptPath = ScriptPath.Substring(4);
            }

            switch (scriptType)
            {
                case 0:
                    {
                        if (!File.Exists(ScriptPath))
                        {
                            ExceptionManager.Throw($"Can't read the script because file doesn't exists.\n(File Path: '{ScriptPath}')", "IngameManagerV2/Script");
                            break;
                        }
                        sourceCode = File.ReadAllText(ScriptPath);
                        break;
                    }

                case 1:
                    {
                        UnityWebRequest www = UnityWebRequest.Get(ScriptPath);
                        www.timeout = 5;

                        yield return www.SendWebRequest();
                        if (www.result != UnityWebRequest.Result.Success)
                        {
                            ExceptionManager.Throw($"Can't read the script because of failed url response.\n(URL: '{ScriptPath}'\n(Result: '{www.result}')", "IngameManagerV2/Script");
                            break;
                        }
                        sourceCode = www.downloadHandler.text;
                        break;
                    }
            }

            _tokens = scanner.Scan(sourceCode);
            //_tokensDebug = _tokens.Select(x => x.ToString()).ToList();
            parser = new Parser(ref _tokens);

            var syntaxTree = parser.Parse();
            Interpreter.Interpret(syntaxTree);

            PauseManager.Remove(true);
        }

        // Update is called once per frame
        void Update()
        {
            ClickType downType = ClickType.None;

            //Keyboard
            #region Hotkeys
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (Focused) ShowMenu();
                else HideMenu();
            }
            else if (Input.GetKeyDown(KeyCode.Space)) downType = ClickType.Dialog;
            #endregion
            
            //Mouse
            if (downType == ClickType.None) downType = GetMouseDownType();

            //If unfocused, change to None (for preventing click event overlapped)
            if (!Focused) downType = ClickType.None;

            //Text Handle
            if (ContentUI.text.Length == 0) _readAll = true;
            if (_readAll && _noWait)
            {
                _goToNext = true;
                _noWait = false;
            }

            switch (downType)
            {
                case ClickType.Dialog:
                    {
                        if (!_readAll) //while reading
                        {
                            _readAll = true;
                            ContentUI.maxVisibleCharacters = _maxAllTextLength;
                            //ContentUI.maxVisibleCharacters = _maxTextLength; //uncomment this if you want to show users tag by tag
                        }
                        else if (PauseManager.Paused)
                        {
                            if (PauseManager.Remove())
                            {
                                if (ContentUI.maxVisibleCharacters >= _maxTextLength) break;
                            }
                        }
                        else //if already read then need to go to next
                        {
                            _goToNext = true;
                            _readAll = false;
                        }
                        break;
                    }

                default:
                    break;
            }

            if (_goToNext && !PauseManager.Paused)
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
                    else if (script is Pause pause)
                    {
                        //pause.ActionAfter += () => { _goToNext = true; }; //whitelist
                        LetsPause(pause, true);
                    }
                }
                else
                {
                    //Story End
                    Main();
                }
            }

            if (IsReeverb) Reeverb();
        }

        ClickType GetMouseDownType()
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
                    return ClickType.Dialog;
                    //TODO: if other button is touched, return 2~
                    //if (result.gameObject.name == "DialogUI") return true;
                }

                return ClickType.Dialog;
            }

            return ClickType.None;
        }

        #region Keywords: Renpy
        #region Texts
        public void LetsNarration(string content)
        {
            NameUI.text = string.Empty;
            NameBackgroundUI.enabled = false;

            ContentUI.transform.localPosition = new Vector3(62.9156f, -22.304f, ContentUI.transform.position.z);
            ContentUI.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1939.294f);
            ContentUI.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 301.8214f);
            CheckWindowAuto();
            DownArrow.enabled = false;

            StartCoroutine(ProcessText(content));

            _goToNext = false;
        }

        public void LetsNarrationImmediate(string content, bool isMenu = false)
        {
            NameUI.text = string.Empty;
            NameBackgroundUI.enabled = false;

            ContentUI.transform.localPosition = new Vector3(62.9156f, -22.304f, ContentUI.transform.position.z);
            ContentUI.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1939.294f);
            ContentUI.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 301.8214f);
            CheckWindowAuto();
            DownArrow.enabled = false;

            ProcessTextImmediate(content, !isMenu);

            _goToNext = false;
        }

        public void LetsDialog(string chrName, string content)
        {
            var chr = GetVariable(chrName, ref Local.Characters, ref Global.Characters);

            if (chr == null)
            {
                if (!string.IsNullOrWhiteSpace(content))
                {
                    chr = new Character(); //temporary character name
                    chr.Name = new Script.Keywords.StringLiteral(chrName);
                }
                else
                {
                    ExceptionManager.Throw($"Invalid character argument '{chrName}' on dialog.", "IngameManagerV2");
                    return;
                }
            }

            CheckWindowAuto();

            ProcessDialogName(chr);
            ContentUI.transform.localPosition = new Vector3(45.9257f, -22.304f, ContentUI.transform.position.z);
            ContentUI.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1349.591f);
            ContentUI.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 301.8214f);

            StartCoroutine(ProcessText(content, chrName));

            _goToNext = false;
        }

        /// <summary>
        /// Supports the Text Tag
        /// </summary>
        private IEnumerator ProcessText(string text, string chrName = "")
        {
            bool completed = false;
            bool skipNext = false;
            TextTagOption option = new TextTagOption();

            _tagIndex = 0;
            _textTags.Clear();
            _readAll = false;
            _maxAllTextLength = text.Length;

            if (string.IsNullOrEmpty(text))
            {
                ContentUI.text = string.Empty;
                _readAll = true;
                yield break;
            }

            Script.Keywords.StringLiteral.ApplyTag(text, _textTags);
            //_textTagsDebug = _textTags.Select(x => x.ToString()).ToList();

            while (!completed)
            {
                if (!PauseManager.Paused)
                {
                    option.Ease = DefaultEase;
                    option.CPS = SettingsManager.Settings.UI.CPS;

                    bool skip = _tagIndex < _textTags.Count && !string.IsNullOrEmpty(_textTags[_tagIndex].PrimaryData.Tag);
                    if (skip) _readAll = false;
                    if (skipNext)
                    {
                        _readAll = false;
                        skipNext = false;
                    }

                    LetsTextTag(ContentUI, _textTags, ref _tagIndex, out completed, option);
                    yield return TMPDOText(ContentUI, option.StartIndex, option.CPS, option.Ease);

                    if (skip)
                    {
                        _readAll = true;
                        skipNext = true;
                    }

                    option.StartIndex = _maxTextLength;
                }

                //GoTo
                if (!completed) yield return null;
                if (completed && _readAll) ContentUI.maxVisibleCharacters = _maxAllTextLength;
            }

            _readAll = true;
            ShowDownArrow(); //UI
            if (!string.IsNullOrWhiteSpace(ContentUI.text)) Histories.Add(Tuple.Create(chrName, ContentUI.text)); //History
        }

        /// <summary>
        /// without Text Typing effect
        /// </summary>
        private void ProcessTextImmediate(string text, bool showDownArrow = true, string chrName = "")
        {
            bool completed = false;
            TextTagOption option = new TextTagOption();

            _tagIndex = 0;
            _textTags.Clear();
            Script.Keywords.StringLiteral.ApplyTag(text, _textTags);

            while (!completed)
            {
                LetsTextTag(ContentUI, _textTags, ref _tagIndex, out completed, option);
            }

            ContentUI.maxVisibleCharacters = ContentUI.text.Length;
            _maxAllTextLength = text.Length;
            _readAll = true;
            if (showDownArrow) ShowDownArrow(); //UI
            if (!string.IsNullOrWhiteSpace(ContentUI.text)) Histories.Add(Tuple.Create(chrName, ContentUI.text)); //History
        }

        private void ProcessDialogName(Character chr)
        {
            string name = chr.Name.Interpret() as string;
            name = Script.Keywords.StringLiteral.ApplyVariable(name);

            var nameTextTags = new List<TextTag>();
            int nameTagIndex = 0;
            bool completed = false;
            TextTagOption option = new TextTagOption();

            Script.Keywords.StringLiteral.ApplyTag(name, nameTextTags);

            while (!completed)
            {
                LetsTextTag(NameUI, nameTextTags, ref nameTagIndex, out completed, option);
            }
            NameUI.maxVisibleCharacters = NameUI.text.Length;
            NameUI.color = chr.Colour;
            NameBackgroundUI.enabled = true;
        }

        /// <summary>
        /// Interpret Tag + Set Text on UI
        /// </summary>
        private void LetsTextTag(TextMeshProUGUI textUI, List<TextTag> textTags, ref int tagIndex, out bool completed, TextTagOption option)
        {
            completed = tagIndex + 1 >= textTags.Count;

            if (tagIndex >= textTags.Count) return; //Something went wrong

            TextTag tag = textTags[tagIndex];

            //for converting Tag Argument properly (Renpy script -> Text Mesh Pro script)
            #region Predefined
            foreach (var prefix in tag.PrefixPredefined) //General
            {
                switch (prefix.Tag)
                {
                    case "size":
                        {
                            if (prefix.TagArgument is string s)
                            {
                                if (s.StartsWith("*") && float.TryParse(s.Substring(1), out float ratio))
                                {
                                    int ratioRound = (int)(ratio * 100);
                                    prefix.TagArgument = string.Concat(ratioRound, "%");
                                }
                            }
                            break;
                        }
                }
            }

            foreach (var prefix in tag.PrefixPredefinedCustom) //General (Custom)
            {
                switch (prefix.Tag)
                {
                    case "sg":
                        {
                            var sb = new StringBuilder();
                            float x = 0f;
                            bool multiply = false;

                            if (prefix.TagArgument is string s)
                            {
                                if (s.StartsWith("*") && float.TryParse(s.Substring(1), out x)) multiply = true;
                                else float.TryParse(s, out x);
                            }

                            if (x != 0f)
                            {
                                float currentX = x;

                                for (int i = 0; i < tag.Text.Length; i++)
                                {
                                    sb.Append(tag.Text[i]);

                                    if (i != tag.Text.Length - 1)
                                    {
                                        sb.Append("<size=");

                                        if (multiply)
                                        {
                                            currentX *= x;
                                            int ratioRound = (int)(currentX * 100);

                                            sb.Append(ratioRound);
                                            sb.Append("%");
                                        }
                                        else
                                        {
                                            currentX += x;

                                            if (x > 0f) sb.Append("+");
                                            sb.Append((int)currentX);
                                        }

                                        sb.Append(">");
                                    }
                                    else
                                    {
                                        sb.Append("<size=100%>");
                                    }
                                }
                                tag.Text = sb.ToString();
                            }
                            break;
                        }
                }
            }
            #endregion

            string textWithPredefined = tag.GetTextWithPredefined();

            if (tagIndex == 0) textUI.text = textWithPredefined;
            else textUI.text += textWithPredefined;

            #region Dialogue
            switch (tag.PrimaryData.Tag)
            {
                case "w":
                    {
                        Pause pause = Pause.GetInfinity();
                        if (tag.PrimaryData.TagArgument != null) pause.Delay = (float)tag.PrimaryData.TagArgument;

                        pause.ActionAfter = new Action(() =>
                        {
                            _goToNext = false;
                        });
                        LetsPause(pause);
                        break;
                    }

                case "p":
                    {
                        Pause pause = Pause.GetInfinity();
                        if (tag.PrimaryData.TagArgument != null) pause.Delay = (float)tag.PrimaryData.TagArgument;

                        pause.ActionAfter = new Action(() =>
                        {
                            textUI.text += "\n";
                            _goToNext = false;
                        });
                        LetsPause(pause);
                        break;
                    }

                case "nw":
                    {
                        if (tag.PrimaryData.TagArgument != null)
                        {
                            float delay = (float)tag.PrimaryData.TagArgument;

                            Pause pause = new Pause(delay, false);
                            pause.ActionAfter = new Action(() =>
                            {
                                _noWait = true;
                                _goToNext = false;
                            });
                            LetsPause(pause);
                        }
                        else _noWait = true;
                        break;
                    }

                case "fast":
                    {
                        option.StartIndex = textUI.text.Length;
                        break;
                    }

                case "done":
                    {

                        break;
                    }

                case "clear":
                    {

                        break;
                    }

                    //https://www.renpy.org/doc/html/text.html#dialogue-text-tags
            }
            #endregion
            #region General
            foreach (var prefix in tag.PrefixDatas)
            {
                switch (prefix.Tag)
                {
                    case "a":
                        {

                            break;
                        }

                    case "alpha":
                        {

                            break;
                        }

                    case "alt":
                        {

                            break;
                        }

                    case "art":
                        {
                            
                            break;
                        }

                    case "cps":
                        {
                            float x = 0f;
                            bool multiply = false;

                            if (prefix.TagArgument is string s)
                            {
                                if (s.StartsWith("*") && float.TryParse(s.Substring(1), out x)) multiply = true;
                                else float.TryParse(s, out x);
                            }

                            if (multiply) option.CPS *= x;
                            else option.CPS = x;

                            break;
                        }

                    case "size":
                        {
                            //DEPRECATED!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                            //I overlooked the Text Mesh Pro's Tag Text
                            break;

                            textUI.ForceMeshUpdate(true);

                            for (int i = option.StartIndex; i < textUI.textInfo.characterCount; i++)
                            {
                                var info = textUI.textInfo.characterInfo[i];
                                float ds = (float)prefix.TagArgument;

                                info.pointSize += ds;
                            }
                            break;
                        }

                    case "ease":
                        {
                            if (prefix.TagArgument != null)
                            {
                                string name = (string)prefix.TagArgument;
                                if (Enum.TryParse(name, out Ease ease)) option.Ease = ease;
                            }
                            break;
                        }
                }
            }
            #endregion

            tagIndex++;
        }

        public IEnumerator TMPDOText(TextMeshProUGUI text, float start, float cps, Ease ease)
        {
            if (text.text.Length == 0)
            {
                _readAll = true;
                yield break;
            }
            if (_readAll) yield break;

            float end = text.text.Length;
            float duration = 0f;

            bool predefined = false;

            string textToShow = text.text.Substring((int)start);
            predefined = textToShow.Contains('<') && textToShow.Contains('>');

            if (predefined)
            {
                text.ForceMeshUpdate(true);
                end = text.textInfo.characterCount;
            }

            if (cps > 0f) duration = (1 / cps) * (end - start);
            else duration = 0f;
            _maxTextLength = (int)end;

            if (duration == 0f)
            {
                text.maxVisibleCharacters = _maxTextLength;
                yield break;
            }

            var id = Guid.NewGuid().ToString();
            yield return Tween.Custom(id, start, end, duration, (string target, float x) =>
            {
                if (!_readAll) text.maxVisibleCharacters = (int)x;
                else Tween.StopAll(id);
            }, ease).ToYieldInstruction();
        }

        public void LetsWindow(bool show, IPause transition = null, bool immediate = false)
        {
            if (!immediate)
            {
                if (transition == null) transition = new Dissolve(0.5f);
                
                With with = new With(true);
                with.Transition = transition;

                PauseBeforeShow(with);

                if (transition is Dissolve dissolve)
                {
                    if (show) Tween.Alpha(CanvasDialogUIGroup, 0f, 1f, dissolve.GetPauseTime(), Ease.OutSine);
                    else Tween.Alpha(CanvasDialogUIGroup, 1f, 0f, dissolve.GetPauseTime(), Ease.InSine);
                }
                else
                {
                    LetsWindow(show); //Fade Not Supported
                }
            }
            else
            {
                if (show) CanvasDialogUIGroup.alpha = 1f;
                else CanvasDialogUIGroup.alpha = 0f;
            }
        }

        private void CheckWindowAuto()
        {
            if (PauseManager.Paused && string.IsNullOrEmpty(NameUI.text) && string.IsNullOrEmpty(ContentUI.text)) return;
            if (CanvasDialogUIGroup.alpha == 0f) LetsWindow(true, null, true);
        }

        private void ShowDownArrow()
        {
            var id = Guid.NewGuid().ToString();
            
            DownArrow.enabled = true;
            Tween.Custom(id, 0f, 0.5f, 1f, (id, x) => {
                if (_readAll && DownArrow.enabled && !PauseManager.Paused) DownArrow.color = new Color(DownArrow.color.r, DownArrow.color.g, DownArrow.color.b, x);
                else
                {
                    DownArrow.enabled = false;
                    if (!PauseManager.Paused) Tween.StopAll(id);
                }
            }, Ease.InOutSine, -1, CycleMode.Yoyo);
        }
        #endregion
        #region Images
        public IEnumerator LetsShow(Show show, bool emptyDialog = true, string parent = "CanvasImage")
        {
            var image = GetVariable(show.Tag, ref Local.Images, ref Global.Images);
            Texture2D texture = null;

            if (image == null)
            {
                ExceptionManager.Throw($"The image '{show.Tag}' variable doesn't exists while interpreting 'show' statement.", "IngameManagerV2", show.Line);
                yield break;
            }
            if (string.IsNullOrEmpty(show.Attributes)) texture = image.MainImage;
            else
            {
                if (!image.SubImages.TryGetValue(show.Attributes, out var subPath))
                {
                    ExceptionManager.Throw($"The image '{show.Tag}' that has a attribute '{show.Attributes}' variable doesn't exists.", "IngameManagerV2", show.Line);
                    yield break;
                }
                texture = subPath;
            }

            var sceneImages = new List<GameObject>();
            var prefabPrev = GameObject.Find(show.Tag);

            if (show.IsScene) //already adding image object to list (issue #18)
            {
                var canvasImage = this.transform.Find("CanvasImage");

                if (canvasImage == null) Debug.Log("WHY");

                foreach (Transform child in canvasImage)
                {
                    if (child.gameObject.name == show.Tag) continue;
                    sceneImages.Add(child.gameObject);
                }
            }

            RawImage prefab = null;
            bool showed = false;
            var sceneAction = new Action(() =>
            {
                //Destroy all images if scene
                if (show.IsScene && sceneImages.Count > 0)
                {
                    foreach (var sceneImage in sceneImages) Destroy(sceneImage);
                    sceneImages.Clear();
                }

                //Additionally, previous itself
                if (prefabPrev != null) Destroy(prefabPrev);
            });
            var showActionBefore = new Action(() =>
            {
                sceneAction.Invoke();
                prefab = ShowImage(show, texture, parent);

                showed = true;
            });

            PauseBeforeShow(show.With, emptyDialog);
            yield return LetsWithBefore(show.With, true, showActionBefore);
            if (!showed) prefab = ShowImage(show, texture, parent); //equals to showActionAfter
            yield return LetsWithAfter(show.With, true, prefab, sceneAction);
            sceneAction?.Invoke();
        }

        private RawImage ShowImage(Show show, Texture2D texture, string parent = "CanvasImage", bool allowEmptyTexture = false, int defaultWidth = 350, int defaultHeight = 350)
        {
            if (!allowEmptyTexture && texture == null) return null;

            float width = texture?.width ?? defaultWidth;
            float height = texture?.height ?? defaultHeight;

            RawImage prefab = Instantiate(CharacterSample, this.transform.Find(parent));
            prefab.transform.SetAsLastSibling();
            prefab.texture = texture;
            prefab.name = show.Tag;
            prefab.rectTransform.sizeDelta = new Vector3(width, height);

            if (!string.IsNullOrEmpty(show.At))
            {
                var transform = GetVariable(show.At, ref Local.Transforms, ref Global.Transforms);
                if (transform == null)
                {
                    ExceptionManager.Throw($"The transform '{show.At}' variable doesn't exists while interpreting 'show' statement.", "IngameManagerV2", show.Line);
                    return prefab;
                }

                if (transform.Blocks.Count > 0) StartCoroutine(ApplyImageTransform(transform, prefab, width, height));
            }
            else
            {
                prefab.transform.localPosition = new Vector3(0f, -(720 - height / 2));
            }

            //Dialog
            if (show?.With?.Transition is Fade) LetsWindow(true);

            if (ImageChild.ContainsKey(show.Tag)) ImageChild[show.Tag] = prefab;
            else ImageChild.Add(show.Tag, prefab);

            return prefab;
        }

        private IEnumerator ApplyImageTransform(RpyTransform transform, RawImage prefab, float width, float height)
        {
            float time = 0f;
            SizeF textureSize = new SizeF(width, height);
            SizeF textureSizeScaled = new SizeF(textureSize);
            var blocksToExecute = new List<IATL>();

            for (int i = 0; i < transform.Blocks.Count; i++)
            {
                var block = transform.Blocks[i];
                block.ApplyExpression();

                //Init before execution
                foreach (var interiorBlock in block.Interior)
                {
                    interiorBlock.Texture = prefab;
                    interiorBlock.TextureSize = textureSize;

                    interiorBlock.EaseKind = block.EaseKind;
                    interiorBlock.EaseDuration = block.EaseDuration;
                    //interiorBlock.StartDelay = time;

                    blocksToExecute.Add(interiorBlock);
                }

                //Execution (for optimization)
                float timePrevious = time;

                time += block.EaseDuration;

                if (timePrevious < time || i == transform.Blocks.Count - 1)
                {
                    for (int j = 0; j < blocksToExecute.Count; j++)
                    {
                        var interiorBlock = blocksToExecute[j];
                        interiorBlock.TextureSizeScaled = textureSizeScaled;
                        interiorBlock.Interpret();

                        //scaled size changed (ex: zoom)
                        if (interiorBlock.TextureSizeScaled != textureSizeScaled) textureSizeScaled = interiorBlock.TextureSizeScaled;

                        //repeat
                        if (interiorBlock is RpyRepeat repeat)
                        {
                            int count = -1;
                            if (repeat.Value != null && repeat.Value.Interpret() is float temp) count = (int)temp;

                            repeat.CurrentCount++;

                            if (count < 0 || repeat.CurrentCount < count) i = -1; //loop from start
                            break;
                        }
                    }
                    blocksToExecute.Clear();

                    yield return Tween.Delay(block.EaseDuration).ToYieldInstruction();
                }
            }
        }

        private void PauseBeforeShow(With with, bool emptyDialog = true)
        {
            if (with == null) return;

            with.Transition = ParseWithKind(with);
            float time = with.Transition.GetPauseTime();

            if (time > 0f && with.Pause)
            {
                Pause pause = new Pause(time, true);
                //pause.ActionAfter += () => { _goToNext = true; }; //whitelist
                LetsPause(pause, emptyDialog);
            }
        }

        private IPause ParseWithKind(With with)
        {
            var result = with.Transition;

            if (result is Script.Keywords.GetVariable identifier)
            {
                switch ((string)identifier)
                {
                    case "dissolve":
                        result = new Dissolve(0.5f);
                        break;

                    case "fade":
                        result = new Fade(0.5f, 0.0f, 0.5f);
                        break;
                }
            }

            return result;
        }

        public IEnumerator LetsWithBefore(With with, bool isShow, Action showAction = null)
        {
            if (with == null) yield break;
            var result = with.Transition;

            if (result is Fade fade)
            {
                //Dialog
                NameUI.text = "";
                NameBackgroundUI.enabled = true;
                ContentUI.text = "";
                LetsWindow(false);

                if (IsSkipping) //No delay
                {
                    CanvasDefaultGroup.alpha = 1f;
                    showAction?.Invoke();
                    yield break;
                }

                float outTime = fade.OutTime?.Interpret() as float? ?? 0f;
                float holdTime = fade.HoldTime?.Interpret() as float? ?? 0f;
                float inTime = fade.InTime?.Interpret() as float? ?? 0f;

                yield return Tween.Custom(1f, 0f, outTime, x =>
                {
                    CanvasDefaultGroup.alpha = x;
                }, Ease.OutCubic).ToYieldInstruction();

                showAction?.Invoke();
                yield return Tween.Delay(holdTime).ToYieldInstruction();
                yield return Tween.Custom(0f, 1f, inTime, x =>
                {
                    CanvasDefaultGroup.alpha = x;
                }, Ease.InCubic).ToYieldInstruction();
            }
        }

        public IEnumerator LetsWithAfter(With with, bool isShow, RawImage image = null, Action sceneAction = null)
        {
            if (with == null) yield break;
            var result = with.Transition;

            if (result is Dissolve dissolve && image != null)
            {
                if (IsSkipping) //No delay
                {
                    image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
                    sceneAction?.Invoke();
                    yield break;
                }

                float start = isShow ? 0f : 1f;
                float end = isShow ? 1f : 0f;

                yield return Tween.Custom(start, end, result.GetPauseTime(), x =>
                {
                    image.color = new Color(image.color.r, image.color.g, image.color.b, x);
                }, Ease.InSine).ToYieldInstruction();
                sceneAction?.Invoke();
            }
        }

        public IEnumerator LetsHide(Show show, bool emptyDialog = true)
        {
            RawImage prefab = GameObject.Find(show.Tag)?.GetComponent<RawImage>();
            bool showed = false;
            var showAction = new Action(() =>
            {
                ImageChild.Remove(show.Tag);
                Destroy(prefab.gameObject);
                if (show?.With?.Transition is Fade) LetsWindow(true);
                showed = true;
            });

            PauseBeforeShow(show.With, emptyDialog);
            yield return LetsWithBefore(show.With, false, showAction);
            yield return LetsWithAfter(show.With, false, prefab);
            if (!showed) showAction.Invoke();
        }
        #endregion
        #region Audio
        public void LetsPlay(RpyAudio audio, string path, float fadein = 0f, float fadeout = 0f, float volume = 1f)
        {
            switch (audio.Channel)
            {
                case "music":
                    {
                        if (CurrentMusic != null && CurrentMusic.GetFileName() == audio.GetFileName()) //reeverbed
                        {
                            _reverbFilter.enabled = false;
                            MusicPlayer.time = _preservedMusicTime;
                            MusicPlayer.volume = volume;
                            MusicPlayer.mute = false;
                        }
                        else
                        {
                            AudioClip clip = LoadResource<AudioClip>(path, "audio");

                            if (clip != null)
                            {
                                if (fadein > 0f)
                                {
                                    Ease ease = ParseEaseFromString(audio.fadeease);
                                    Tween.AudioVolume(MusicPlayer, 0f, volume, fadein, ease);
                                }
                                else
                                {
                                    MusicPlayer.volume = volume;
                                }

                                MusicPlayer.clip = clip;
                                MusicPlayer.loop = audio.Loop;
                                MusicPlayer.Play();
                                CurrentMusic = audio;
                            }
                        }
                        break;
                    }

                case "sound": //DEPRECATED METHOD!! Let's renewal this code
                    {
                        AudioClip clip = LoadResource<AudioClip>(path, "audio");

                        if (clip != null)
                        {
                            if (fadein > 0f)
                            {
                                Ease ease = ParseEaseFromString(audio.fadeease);
                                Tween.AudioVolume(SoundPlayer, 0f, volume, fadein, ease);
                            }
                            else
                            {
                                SoundPlayer.volume = volume;
                            }

                            SoundPlayer.clip = clip;
                            SoundPlayer.loop = audio.Loop;
                            SoundPlayer.Play();
                        }
                        break;
                    }

                default:
                    ExceptionManager.Throw("TODO: support channel on play keyword", "IngameManagerV2", audio.Line);
                    //reference: Let's use Audio Mixer Group
                    break;
            }
        }

        public void LetsStop(RpyAudio audio, float fadeout = 0f)
        {
            switch (audio.Channel)
            {
                case "music":
                    {
                        if (fadeout > 0f)
                        {
                            Ease ease = ParseEaseFromString(audio.fadeease);
                            Tween.AudioVolume(MusicPlayer, 0f, fadeout, ease).OnComplete(() =>
                            {
                                MusicPlayer.Stop();
                            });
                        }
                        else
                        {
                            MusicPlayer.Stop();
                        }
                        break;
                    }

                case "sound":
                    {
                        if (fadeout > 0f)
                        {
                            Ease ease = ParseEaseFromString(audio.fadeease);
                            Tween.AudioVolume(SoundPlayer, 0f, fadeout, ease).OnComplete(() =>
                            {
                                SoundPlayer.Stop();
                            });
                        }
                        else
                        {
                            SoundPlayer.Stop();
                        }
                        break;
                    }

                default:
                    ExceptionManager.Throw("TODO: support channel on stop keyword", "IngameManagerV2", audio.Line);
                    break;
            }
        }
        #endregion
        #region Etc
        public void LetsPause(Pause pause, bool emptyDialog = false)
        {
            PauseManager.Add(pause);
            if (emptyDialog) LetsNarration(string.Empty);
        }

        public void CallInteriorBlock(IEnumerable<Script.Keywords.IStatement> block)
        {
            var function = new Script.Keywords.Function();
            function.Name = Interpreter.CurrentPoint.Name; //temporary, because of sharing variables
            function.Block = new List<Script.Keywords.IStatement>(block);
            function.Block.Add(new Return());

            Script.Keywords.Call.Interpret(function);
        }
        #endregion
        #endregion
        #region Keywords: Custom
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

        public IEnumerator LetsGoTo(int line)
        {
            int currentLine = 0;
            IsSkipping = true;

            while (currentLine < line)
            {
                var block = Interpreter.CurrentPoint?.GetCurrentBlock();

                if (block == null)
                {
                    IsSkipping = false;
                    yield break;
                }

                currentLine = block.Line;
                if (currentLine >= line)
                {
                    _goToNext = true;
                    _readAll = false;

                    IsSkipping = false;
                    yield break;
                }

                _goToNext = true;
                _readAll = true;

                //Warning: if pause is removed, some transitions (or animations) might not be work properly
                if (PauseManager.Paused) PauseManager.Remove(true);
                if (MenuChoiceManager.Instance.Active) MenuChoiceManager.Instance.OnClick(0); //always click Menu Block 0

                yield return null;
            }

            IsSkipping = false;
        }

        public IEnumerator LetsFX(string name, string at)
        {
            if (!string.IsNullOrEmpty(at))
            {
                var transform = GetVariable(at, ref Local.Transforms, ref Global.Transforms);
                if (transform == null)
                {
                    ExceptionManager.Throw($"The transform '{at}' variable doesn't exists while interpreting 'FX' statement.", "IngameManagerV2");
                    yield break;
                }
            }

            switch (name)
            {
                #region N.C.
                case "NC":
                    {
                        ApplyInternalImage("$nc_frame", "images/fx_nc_frame.png");
                        ApplyInternalImage("$nc_circle1", "images/fx_circle.png");
                        ApplyInternalImage("$nc_circle2", "images/fx_circle.png");
                        ApplyInternalImage("$nc_circle3", "images/fx_circle.png");

                        Show frame = new Show();
                        frame.Tag = "$nc_frame";
                        frame.At = at;
                        frame.With = new With(false);
                        frame.With.Transition = new Dissolve(0.16f);
                        frame.With.Pause = false;

                        RpyTransform t1 = new RpyTransform();
                        ATLBlock a1 = new ATLBlock();
                        t1.Name = "$nc_circle1_t";
                        t1.IsGlobal = true;
                        a1.Interior.Add(new RpyColour("#313131"));
                        a1.Interior.Add(new RpyZoom(0.12f));
                        a1.Interior.Add(new RpyCenter(0.428f, true));
                        a1.Interior.Add(new RpyCenter(0.5f, false));
                        t1.Blocks.Add(a1);
                        t1.Interpret(); //Add

                        RpyTransform t2 = new RpyTransform();
                        ATLBlock a2 = new ATLBlock();
                        t2.Name = "$nc_circle2_t";
                        t2.IsGlobal = true;
                        a2.Interior.Add(new RpyColour("#313131"));
                        a2.Interior.Add(new RpyZoom(0.12f));
                        a2.Interior.Add(new RpyCenter(0.498f, true));
                        a2.Interior.Add(new RpyCenter(0.5f, false));
                        t2.Blocks.Add(a2);
                        t2.Interpret();

                        RpyTransform t3 = new RpyTransform();
                        ATLBlock a3 = new ATLBlock();
                        t3.Name = "$nc_circle3_t";
                        t3.IsGlobal = true;
                        a3.Interior.Add(new RpyColour("#313131"));
                        a3.Interior.Add(new RpyZoom(0.12f));
                        a3.Interior.Add(new RpyCenter(0.568f, true));
                        a3.Interior.Add(new RpyCenter(0.5f, false));
                        t3.Blocks.Add(a3);
                        t3.Interpret();

                        Show circle1 = new Show();
                        circle1.Tag = "$nc_circle1";
                        circle1.At = "$nc_circle1_t";
                        circle1.With = new With(false);
                        circle1.With.Transition = new Dissolve(0.275f);
                        circle1.With.Pause = false;

                        Show circle2 = new Show();
                        circle2.Tag = "$nc_circle2";
                        circle2.At = "$nc_circle2_t";
                        circle2.With = new With(false);
                        circle2.With.Transition = new Dissolve(0.275f);
                        circle2.With.Pause = false;

                        Show circle3 = new Show();
                        circle3.Tag = "$nc_circle3";
                        circle3.At = "$nc_circle3_t";
                        circle3.With = new With(false);
                        circle3.With.Transition = new Dissolve(0.275f);
                        circle3.With.Pause = false;

                        yield return LetsShow(frame, false);

                        for (int i = 0; i < 2; i++)
                        {
                            circle1.With = null;
                            Coroutine a = StartCoroutine(LetsShow(circle1, false, "CanvasImage/$nc_frame"));
                            Coroutine b = StartCoroutine(Tween.Delay(0.33f).ToYieldInstruction());

                            yield return a;
                            yield return b;

                            circle1.With = new With(false);
                            circle1.With.Transition = new Dissolve(0.275f);
                            circle1.With.Pause = false;
                            circle2.With = null;
                            Coroutine c = StartCoroutine(LetsHide(circle1, false));
                            Coroutine d = StartCoroutine(LetsShow(circle2, false, "CanvasImage/$nc_frame"));
                            Coroutine e = StartCoroutine(Tween.Delay(0.33f).ToYieldInstruction());

                            yield return c;
                            yield return d;
                            yield return e;

                            circle2.With = new With(false);
                            circle2.With.Transition = new Dissolve(0.275f);
                            circle2.With.Pause = false;
                            circle3.With = null;
                            Coroutine f = StartCoroutine(LetsHide(circle2, false));
                            Coroutine g = StartCoroutine(LetsShow(circle3, false, "CanvasImage/$nc_frame"));
                            Coroutine h = StartCoroutine(Tween.Delay(0.33f).ToYieldInstruction());

                            yield return f;
                            yield return g;
                            yield return h;

                            circle3.With = new With(false);
                            circle3.With.Transition = new Dissolve(0.275f);
                            circle3.With.Pause = false;
                            if (i != 1) StartCoroutine(LetsHide(circle3, false));
                            else yield return LetsHide(circle3, false);
                        }

                        yield return LetsHide(frame, false);
                        break;
                    }

                case "NC_ONCE":
                    {
                        ApplyInternalImage("$nc_frame", "images/fx_nc_frame.png");
                        ApplyInternalImage("$nc_circle1", "images/fx_circle.png");
                        ApplyInternalImage("$nc_circle2", "images/fx_circle.png");
                        ApplyInternalImage("$nc_circle3", "images/fx_circle.png");

                        Show frame = new Show();
                        frame.Tag = "$nc_frame";
                        frame.At = at;
                        frame.With = new With(false);
                        frame.With.Transition = new Dissolve(0.16f);
                        frame.With.Pause = false;

                        RpyTransform t1 = new RpyTransform();
                        ATLBlock a1 = new ATLBlock();
                        t1.Name = "$nc_circle1_t";
                        t1.IsGlobal = true;
                        a1.Interior.Add(new RpyColour("#313131"));
                        a1.Interior.Add(new RpyZoom(0.12f));
                        a1.Interior.Add(new RpyCenter(0.428f, true));
                        a1.Interior.Add(new RpyCenter(0.5f, false));
                        t1.Blocks.Add(a1);
                        t1.Interpret(); //Add

                        RpyTransform t2 = new RpyTransform();
                        ATLBlock a2 = new ATLBlock();
                        t2.Name = "$nc_circle2_t";
                        t2.IsGlobal = true;
                        a2.Interior.Add(new RpyColour("#313131"));
                        a2.Interior.Add(new RpyZoom(0.12f));
                        a2.Interior.Add(new RpyCenter(0.498f, true));
                        a2.Interior.Add(new RpyCenter(0.5f, false));
                        t2.Blocks.Add(a2);
                        t2.Interpret();

                        RpyTransform t3 = new RpyTransform();
                        ATLBlock a3 = new ATLBlock();
                        t3.Name = "$nc_circle3_t";
                        t3.IsGlobal = true;
                        a3.Interior.Add(new RpyColour("#313131"));
                        a3.Interior.Add(new RpyZoom(0.12f));
                        a3.Interior.Add(new RpyCenter(0.568f, true));
                        a3.Interior.Add(new RpyCenter(0.5f, false));
                        t3.Blocks.Add(a3);
                        t3.Interpret();

                        Show circle1 = new Show();
                        circle1.Tag = "$nc_circle1";
                        circle1.At = "$nc_circle1_t";
                        circle1.With = new With(false);
                        circle1.With.Transition = new Dissolve(0.275f);
                        circle1.With.Pause = false;

                        Show circle2 = new Show();
                        circle2.Tag = "$nc_circle2";
                        circle2.At = "$nc_circle2_t";
                        circle2.With = new With(false);
                        circle2.With.Transition = new Dissolve(0.275f);
                        circle2.With.Pause = false;

                        Show circle3 = new Show();
                        circle3.Tag = "$nc_circle3";
                        circle3.At = "$nc_circle3_t";
                        circle3.With = new With(false);
                        circle3.With.Transition = new Dissolve(0.275f);
                        circle3.With.Pause = false;

                        yield return LetsShow(frame, false);

                        circle1.With = null;
                        Coroutine a = StartCoroutine(LetsShow(circle1, false, "CanvasImage/$nc_frame"));
                        Coroutine b = StartCoroutine(Tween.Delay(0.66f).ToYieldInstruction());

                        yield return a;
                        yield return b;

                        circle1.With = new With(false);
                        circle1.With.Transition = new Dissolve(0.275f);
                        circle1.With.Pause = false;
                        circle2.With = null;
                        Coroutine c = StartCoroutine(LetsHide(circle1, false));
                        Coroutine d = StartCoroutine(LetsShow(circle2, false, "CanvasImage/$nc_frame"));
                        Coroutine e = StartCoroutine(Tween.Delay(0.66f).ToYieldInstruction());

                        yield return c;
                        yield return d;
                        yield return e;

                        circle2.With = new With(false);
                        circle2.With.Transition = new Dissolve(0.275f);
                        circle2.With.Pause = false;
                        circle3.With = null;
                        Coroutine f = StartCoroutine(LetsHide(circle2, false));
                        Coroutine g = StartCoroutine(LetsShow(circle3, false, "CanvasImage/$nc_frame"));
                        Coroutine h = StartCoroutine(Tween.Delay(0.66f).ToYieldInstruction());

                        yield return f;
                        yield return g;
                        yield return h;

                        circle3.With = new With(false);
                        circle3.With.Transition = new Dissolve(0.275f);
                        circle3.With.Pause = false;
                        yield return LetsHide(circle3, false);

                        yield return LetsHide(frame, false);
                        break;
                    }
                #endregion
                #region L.C.
                case "LC":
                    {
                        float currentWhole = 0f;
                        float current = 0f;

                        const float RADIUS = 120f;

                        var circles = new RawImage[8];
                        var circleShows = new Show[8];
                        var status = new int[8];
                        var colorTable1 = new string[8] { "#E9EAEB", "#D2D4D6", "#D2D3D4", "#A7A9AB", "#919395", "#7D7C7F", "#4E4E50", "#4E4E50" };
                        var colorTable2 = new Color[8] {
                            new Color(0.914f, 0.918f, 0.922f), new Color(0.824f, 0.831f, 0.839f), new Color(0.824f, 0.827f, 0.831f), new Color(0.655f, 0.663f, 0.671f),
                            new Color(0.569f, 0.576f, 0.584f), new Color(0.49f, 0.486f, 0.498f), new Color(0.306f, 0.306f, 0.314f), new Color(0.306f, 0.306f, 0.314f)
                        };

                        Show parentShow = new Show();
                        parentShow.Tag = "$lc_blank";
                        parentShow.At = at;

                        var parent = ShowImage(parentShow, null, allowEmptyTexture: true);
                        parent.color = new Color(0f, 0f, 0f, 0f);
                        parent.transform.SetAsLastSibling();

                        for (int i = 0; i < 8; i++)
                        {
                            string circleName = $"$lc_circle{i + 1}";
                            float progress = i / 8f;
                            float t = progress * 2 * Mathf.PI;
                            float x = -Mathf.Sin(t);
                            float y = -Mathf.Cos(t);

                            ApplyInternalImage(circleName, "images/fx_circle.png");

                            RpyTransform tr = new RpyTransform();
                            ATLBlock atl = new ATLBlock();
                            tr.Name = $"{circleName}_t";
                            tr.IsGlobal = true;
                            atl.Interior.Add(new RpyZoom(0.1f));
                            atl.Interior.Add(new RpyCenter((int)(1280 + x * RADIUS), true));
                            atl.Interior.Add(new RpyCenter((int)(720 + y * RADIUS), false));
                            atl.Interior.Add(new RpyColour(colorTable1[i]));
                            tr.Blocks.Add(atl);
                            tr.Interpret(); //Add

                            Show circle = new Show();
                            circle.Tag = circleName;
                            circle.At = tr.Name;

                            yield return LetsShow(circle, false, "CanvasImage/$lc_blank");

                            RawImage circle2 = null;

                            if (!ImageChild.TryGetValue(circleName, out circle2)) circle2 = GameObject.Find(circleName)?.GetComponent<RawImage>(); //alternative, but expensive cost
                            circles[i] = circle2;
                            circleShows[i] = circle;
                            status[i] = i;
                        }

                        while (currentWhole <= 2.2f)
                        {
                            currentWhole += Time.deltaTime;
                            current += Time.deltaTime;

                            if (current >= 0.13f)
                            {
                                for (int i = 0; i < 8; i++)
                                {
                                    status[i]++;
                                    if (status[i] >= 8) status[i] = 0;

                                    if (circles[i] == null) continue; //something went wrong... maybe synchronization issue?
                                    circles[i].color = colorTable2[status[i]];
                                }

                                current = 0f;
                            }

                            yield return null;
                        }

                        for (int i = 0; i < 8; i++) yield return LetsHide(circleShows[i], false);
                        break;
                    }

                case "LC_FRAME":
                    {
                        float currentWhole = 0f;
                        float current = 0f;

                        const float RADIUS = 150f;

                        var circles = new RawImage[8];
                        var circleShows = new Show[8];
                        var status = new int[8];
                        var colorTable1 = new string[8] { "#E9EAEB", "#D2D4D6", "#D2D3D4", "#A7A9AB", "#919395", "#7D7C7F", "#4E4E50", "#4E4E50" };
                        var colorTable2 = new Color[8] {
                            new Color(0.914f, 0.918f, 0.922f), new Color(0.824f, 0.831f, 0.839f), new Color(0.824f, 0.827f, 0.831f), new Color(0.655f, 0.663f, 0.671f),
                            new Color(0.569f, 0.576f, 0.584f), new Color(0.49f, 0.486f, 0.498f), new Color(0.306f, 0.306f, 0.314f), new Color(0.306f, 0.306f, 0.314f)
                        };

                        ApplyInternalImage("$lc_frame", "images/fx_nc_frame.png");

                        Show frame = new Show();
                        frame.Tag = "$lc_frame";
                        frame.At = at;
                        frame.With = new With(false);
                        frame.With.Transition = new Dissolve(0.16f);
                        frame.With.Pause = false;

                        yield return LetsShow(frame, false);

                        for (int i = 0; i < 8; i++)
                        {
                            string circleName = $"$lc_circle{i + 1}";
                            float progress = i / 8f;
                            float t = progress * 2 * Mathf.PI;
                            float x = -Mathf.Sin(t);
                            float y = -Mathf.Cos(t);

                            ApplyInternalImage(circleName, "images/fx_circle.png");

                            RpyTransform tr = new RpyTransform();
                            ATLBlock atl = new ATLBlock();
                            tr.Name = $"{circleName}_t";
                            tr.IsGlobal = true;
                            atl.Interior.Add(new RpyZoom(0.13f));
                            atl.Interior.Add(new RpyCenter((int)(1280 + x * RADIUS), true));
                            atl.Interior.Add(new RpyCenter((int)(720 + y * RADIUS), false));
                            atl.Interior.Add(new RpyColour(colorTable1[i]));
                            tr.Blocks.Add(atl);
                            tr.Interpret(); //Add

                            Show circle = new Show();
                            circle.Tag = circleName;
                            circle.At = tr.Name;

                            yield return LetsShow(circle, false, "CanvasImage/$lc_frame");

                            RawImage circle2 = null;

                            if (!ImageChild.TryGetValue(circleName, out circle2)) circle2 = GameObject.Find(circleName)?.GetComponent<RawImage>(); //alternative, but expensive cost
                            circles[i] = circle2;
                            circleShows[i] = circle;
                            status[i] = i;
                        }

                        while (currentWhole <= 2.2f)
                        {
                            currentWhole += Time.deltaTime;
                            current += Time.deltaTime;

                            if (current >= 0.13f)
                            {
                                for (int i = 0; i < 8; i++)
                                {
                                    status[i]++;
                                    if (status[i] >= 8) status[i] = 0;

                                    if (circles[i] == null) continue; //something went wrong... maybe synchronization issue?
                                    circles[i].color = colorTable2[status[i]];
                                }

                                current = 0f;
                            }

                            yield return null;
                        }

                        var coroutines = new Coroutine[9];

                        for (int i = 0; i < 8; i++)
                        {
                            circleShows[i].With = new With(false);
                            circleShows[i].With.Transition = new Dissolve(0.16f);
                            circleShows[i].With.Pause = false;
                            coroutines[i] = StartCoroutine(LetsHide(circleShows[i], false));
                        }
                        coroutines[8] = StartCoroutine(LetsHide(frame, false));
                        for (int i = 0; i < 9; i++) yield return coroutines[i];

                        break;
                    }
                    #endregion
            }
        }

        public void ApplyInternalImage(string name, string path)
        {
            if (Global.Images.ContainsKey(name)) return; //overlapped

            var image = new Script.Keywords.Renpy.Image();
            image.Tag = name;
            image.Data = new Script.Keywords.StringLiteral(path);
            image.IsGlobal = true;

            image.Interpret();
        }
        #endregion
        #region UI: Button Events
        public void ShowMenu()
        {
            float duration = 0.7f;
            Ease ease = Ease.OutExpo;

            //Default
            Vector3 position = new Vector3(-250, 130);
            Tween.Scale(this.transform, 0.77f, duration, ease)
                .Group(Tween.LocalPosition(this.transform, position, duration, ease));

            float fxBlurStart = FxBlur.focusDistance.value; //default: 10f
            float fxColorGradingStart = FxColorGrading.saturation.value; //default: 0f
            Tween.Custom(fxBlurStart, 4f, duration, x => { FxBlur.focusDistance.value = x; }, ease)
                .Group(Tween.Custom(fxColorGradingStart, -100f, duration, x => { FxColorGrading.saturation.value = x; }, ease));

            Pause pause = Pause.GetInfinity(true);
            pause.ActionAfter = () => { _goToNext = false; };
            PauseManager.Add(pause);
            Focused = false;

            //Menu UI
            Vector3 positionMenuStart = new Vector3(1150, 200);
            Vector3 positionMenuEnd = new Vector3(950, 550);
            Tween.LocalPosition(MenuUI.transform, positionMenuStart, positionMenuEnd, duration, ease)
                .Group(Tween.Custom(0f, 1f, duration, x => { MenuUIGroup.alpha = x; }, ease));
            CanvasMenu.SetActive(true);

            //Audio
            var lowpass = MusicPlayer.GetComponent<AudioLowPassFilter>();
            lowpass.enabled = true;
            Tween.Custom(15000f, 300f, 0.5f, x => lowpass.cutoffFrequency = x, ease);
        }

        public void HideMenu()
        {
            float duration = 0.7f;
            Ease ease = Ease.OutExpo;

            //Default
            Vector3 position = new Vector3(0, 0);
            Tween.Scale(this.transform, 1f, duration, ease)
                .Group(Tween.LocalPosition(this.transform, position, duration, ease));

            float fxBlurStart = FxBlur.focusDistance.value; //default: 4f
            float fxColorGradingStart = FxColorGrading.saturation.value; //default: -100f
            Tween.Custom(fxBlurStart, 10f, duration, x => { FxBlur.focusDistance.value = x; }, ease)
                .Group(Tween.Custom(fxColorGradingStart, 0f, duration, x => { FxColorGrading.saturation.value = x; }, ease));

            Focused = true;
            _goToNext = false;
            PauseManager.Remove(true);

            //Menu UI
            Vector3 positionMenu = new Vector3(1150, 200);
            Tween.LocalPosition(MenuUI.transform, positionMenu, duration, ease)
                .Group(Tween.Custom(1f, 0f, duration, x => { MenuUIGroup.alpha = x; }, ease));
                //.OnComplete(() => { CanvasMenu.SetActive(false); }); //uncomment this if something went wrong about Menu UI (switch flickering issue)

            //Audio
            var lowpass = MusicPlayer.GetComponent<AudioLowPassFilter>();
            float lowpassStart = lowpass.cutoffFrequency; //default: 300f
            Tween.Custom(lowpassStart, 15000f, 0.5f, x => lowpass.cutoffFrequency = x, Ease.InQuad).OnComplete(() => { lowpass.enabled = false; }); //Optimized Ease: InSine or InQuad
        }

        public void Return()
        {
            HideMenu();
        }

        public void History()
        {

        }

        public static void Settings(GameObject prefab)
        {
            Instantiate(prefab);
        }

        public void SettingsIngame()
        {
            Settings(this.gameObject);
        }

        public void Main()
        {
            SceneManager.LoadScene("NameSelect"); //MainMenu
        }

        public void Quit()
        {
            Application.Quit();
        }
        #endregion

        #region Etc Methods
        //load priority reference: https://www.renpy.org/doc/html/audio.html
        public static T LoadResource<T>(string pathRaw, string assetName = "images") where T : UnityEngine.Object
        {
            //Trim
            pathRaw = pathRaw.TrimStart('/');
            pathRaw = pathRaw.TrimStart('\\');
            pathRaw = pathRaw.Trim();

            string path = pathRaw.Replace("/", @"\");
            T t = null;

            //#1. assets/{pathRaw}
            t = Resources.Load<T>(ToResourcePath(path, pathRaw, ""));
            if (t != null) return t;

            //#2. assets/{assetName}/{pathRaw}
            t = Resources.Load<T>(ToResourcePath(path, pathRaw, assetName));
            if (t != null) return t;

            // P.S. extension is not needed on loading resource using Resources.Load()
            // and '$/' syntax is deprecated after v0.5.15

            if (t == null)
            {
                string fileName = Path.GetFileName(path);
                if (string.IsNullOrEmpty(fileName)) fileName = pathRaw;

                ExceptionManager.Throw($"Couldn't load the file '{fileName}'. the file doesn't exists.", "IngameManagerV2");
            }
            return t;
        }

        public static string ToResourcePath(string path, string pathRaw, string assetName)
        {
            var sb = new StringBuilder();
            string ext = Path.GetExtension(path);

            sb.Append("assets/");
            if (!string.IsNullOrWhiteSpace(assetName)) sb.Append($"{assetName}/");

            //without extension (more at P.S.)
            if (!string.IsNullOrWhiteSpace(ext)) sb.Append(pathRaw.Substring(0, pathRaw.LastIndexOf(ext)));
            else sb.Append(pathRaw);
            
            return sb.ToString();
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

        public static Ease ParseEaseFromString(string s)
        {
            if (Enum.TryParse(s, out Ease ease)) return ease;
            return Ease.Default;
            //return Ease.Linear;
        }
        #endregion
    }

    public enum ClickType
    {
        None = 0,
        Dialog = 1,
        Other = 2, //TODO
    }
}