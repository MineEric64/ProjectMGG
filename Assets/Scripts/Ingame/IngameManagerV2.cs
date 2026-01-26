using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

using PrimeTween;

using ProjectMGG.Ingame.Script;
using ProjectMGG.Ingame.Script.Keywords.Renpy;
using ProjectMGG.Ingame.Script.Keywords.Renpy.Transitions;
using ProjectMGG.Settings;

using Path = System.IO.Path;

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

        private List<TextTag> _textTags = new List<TextTag>();
        [SerializeField] private List<string> _textTagsDebug;
        private int _tagIndex = 0;
        private bool _noWait = false;
        #endregion
        #region Audio
        public AudioSource MusicPlayer;
        public bool IsReeverb = false;
        public List<float> ReeverbIntervals = new List<float>();
        public float EndReverbTime = 0.0f;

        private float _preservedMusicTime = 0.0f;
        private string _currentPlayingMusic = "";
        private AudioReverbFilter _reverbFilter;
        private float _currentDecayTime = 0.1f;
        #endregion
        #region UI
        //based on QHD (refers to issue #20)
        public const int SCREEN_WIDTH = 2560;
        public const int SCREEN_HEIGHT = 1440;

        public CanvasGroup CanvasDefault; ///Screen
        public CanvasGroup CanvasDialogUI;

        public TextMeshProUGUI NameUI;
        public RawImage NameBackgroundUI;
        public TextMeshProUGUI ContentUI;
        public RawImage CharacterSample;

        public bool WindowAuto = true;
        #endregion
        #region Text & UI
        private GraphicRaycaster _raycaster;
        
        private bool _goToNext = true;
        private bool _readAll = false;
        private int _maxAllTextLength = 0; //used on set _readAll to true
        private int _maxTextLength = 0; //used on get _readAll
        #endregion

        void Awake()
        {
            // Get both of the components we need to do this
            _raycaster = GetComponent<GraphicRaycaster>();
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
                var colour = new UnityEngine.Color(0.5f, 0.5f, 0.5f); //Gray
                UnityEngine.Color[] pixels = Enumerable.Repeat(colour, TextureDefault.width * TextureDefault.height).ToArray();
                TextureDefault.SetPixels(pixels);
                TextureDefault.Apply();

                //Pause Manager
                PauseManager.OnCompleted += (_, _) =>
                {
                    _goToNext = true;
                };
                StartCoroutine(PauseManager.Loop());
            }

            Instance = this;
            Locals = new Dictionary<string, VariableCollection>();
            Global = new VariableCollection();
            if (Enum.TryParse(SettingsManager.Settings.UI.TextEase, out Ease ease)) DefaultEase = ease;

            //Script
            PauseManager.Clear();
            PauseManager.Add(new Pause(15f, true));
            StartCoroutine(InitializeScript()); //Pause will automatically removed after init completed

            //Audio
            _reverbFilter = MusicPlayer.GetComponent<AudioReverbFilter>();

            //UI
            NameUI.text = "";
            NameBackgroundUI.enabled = false;
            ContentUI.text = "";
            CanvasDefault.alpha = 0f;
            Tween.Custom(0f, 1f, 1f, x => CanvasDefault.alpha = x, Ease.InSine);
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
            int downType = 0;

            #region Hotkeys
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GoHome();
            }
            else if (Input.GetKeyDown(KeyCode.Space)) downType = 1;

            if (downType == 0) downType = GetMouseDownType();
            #endregion

            if (ContentUI.text.Length == 0) _readAll = true;
            if (_readAll && _noWait)
            {
                _goToNext = true;
                _noWait = false;
            }

            switch (downType)
            {
                case 1: //Dialog
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
                        LetsPause(pause, true);
                    }
                }
                else
                {
                    //Story End
                    GoHome();
                }
            }

            if (IsReeverb) Reeverb();
        }

        /// <summary>
        /// 0: Not Mouse Clicked
        /// 1: Dialog
        /// 2: other (TODO)
        /// </summary>
        int GetMouseDownType()
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

                return 1;
            }

            return 0;
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

            StartCoroutine(ProcessText(content));

            _goToNext = false;
        }

        public void LetsNarrationImmediate(string content)
        {
            NameUI.text = string.Empty;
            NameBackgroundUI.enabled = false;
            ContentUI.transform.localPosition = new Vector3(62.9156f, -22.304f, ContentUI.transform.position.z);
            ContentUI.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1939.294f);
            ContentUI.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 301.8214f);
            CheckWindowAuto();

            ProcessTextImmediate(content);

            _goToNext = false;
        }

        public void LetsDialog(string chrName, string content)
        {
            var chr = GetVariable(chrName, ref Local.Characters, ref Global.Characters);

            if (chr == null)
            {
                if (string.IsNullOrWhiteSpace(content))
                {
                    ExceptionManager.Throw($"Invalid character argument '{chrName}' on dialog.", "IngameManagerV2");
                    return;
                }
                else
                {
                    chr = new Character(); //temporary character name
                    chr.Name = new Script.Keywords.StringLiteral(chrName);
                }
            }

            CheckWindowAuto();

            NameUI.text = chr.Name.Interpret() as string;
            NameUI.color = chr.Colour;
            NameBackgroundUI.enabled = true;
            ContentUI.transform.localPosition = new Vector3(45.9257f, -22.304f, ContentUI.transform.position.z);
            ContentUI.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1349.591f);
            ContentUI.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 301.8214f);

            StartCoroutine(ProcessText(content));

            _goToNext = false;
        }

        /// <summary>
        /// Supports the Text Tag
        /// </summary>
        private IEnumerator ProcessText(string text)
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

            Script.Keywords.StringLiteral.ApplyTag(text, ref _textTags);
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

                    LetsTextTag(ContentUI, out completed, ref option);
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
        }

        /// <summary>
        /// without Text Typing effect
        /// </summary>
        private void ProcessTextImmediate(string text)
        {
            bool completed = false;
            TextTagOption option = new TextTagOption();

            _tagIndex = 0;
            _textTags.Clear();
            Script.Keywords.StringLiteral.ApplyTag(text, ref _textTags);

            while (!completed)
            {
                LetsTextTag(ContentUI, out completed, ref option);
            }
            ContentUI.maxVisibleCharacters = ContentUI.text.Length;
            _maxAllTextLength = text.Length;
            _readAll = true;
        }

        /// <summary>
        /// Interpret Tag + Set Text on UI
        /// </summary>
        private void LetsTextTag(TextMeshProUGUI textUI, out bool completed, ref TextTagOption option)
        {
            completed = _tagIndex + 1 >= _textTags.Count;

            if (_tagIndex >= _textTags.Count) return; //Something went wrong

            TextTag tag = _textTags[_tagIndex];

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

            if (_tagIndex == 0) textUI.text = textWithPredefined;
            else textUI.text += textWithPredefined;

            switch (tag.PrimaryData.Tag) //Dialogue
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

            foreach (var prefix in tag.PrefixDatas) //General
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

            _tagIndex++;
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
                    if (show) Tween.Alpha(CanvasDialogUI, 0f, 1f, dissolve.GetPauseTime(), Ease.OutSine);
                    else Tween.Alpha(CanvasDialogUI, 1f, 0f, dissolve.GetPauseTime(), Ease.InSine);
                }
                else
                {
                    LetsWindow(show); //Fade Not Supported
                }
            }
            else
            {
                if (show) CanvasDialogUI.alpha = 1f;
                else CanvasDialogUI.alpha = 0f;
            }
        }

        private void CheckWindowAuto()
        {
            if (PauseManager.Paused && string.IsNullOrEmpty(NameUI.text) && string.IsNullOrEmpty(ContentUI.text)) return;
            if (CanvasDialogUI.alpha == 0f) LetsWindow(true, null, true);
        }
        #endregion
        #region Images
        public IEnumerator LetsShow(Show show, bool emptyDialog = true, string parent = "CanvasImage")
        {
            var image = GetVariable(show.Tag, ref Local.Images, ref Global.Images);
            Texture2D texture = null;

            if (image == null)
            {
                ExceptionManager.Throw($"The image '{show.Tag}' variable doesn't exists while interpreting 'show' statement.", "IngameManagerV2");
                yield break;
            }
            if (string.IsNullOrEmpty(show.Attributes)) texture = image.MainImage;
            else
            {
                if (!image.SubImages.TryGetValue(show.Attributes, out var subPath))
                {
                    ExceptionManager.Throw($"The image '{show.Tag}' that has a attribute '{show.Attributes}' variable doesn't exists.", "IngameManagerV2");
                    yield break;
                }
                texture = subPath;
            }

            var sceneImages = new List<GameObject>();
            var prefabPrev = GameObject.Find(show.Tag);

            if (show.IsScene) //already adding image object to list (issue #18)
            {
                var canvasImage = this.transform.Find("CanvasImage");

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

        private RawImage ShowImage(Show show, Texture2D texture, string parent = "CanvasImage")
        {
            if (texture == null) return null;

            RawImage prefab = Instantiate(CharacterSample, this.transform.Find(parent));
            prefab.transform.SetAsLastSibling();
            prefab.texture = texture;
            prefab.name = show.Tag;
            prefab.rectTransform.sizeDelta = new Vector3(texture.width, texture.height);

            if (!string.IsNullOrEmpty(show.At))
            {
                var transform = GetVariable(show.At, ref Local.Transforms, ref Global.Transforms);
                if (transform == null)
                {
                    ExceptionManager.Throw($"The transform '{show.At}' variable doesn't exists while interpreting 'show' statement.", "IngameManagerV2");
                    return prefab;
                }

                float width = texture.width * transform.zoom;
                float height = texture.height * transform.zoom;

                if (transform.zoom != 1f)
                {
                    prefab.transform.localScale = new Vector3(transform.zoom, transform.zoom);
                    prefab.transform.localPosition = new Vector3(0f, -(720 - height / 2));
                }

                //0~1: ratio, 1~: absolute value
                //pos
                if (transform.xpos != -1f)
                {
                    float offset = (transform.xanchor - 0.5f) * width;
                    if (transform.xanchor > 1f) offset = transform.xanchor - (texture.width / 2);

                    if (transform.xpos >= 0f && transform.xpos <= 1f) prefab.transform.localPosition = new Vector3((1280 * (transform.xpos - 0.5f) * 2) - offset, prefab.transform.localPosition.y);
                    else prefab.transform.localPosition = new Vector3(transform.xpos - offset - 1280, prefab.transform.localPosition.y);
                }
                if (transform.ypos != -1f)
                {
                    float offset = (transform.yanchor - 0.5f) * height;
                    if (transform.yanchor > 1f) offset = transform.yanchor - (texture.height / 2);

                    if (transform.ypos >= 0f && transform.ypos <= 1f) prefab.transform.localPosition = new Vector3(prefab.transform.localPosition.x, (-(720 * (transform.ypos - 0.5f) * 2)) + offset);
                    else prefab.transform.localPosition = new Vector3(prefab.transform.localPosition.x, -(transform.ypos - offset - 720));
                }
                
                //center
                if (transform.xcenter != -1f)
                {
                    if (transform.xcenter >= 0f && transform.xcenter <= 1f) prefab.transform.localPosition = new Vector3(1280 * (transform.xcenter - 0.5f) * 2, prefab.transform.localPosition.y);
                    else prefab.transform.localPosition = new Vector3(transform.xcenter - 1280, prefab.transform.localPosition.y);
                }
                if (transform.ycenter != -1f)
                {
                    if (transform.ycenter >= 0f && transform.ycenter <= 1f) prefab.transform.localPosition = new Vector3(prefab.transform.localPosition.x, -(720 * (transform.ycenter - 0.5f) * 2));
                    else prefab.transform.localPosition = new Vector3(prefab.transform.localPosition.x, -(transform.ycenter - 720));
                }

                //align
                if (transform.xalign != -1f)
                {
                    float offset = (transform.xalign - 0.5f) * width;
                    if (transform.xalign > 1f) offset = transform.xalign - (texture.width / 2);

                    if (transform.xalign >= 0f && transform.xalign <= 1f) prefab.transform.localPosition = new Vector3((1280 * (transform.xalign - 0.5f) * 2) - offset, prefab.transform.localPosition.y);
                    else prefab.transform.localPosition = new Vector3(transform.xalign - offset - 1280, prefab.transform.localPosition.y);
                }
                if (transform.yalign != -1f)
                {
                    float offset = (transform.yalign - 0.5f) * height;
                    if (transform.yalign > 1f) offset = transform.yalign - (texture.height / 2);

                    if (transform.yalign >= 0f && transform.yalign >= 1f) prefab.transform.localPosition = new Vector3(prefab.transform.localPosition.x, (-(720 * (transform.yalign - 0.5f) * 2)) - offset);
                    else prefab.transform.localPosition = new Vector3(prefab.transform.localPosition.x, -(transform.yalign - offset - 720));
                }

                //Custom syntax
                if (!string.IsNullOrEmpty(transform.colour))
                {
                    Color color = Color.white;

                    if (UnityEngine.ColorUtility.TryParseHtmlString(transform.colour, out color)) prefab.color = color;
                    else if (!transform.colour.StartsWith('#') && UnityEngine.ColorUtility.TryParseHtmlString(string.Concat("#", transform.colour), out color)) prefab.color = color; //Concat #
                    else ExceptionManager.Throw($"Color Hex '{transform.colour}' parsing failed while interpreting 'show'/'fx' statement.", "IngameManagerV2");
                }
            }
            else
            {
                prefab.transform.localPosition = new Vector3(0f, -(720 - texture.height / 2));
            }

            //Dialog
            if (show?.With?.Transition is Fade) LetsWindow(true);

            return prefab;
        }

        private void PauseBeforeShow(With with, bool emptyDialog = true)
        {
            if (with == null) return;

            with.Transition = ParseWithKind(with);
            float time = with.Transition.GetPauseTime();

            if (time > 0f)
            {
                Pause pause = new Pause(time, true);
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
                    CanvasDefault.alpha = 1f;
                    showAction?.Invoke();
                    yield break;
                }

                float outTime = fade.OutTime?.Interpret() as float? ?? 0f;
                float holdTime = fade.HoldTime?.Interpret() as float? ?? 0f;
                float inTime = fade.InTime?.Interpret() as float? ?? 0f;

                yield return Tween.Custom(1f, 0f, outTime, x =>
                {
                    CanvasDefault.alpha = x;
                }, Ease.OutCubic).ToYieldInstruction();

                showAction?.Invoke();
                yield return Tween.Delay(holdTime).ToYieldInstruction();
                yield return Tween.Custom(0f, 1f, inTime, x =>
                {
                    CanvasDefault.alpha = x;
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
                Destroy(prefab.gameObject);
                if (show?.With?.Transition is Fade) LetsWindow(true);
                showed = true;
            });

            PauseBeforeShow(show.With, emptyDialog);
            yield return LetsWithBefore(show.With, false, showAction);
            yield return LetsWithAfter(show.With, false, prefab);
            if (!showed) showAction.Invoke();
        }

        public IEnumerator LetsFX(string name, string at)
        {
            if (!string.IsNullOrEmpty(at))
            {
                var transform = GetVariable(at, ref Local.Transforms, ref Global.Transforms);
                if (transform == null)
                {
                    ExceptionManager.Throw($"The transform '{at}' variable doesn't exists while interpreting 'fx' statement.", "IngameManagerV2");
                    yield break;
                }
            }

            switch (name)
            {
                #region N.C.
                case "nc":
                    {
                        ApplyInternalImage("$nc_frame", "$/images/fx_nc_frame.png");
                        ApplyInternalImage("$nc_circle1", "$/images/fx_circle.png");
                        ApplyInternalImage("$nc_circle2", "$/images/fx_circle.png");
                        ApplyInternalImage("$nc_circle3", "$/images/fx_circle.png");

                        Show frame = new Show();
                        frame.Tag = "$nc_frame";
                        frame.At = at;
                        frame.With = new With(false);
                        frame.With.Transition = new Dissolve(0.16f);

                        RpyTransform t1 = new RpyTransform();
                        t1.Name = "$nc_circle1_t";
                        t1.IsGlobal = true;
                        t1.colour = "#313131";
                        t1.zoom = 0.12f;
                        t1.xcenter = 0.428f;
                        t1.ycenter = 0.5f;
                        t1.Interpret(); //Add

                        RpyTransform t2 = new RpyTransform();
                        t2.Name = "$nc_circle2_t";
                        t2.IsGlobal = true;
                        t2.colour = "#313131";
                        t2.zoom = 0.12f;
                        t2.xcenter = 0.498f;
                        t2.ycenter = 0.5f;
                        t2.Interpret();

                        RpyTransform t3 = new RpyTransform();
                        t3.Name = "$nc_circle3_t";
                        t3.IsGlobal = true;
                        t3.colour = "#313131";
                        t3.zoom = 0.12f;
                        t3.xcenter = 0.568f;
                        t3.ycenter = 0.5f;
                        t3.Interpret();

                        Show circle1 = new Show();
                        circle1.Tag = "$nc_circle1";
                        circle1.At = "$nc_circle1_t";
                        circle1.With = new With(false);
                        circle1.With.Transition = new Dissolve(0.275f);

                        Show circle2 = new Show();
                        circle2.Tag = "$nc_circle2";
                        circle2.At = "$nc_circle2_t";
                        circle2.With = new With(false);
                        circle2.With.Transition = new Dissolve(0.275f);

                        Show circle3 = new Show();
                        circle3.Tag = "$nc_circle3";
                        circle3.At = "$nc_circle3_t";
                        circle3.With = new With(false);
                        circle3.With.Transition = new Dissolve(0.275f);

                        yield return LetsShow(frame, false);

                        for (int i = 0; i < 2; i++)
                        {
                            circle1.With = null;
                            Coroutine a = StartCoroutine(LetsShow(circle1, false, "CanvasImage/$nc_frame"));
                            Coroutine b = StartCoroutine(ParallelWaitForSeconds(0.33f));

                            yield return a;
                            yield return b;

                            circle1.With = new With(false);
                            circle1.With.Transition = new Dissolve(0.275f);
                            circle2.With = null;
                            Coroutine c = StartCoroutine(LetsHide(circle1, false));
                            Coroutine d = StartCoroutine(LetsShow(circle2, false, "CanvasImage/$nc_frame"));
                            Coroutine e = StartCoroutine(ParallelWaitForSeconds(0.33f));

                            yield return c;
                            yield return d;
                            yield return e;

                            circle2.With = new With(false);
                            circle2.With.Transition = new Dissolve(0.275f);
                            circle3.With = null;
                            Coroutine f = StartCoroutine(LetsHide(circle2, false));
                            Coroutine g = StartCoroutine(LetsShow(circle3, false, "CanvasImage/$nc_frame"));
                            Coroutine h = StartCoroutine(ParallelWaitForSeconds(0.33f));

                            yield return f;
                            yield return g;
                            yield return h;

                            circle3.With = new With(false);
                            circle3.With.Transition = new Dissolve(0.275f);
                            if (i != 1) StartCoroutine(LetsHide(circle3, false));
                            else yield return LetsHide(circle3, false);
                        }

                        yield return LetsHide(frame, false);
                        break;
                    }

                case "nc_once":
                    {
                        ApplyInternalImage("$nc_frame", "$/images/fx_nc_frame.png");
                        ApplyInternalImage("$nc_circle1", "$/images/fx_circle.png");
                        ApplyInternalImage("$nc_circle2", "$/images/fx_circle.png");
                        ApplyInternalImage("$nc_circle3", "$/images/fx_circle.png");

                        Show frame = new Show();
                        frame.Tag = "$nc_frame";
                        frame.At = at;
                        frame.With = new With(false);
                        frame.With.Transition = new Dissolve(0.16f);

                        RpyTransform t1 = new RpyTransform();
                        t1.Name = "$nc_circle1_t";
                        t1.IsGlobal = true;
                        t1.colour = "#313131";
                        t1.zoom = 0.12f;
                        t1.xcenter = 0.428f;
                        t1.ycenter = 0.5f;
                        t1.Interpret(); //Add

                        RpyTransform t2 = new RpyTransform();
                        t2.Name = "$nc_circle2_t";
                        t2.IsGlobal = true;
                        t2.colour = "#313131";
                        t2.zoom = 0.12f;
                        t2.xcenter = 0.498f;
                        t2.ycenter = 0.5f;
                        t2.Interpret();

                        RpyTransform t3 = new RpyTransform();
                        t3.Name = "$nc_circle3_t";
                        t3.IsGlobal = true;
                        t3.colour = "#313131";
                        t3.zoom = 0.12f;
                        t3.xcenter = 0.568f;
                        t3.ycenter = 0.5f;
                        t3.Interpret();

                        Show circle1 = new Show();
                        circle1.Tag = "$nc_circle1";
                        circle1.At = "$nc_circle1_t";
                        circle1.With = new With(false);
                        circle1.With.Transition = new Dissolve(0.275f);

                        Show circle2 = new Show();
                        circle2.Tag = "$nc_circle2";
                        circle2.At = "$nc_circle2_t";
                        circle2.With = new With(false);
                        circle2.With.Transition = new Dissolve(0.275f);

                        Show circle3 = new Show();
                        circle3.Tag = "$nc_circle3";
                        circle3.At = "$nc_circle3_t";
                        circle3.With = new With(false);
                        circle3.With.Transition = new Dissolve(0.275f);

                        yield return LetsShow(frame, false);

                        circle1.With = null;
                        Coroutine a = StartCoroutine(LetsShow(circle1, false, "CanvasImage/$nc_frame"));
                        Coroutine b = StartCoroutine(ParallelWaitForSeconds(0.66f));

                        yield return a;
                        yield return b;

                        circle1.With = new With(false);
                        circle1.With.Transition = new Dissolve(0.275f);
                        circle2.With = null;
                        Coroutine c = StartCoroutine(LetsHide(circle1, false));
                        Coroutine d = StartCoroutine(LetsShow(circle2, false, "CanvasImage/$nc_frame"));
                        Coroutine e = StartCoroutine(ParallelWaitForSeconds(0.66f));

                        yield return c;
                        yield return d;
                        yield return e;

                        circle2.With = new With(false);
                        circle2.With.Transition = new Dissolve(0.275f);
                        circle3.With = null;
                        Coroutine f = StartCoroutine(LetsHide(circle2, false));
                        Coroutine g = StartCoroutine(LetsShow(circle3, false, "CanvasImage/$nc_frame"));
                        Coroutine h = StartCoroutine(ParallelWaitForSeconds(0.66f));

                        yield return f;
                        yield return g;
                        yield return h;

                        circle3.With = new With(false);
                        circle3.With.Transition = new Dissolve(0.275f);
                        yield return LetsHide(circle3, false);

                        yield return LetsHide(frame, false);
                        break;
                    }
                #endregion
                #region L.C.
                case "lc":
                    {
                        float current = 0f;
                        const float RADIUS = 120f;
                        var circles = new Show[8];
                        var transforms = new RpyTransform[8];
                        var status = new int[8];

                        var parent = new GameObject("$lc_blank");
                        parent.transform.parent = this.transform.Find("CanvasImage");
                        parent.transform.localPosition = new Vector3(0, 0, 0);
                        parent.AddComponent<RectTransform>().sizeDelta = new Vector2(1000, 1000);

                        for (int i = 0; i < 8; i++)
                        {
                            string circleName = $"$lc_circle{i + 1}";
                            float progress = i / 8f;
                            float t = progress * 2 * Mathf.PI;
                            float x = Mathf.Cos(t);
                            float y = Mathf.Sin(t);

                            ApplyInternalImage(circleName, "$/images/fx_circle.png");

                            RpyTransform tr = new RpyTransform();
                            tr.Name = $"{circleName}_t";
                            tr.IsGlobal = true;
                            tr.zoom = 0.1f;
                            tr.xcenter = 1280 + x * RADIUS;
                            tr.ycenter = 720 + y * RADIUS;
                            tr.Interpret(); //Add

                            Show circle = new Show();
                            circle.Tag = circleName;
                            circle.At = tr.Name;

                            circles[i] = circle;
                            transforms[i] = tr;
                            status[i] = i;

                            yield return LetsShow(circle, false, "CanvasImage/$lc_blank");
                            yield return new WaitForSeconds(0.3f);
                        }

                        while (current <= 2f)
                        {
                            current += Time.deltaTime;

                            yield return null;
                        }
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

        private IEnumerator ParallelWaitForSeconds(float seconds)
        {
            yield return new WaitForSeconds(seconds);
        }
        #endregion
        #region Audio
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

        public void LetsStop(string channel)
        {
            switch (channel)
            {
                case "music":
                    {
                        MusicPlayer.Stop();
                        break;
                    }

                default:
                    ExceptionManager.Throw("TODO: support channel on stop keyword", "IngameManagerV2");
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
                if (MenuChoiceManager.Instance.IsWorking) MenuChoiceManager.Instance.OnClick(0); //always click Menu Block 0

                yield return null;
            }

            IsSkipping = false;
        }
        #endregion
        #region UI: Button Events
        public void GoHome()
        {
            SceneManager.LoadScene("MainMenu");
        }
        #endregion

        #region Etc Methods
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
        #endregion
    }
}