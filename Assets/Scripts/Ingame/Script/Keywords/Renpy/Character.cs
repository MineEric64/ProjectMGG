using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProjectMGG.Ingame.Script.Keywords.Renpy
{
    public class Character : IExpression
    {
        public string NameVar { get; set; }
        public string Name { get; set; }
        public Color Colour { get; set; }
        [Obsolete()] public Dictionary<string, string> Images { get; set; }

        public Character()
        {

        }

        public Character(string nameVar, string name, Color colour)
        {
            NameVar = nameVar;
            Name = name;
            Colour = colour;
        }

        public void Print(int depth)
        {
            return;
        }

        public object Interpret()
        {
            return this;
        }

        [Obsolete()]
        public static Character Interpret(string code)
        {
            string[] args = code.Split(" ");
            var chr = new Character("", "", new Color(1.0f, 1.0f, 1.0f));

            if (args.Length >= 4)
            {
                chr.NameVar = args[1];
                string[] characterRaw = code.Substring(code.IndexOf("Character") + 10).Replace(")", "").Split(",");

                for (int i = 0; i < characterRaw.Length; i++)
                {
                    string text = characterRaw[i].Trim();

                    if (i == 0) chr.Name = text.Replace("\"", "");
                    else
                    {
                        string[] subArgs = text.Split("=");

                        if (subArgs.Length >= 2 && subArgs[0] == "color")
                        {
                            if (ColorUtility.TryParseHtmlString(subArgs[1].Replace("\"", ""), out Color colour))
                            {
                                chr.Colour = colour;
                            }
                        }
                    }
                }
            }

            return chr;
        }

        public override string ToString()
        {
            return $"Character(name={Name}, color={Colour})";
        }
    }
}