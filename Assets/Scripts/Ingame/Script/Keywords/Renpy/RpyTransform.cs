using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using ProjectMGG.Ingame.Script.Keywords.Renpy.ATL;

namespace ProjectMGG.Ingame.Script.Keywords.Renpy
{
    public class RpyTransform : IStatement
    {
        public static float xanchor = 0f;
        public static float yanchor = 0f;

        public int Line { get; set; } = 0;
        public string Name { get; set; }
        public bool IsGlobal { get; set; } = false;

        public List<ATLBlock> Blocks { get; set; } = new List<ATLBlock>();

        public RpyTransform()
        {
            //uncomment this if you want to reset anchor every init
            //Init();
        }

        public static void Init()
        {
            xanchor = 0f;
            yanchor = 0f;
        }

        public void Interpret()
        {
            if (string.IsNullOrEmpty(Name))
            {
                ExceptionManager.Throw("The transform name can't be empty.", "Script/RpyTransform", Line);
                return;
            }

            var vars = IsGlobal ? IngameManagerV2.Global : IngameManagerV2.Local;

            if (vars.Transforms.ContainsKey(Name)) vars.Transforms[Name] = this; //overwrite
            else vars.Transforms.Add(Name, this);
        }
    }
}