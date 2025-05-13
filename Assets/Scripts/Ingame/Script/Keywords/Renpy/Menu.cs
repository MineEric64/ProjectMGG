using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectMGG.Ingame.Script.Keywords.Renpy
{
    public class Menu : IStatement
    {
        public int Line { get; set; } = 0;
        public int Count { get; set; } = 0;
        public List<string> Names { get; set; } = new List<string>();
        public List<List<IStatement>> Blocks { get; set; } = new List<List<IStatement>>();

        public void Interpret()
        {
            MenuChoiceManager.Instance.CreateMenu(this);
        }
    }
}