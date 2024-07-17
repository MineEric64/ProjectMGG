using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ScriptInterpreter
{
    public List<ScriptSyntax> Scripts;
    public Dictionary<string, Character> Characters;
    public Dictionary<string, string> Images;
    
    private int _index = 0;

    public ScriptInterpreter(string path)
    {
        Scripts = new List<ScriptSyntax>();
        Characters = new Dictionary<string, Character>();
        Images = new Dictionary<string, string>();

        ConvertInternal(path, ref Scripts, ref Characters, ref Images);
    }

    private static void ConvertInternal(string path, ref List<ScriptSyntax> scripts, ref Dictionary<string, Character> characters, ref Dictionary<string, string> images)
    {
        if (!File.Exists(path))
        {
            Debug.LogError("ScriptInterpreter : Can't read the script because file doesn't exists.");
            return;
        }

        string essentialParent = "";

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
                        characters.Add(chr.NameVar, chr);
                    }
                    break;

                case "image":
                    if (args.Length >= 4)
                    {
                        string filePath = text.Substring(text.IndexOf("=") + 1).Trim().Replace("\"", "");
                        if (characters.TryGetValue(args[1], out var chr))
                        {
                            chr.Images.Add("default", filePath);
                        }
                        else images.Add(args[1], filePath);
                    }
                    break;

                case "transform":
                case "label":
                    essentialParent = args[0];
                    break;
            }
            if (text.Contains("\t"))
            {
                string argWithoutTab = args[0].Replace("\t", "");
                string textWithoutTab = text.Replace("\t", "");

                switch (essentialParent)
                {
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
                                    script.EssentialSyntax = "$narration";
                                    script.Arguments.Add(textWithoutTab.Replace("\"", ""));
                                }
                                else if (textWithoutTab.StartsWith("#"))
                                {
                                    script.EssentialSyntax = "#";
                                    script.Arguments.Add(textWithoutTab.Substring(1).TrimStart());
                                }
                                else if (characters.ContainsKey(argWithoutTab))
                                {
                                    script.EssentialSyntax = "$dialog";
                                    script.Arguments.Add(argWithoutTab);
                                    script.Arguments.Add(textWithoutTab.Substring(argWithoutTab.Length + 2).TrimEnd('"'));
                                }
                                
                                break;
                        }
                        break;

                    case "transform":
                        switch (argWithoutTab)
                        {
                            case "xalign":
                                break;

                            case "yalign":
                                break;

                            case "zoom":
                                break;
                        }
                        break;
                }
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
}
