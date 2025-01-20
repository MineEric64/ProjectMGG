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
        public IExpression Name { get; set; }
        public Color Colour { get; set; }

        public Character()
        {

        }

        public object Interpret()
        {
            return this;
        }

        public override string ToString()
        {
            return $"Character(name={Name}, color={Colour})";
        }
    }
}