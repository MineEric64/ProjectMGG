using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ProjectMGG.Ingame.Script.Keywords.Renpy;

namespace ProjectMGG.Ingame.Script.Keywords
{
    public class Variable : IStatement
    {
        public string Name { get; set; }
        public IExpression Expression { get; set; }
        public bool IsGlobal { get; set; } = false;

        public void Interpret()
        {
            var vars = IsGlobal ? IngameManagerV2.Global : IngameManagerV2.Local;

            if (Expression is Character character)
            {
                character.NameVar = Name;
                vars.Characters.Add(Name, character);
            }
            else
            {
                vars.Others.Add(Name, Expression.Interpret());
                //Interpreter.Local[Interpreter.Local.Count - 1][0][Name] = Expression.Interpret();
            }
        }
    }
}