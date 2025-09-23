using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProjectMGG.Ingame.Script.Keywords.Renpy
{
    public class Character : IExpression
    {
        public static Color COLOUR_RED { get; } = new Color(0.8f, 0.2f, 0.24f);
        public static Color COLOUR_YELLOW { get; } = new Color(0.96f, 0.76f, 0.37f);

        public static Color DEFAULT_COLOUR { get; } = COLOUR_YELLOW;

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