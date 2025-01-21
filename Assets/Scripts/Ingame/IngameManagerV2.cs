using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using UnityEngine.EventSystems;
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

        public static VariableCollection Local { get; private set; } = new VariableCollection(); //TODO: every label (using stack)
        public static VariableCollection Global { get; private set; } = new VariableCollection();

        public static string ScriptPath { get; set; }
        public static string PlayerName { get; set; } //성이름
        public static string PlayerName2 { get; set; } //이름

        public CanvasGroup CanvasDefault;

        private List<Token> _tokens;
        [SerializeField] private List<string> _tokensDebug;
        public Interpreter Interpreter;

        [SerializeField]
        private List<TextTag> _textTags;
        private int _tagIndex = 0;

        public TextMeshProUGUI NameUI;
        public TextMeshProUGUI ContentUI;
        public RawImage CharacterSample;
        public float TextAnimationMultiplier = 0.04f;
        public bool NoWait = false;

        public AudioSource MusicPlayer;
        public bool IsReeverb = false;
        public List<float> ReeverbIntervals = new List<float>();
        public float EndReverbTime = 0.0f;

        private float _preservedMusicTime = 0.0f;
        private string _currentPlayingMusic = "";
        private AudioReverbFilter _reverbFilter;
        private float _currentDecayTime = 0.1f;

        private GraphicRaycaster _raycaster;
        private bool _goToNext = true;
        private bool _readAll = false;
        private bool _paused = false;
        private bool _pausedHard = false;

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

            if (!File.Exists(ScriptPath))
            {
                ExceptionManager.Throw($"Can't read the script because file doesn't exists.\n(File Path: '{ScriptPath}')", "IngameManagerV2/Script");
                return;
            }

            string sourceCode = File.ReadAllText(ScriptPath);
            _tokens = scanner.Scan(sourceCode);
            //_tokensDebug = _tokens.Select(x => x.ToString()).ToList();
            parser = new Parser(ref _tokens);

            var syntaxTree = parser.Parse();
            Interpreter.Interpret(syntaxTree);

            //Audio
            _reverbFilter = MusicPlayer.GetComponent<AudioReverbFilter>();

            //UI
            NameUI.text = "";
            ContentUI.text = "";

            CanvasDefault = GetComponent<CanvasGroup>(); //Fade In
            CanvasDefault.alpha = 0f;
            Tween.Custom(0f, 1f, 1f, x => CanvasDefault.alpha = x, Ease.InSine);
        }

        // Update is called once per frame
        void Update()
        {
            #region Hotkeys
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GoHome();
            }
            #endregion

            if (ContentUI.text.Length == 0) _readAll = true;
            else if (!_readAll) _readAll = ContentUI.maxVisibleCharacters == ContentUI.text.Length;
            if (_readAll && NoWait)
            {
                _goToNext = true;
                NoWait = false;
            }

            switch (GetMouseDownType())
            {
                case 1: //Dialog
                    {
                        if (_paused && !_pausedHard)
                        {
                            _goToNext = true;
                            _paused = false;
                            break;
                        }

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
                        break;
                    }

                default:
                    break;
            }

            if (_goToNext && !_paused)
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
                        LetsNarration(string.Empty);
                        StartCoroutine(LetsPause(pause));
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

        public void TMPDOText(TextMeshProUGUI text, float duration)
        {
            if (text.text.Length == 0)
            {
                _readAll = true;
                return;
            }

            _readAll = false;
            text.maxVisibleCharacters = 0;

            var ease = Ease.Linear;
            Enum.TryParse(SettingsManager.Settings.UI.TextEase, out ease);

            Tween.Custom(0f, text.text.Length, duration, x =>
            {
                if (!_readAll) text.maxVisibleCharacters = (int)x;
            }, ease);
        }

        #region Keywords: Renpy
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
                if (string.IsNullOrWhiteSpace(content))
                {
                    ExceptionManager.Throw($"Invalid character argument '{chrName}' on dialog.", "IngameManagerV2");
                    return;
                }
                else
                {
                    chr = new Character() //temporary character name
                    {
                        Name = new Script.Keywords.StringLiteral(chrName),
                        Colour = new Color(0.553f, 0.129f, 0.1568f)
                    };
                }
            }

            NameUI.text = chr.Name.Interpret() as string;
            NameUI.color = chr.Colour;
            ContentUI.text = content;
            TMPDOText(ContentUI, TextAnimationMultiplier * ContentUI.text.Length);

            _goToNext = false;
        }

        public void LetsShow(Show show)
        {
            var image = GetVariable(show.Tag, ref Local.Images, ref Global.Images);
            string resource = "";

            if (image == null)
            {
                ExceptionManager.Throw($"The image '{show.Tag}' variable doesn't exists while interpreting 'show' statement.", "IngameManagerV2");
                return;
            }
            if (string.IsNullOrEmpty(show.Attributes)) resource = image.MainImage;
            else
            {
                if (!image.SubImages.TryGetValue(show.Attributes, out var subPath))
                {
                    ExceptionManager.Throw($"The image '{show.Tag}' that has a attribute '{show.Attributes}' variable doesn't exists.", "IngameManagerV2");
                    return;
                }
                resource = subPath;
            }

            Texture2D texture = LoadResource<Texture2D>(resource);
            RawImage prefab = GameObject.Find(show.Tag)?.GetComponent<RawImage>();

            if (prefab == null)
            {
                prefab = Instantiate(CharacterSample, this.transform.Find("CanvasImage"));
                prefab.transform.SetSiblingIndex(1);
            }

            if (texture != null)
            {
                prefab.texture = texture;
                prefab.name = show.Tag;
                prefab.rectTransform.sizeDelta = new Vector3(texture.width, texture.height);

                if (!string.IsNullOrEmpty(show.At))
                {
                    var transform = GetVariable(show.At, ref Local.Transforms, ref Global.Transforms);
                    if (transform == null)
                    {
                        ExceptionManager.Throw($"The transform '{show.At}' variable doesn't exists while interpreting 'show' statement.", "IngameManagerV2");
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

                LetsWith(show.With, true, prefab);
            }
        }

        public void LetsWith(With with, bool isShow, RawImage image = null)
        {
            if (with == null) return;

            var result = with.Transition.Interpret();

            if (with.Transition is Script.Keywords.GetVariable identifier)
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

            Pause pause = new Pause();
            pause.Delay = 0f;
            pause.Hard = true;

            if (result is Dissolve dissolve)
            {
                float start = isShow ? 0f : 1f;
                float end = isShow ? 1f : 0f;
                float time = (float)dissolve.Time.Interpret();
                pause.Delay = time;

                Tween.Custom(start, end, time, x =>
                {
                    image.color = new Color(image.color.r, image.color.g, image.color.b, x);
                }, Ease.Linear);
            }
            else if (result is Fade fade)
            {
                float endTime = (float)fade.OutTime.Interpret();
                float holdTime = (float)fade.HoldTime.Interpret();
                float inTime = (float)fade.InTime.Interpret();
                pause.Delay = endTime + holdTime + inTime;
                
                Tween.Custom(1f, 0f, endTime, x =>
                {
                    CanvasDefault.alpha = x;
                }, Ease.OutCubic).OnComplete(() =>
                {
                    Tween.Custom(0f, 1f, holdTime, _ => { }).OnComplete(() =>
                    {
                        Tween.Custom(0f, 1f, inTime, x =>
                        {
                            CanvasDefault.alpha = x;
                        }, Ease.InCubic);
                    });
                });
            }

            if (pause.Delay > 0f) StartCoroutine(LetsPause(pause));
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

        public IEnumerator LetsPause(Pause pause)
        {
            _paused = true;
            _pausedHard = pause.Hard;
            yield return new WaitForSeconds((float)pause.Delay);

            if (_paused) //if not paused already (for hard)
            {
                _paused = false;
                _goToNext = true;
            }
        }

        private void LetsTextTag(out bool completed)
        {
            //_tagIndex (like script)
            completed = _tagIndex >= 
        }
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