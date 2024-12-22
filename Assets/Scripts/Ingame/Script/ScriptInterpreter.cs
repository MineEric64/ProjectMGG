using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;

using SmartFormat;

[Obsolete("This ScriptInterpreter class is deprecated. Please use other class 'ScriptInterpreterV2'.")]
public class ScriptInterpreter
{
    public List<ScriptSyntax> Scripts;
    public Dictionary<string, Character> Characters;
    public Dictionary<string, string> Images;
    public Dictionary<string, transform> Transforms;
    
    private int _index = 0;

    public ScriptInterpreter(string path)
    {
        Scripts = new List<ScriptSyntax>();
        Characters = new Dictionary<string, Character>();
        Images = new Dictionary<string, string>();
        Transforms = new Dictionary<string, transform>();

        ConvertInternal(path, ref Scripts, ref Characters, ref Images, ref Transforms);
    }

    private static void ConvertInternal(string path, ref List<ScriptSyntax> scripts, ref Dictionary<string, Character> characters, ref Dictionary<string, string> images, ref Dictionary<string, transform> transforms)
    {
        if (!File.Exists(path))
        {
            Debug.LogError("ScriptInterpreter : Can't read the script because file doesn't exists.");
            return;
        }

        string essentialParent = "";
        transform transform_ = null;

        foreach (string text in File.ReadAllLines(path))
        {
            if (string.IsNullOrWhiteSpace(text)) continue;

            var script = new ScriptSyntax("#");
            string[] args = text.Split(" ");

            switch (args[0])
            {
                case "define":
                    if (text.Contains("Character"))
                    {
                        var chr = Character.Interpret(text);
                        chr.Name = ConvertToSyntax(chr.Name);
                        characters.Add(chr.NameVar, chr);
                    }
                    break;

                case "image":
                    if (args.Length >= 4)
                    {
                        string filePath = text.Substring(text.IndexOf("=") + 1).Trim().Replace("\"", "");
                        if (characters.TryGetValue(args[1], out var chr))
                        {
                            if (args[2] != "=") chr.Images.Add(args[2], filePath);
                            else chr.Images.Add("default", filePath);
                        }
                        else images.Add(args[1], filePath);
                    }
                    break;

                case "transform":
                    essentialParent = "transform";
                    transform_ = new transform();
                    transforms.Add(args[1].TrimEnd(':').TrimEnd(), transform_);
                    break;

                case "label":
                    essentialParent = "label";
                    break;
            }
            if (text.Contains("\t"))
            {
                string argWithoutTab = args[0].Replace("\t", "");
                string textWithoutTab = text.Replace("\t", "");

                switch (essentialParent)
                {
                    case "transform":
                        float value = -1;

                        if (args.Length >= 2) float.TryParse(args[1], out value);
                        transform_.Options.Add(argWithoutTab, value);
                        break;

                    case "label":
                        switch (argWithoutTab)
                        {
                            case "scene":
                                if (args.Length >= 2)
                                {
                                    script.EssentialSyntax = "scene";
                                    script.Arguments.Add(args[1]);
                                }
                                break;

                            case "show":
                                if (args.Length >= 2)
                                {
                                    script.EssentialSyntax = "show";
                                    script.Arguments.Add(args[1]);

                                    for (int i = 2; i < args.Length; i++)
                                    {
                                        switch (args[i])
                                        {
                                            case "at":
                                                script.Arguments.Add($"at:{args[i + 1]}");
                                                break;

                                            case "with":
                                                script.Arguments.Add($"with:{args[i + 1]}");
                                                break;

                                            default:
                                                if (i == 2) script.Arguments.Add($"image:{args[2]}");
                                                break;
                                        }
                                    }
                                }
                                break;

                            case "play":
                                if (args.Length >= 3)
                                {
                                    string filePath = textWithoutTab.Substring(textWithoutTab.IndexOf("\"") + 1).TrimEnd('"');
                                    script.EssentialSyntax = "play";
                                    script.Arguments.Add(args[1]);
                                    script.Arguments.Add(filePath);
                                }
                                break;

                            case "reeverb":
                                script.EssentialSyntax = "reeverb";
                                break;

                            default:
                                if (textWithoutTab.StartsWith("\"")) {
                                    string textConverted = ConvertToSyntax(textWithoutTab.Replace("\"", ""));

                                    script.EssentialSyntax = "$narration";
                                    script.Arguments.Add(textConverted);
                                }
                                else if (textWithoutTab.StartsWith("#"))
                                {
                                    script.EssentialSyntax = "#";
                                    script.Arguments.Add(textWithoutTab.Substring(1).TrimStart());
                                }
                                else if (characters.ContainsKey(argWithoutTab))
                                {
                                    string textConverted = ConvertToSyntax(textWithoutTab.Substring(argWithoutTab.Length + 2).TrimEnd('"'));

                                    script.EssentialSyntax = "$dialog";
                                    script.Arguments.Add(argWithoutTab);
                                    script.Arguments.Add(textConverted);
                                }
                                
                                break;
                        }
                        break;
                }
            }
            else
            {

            }

            if (script.EssentialSyntax == "#" && script.Arguments.Count == 0) continue; //unsupported feature (TODO)
            scripts.Add(script);
        }

        string fileName = Path.GetFileName(path);
        Debug.Log($"ScriptInterpreter : Converted '{fileName}' successfully.");
    }

    public ScriptSyntax GetNumerator()
    {
        if (_index == Scripts.Count) return null;
        return Scripts[_index++];
    }

    public ScriptSyntax Peek()
    {
        if (_index == Scripts.Count) return null;
        return Scripts[_index];
    }

    private static string ConvertToSyntax(string text)
    {
        string text2 = text;

        text2 = text2.Replace("[playername:은]", Smart.Format("{0:은}", ParamManager.PlayerName));
        text2 = text2.Replace("[playername:는]", Smart.Format("{0:는}", ParamManager.PlayerName));
        text2 = text2.Replace("[playername:이]", Smart.Format("{0:이}", ParamManager.PlayerName));
        text2 = text2.Replace("[playername:가]", Smart.Format("{0:가}", ParamManager.PlayerName));

        text2 = text2.Replace("[playername2:은]", Smart.Format("{0:은}", ParamManager.PlayerName2));
        text2 = text2.Replace("[playername2:는]", Smart.Format("{0:는}", ParamManager.PlayerName2));
        text2 = text2.Replace("[playername2:이]", Smart.Format("{0:이}", ParamManager.PlayerName2));
        text2 = text2.Replace("[playername2:가]", Smart.Format("{0:가}", ParamManager.PlayerName2));
        text2 = text2.Replace("[playername2:야]", Smart.Format("{0:야}", ParamManager.PlayerName2));

        text2 = text2.Replace("[playername]", ParamManager.PlayerName);
        text2 = text2.Replace("[playername2]", ParamManager.PlayerName2);

        return text2;
    }
}
