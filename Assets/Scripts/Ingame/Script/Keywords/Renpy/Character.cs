using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProjectMGG.Ingame.Script.Keywords.Renpy
{
    public class Character : IExpression
    {
        public static Color DEFAULT_COLOUR { get; } = new Color(0.553f, 0.129f, 0.1568f);

        public IExpression Name { get; set; }
        public Color Colour { get; set; } = DEFAULT_COLOUR;

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