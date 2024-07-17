using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ScriptInterpreter
{
    public List<ScriptSyntax> Scripts;

    private int _index = 0;

    public ScriptInterpreter(string path)
    {
        Scripts = new List<ScriptSyntax>();
        ConvertInternal(path, ref Scripts);
    }

    private static void ConvertInternal(string path, ref List<ScriptSyntax> scripts)
    {
        if (!File.Exists(path))
        {
            Debug.Log("[Error] ScriptInterpreter : Can't read the script because file doesn't exists.");
            return;
        }

        string essentialParent = "";

        foreach (string text in File.ReadAllLines(path))
        {
            if (string.IsNullOrWhiteSpace(text)) continue;

            var script = new ScriptSyntax("#", new object[] {});
            string[] args = text.Split(" ");

            switch (args[0])
            {
                case "define":
                    if (text.Contains("Character"))
                    {
                        script.Arguments.Add(Character.Interpret(text));
                    }
                    break;

                case "image":
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
                                break;

                            case "show":
                                break;

                            case "play":
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

            scripts.Add(script);
        }

        string fileName = Path.GetFileName(path);
        Debug.Log($"[Info] ScriptInterpreter : Converted '{fileName}' successfully.");
    }

    public ScriptSyntax GetNumerator()
    {
        return Scripts[_index++];
    }
}
