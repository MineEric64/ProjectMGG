using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProjectMGG.Ingame.Script.Keywords.Renpy
{
    public class Character : IExpression, IComparable<Character>, ICloneable
    {
        public static Color COLOUR_RED { get; } = new Color(0.8f, 0.2f, 0.24f);
        public static Color COLOUR_YELLOW { get; } = new Color(0.96f, 0.76f, 0.37f);

        public static Color DEFAULT_COLOUR { get; } = COLOUR_YELLOW;

        public IExpression Name { get; set; }
        public string VariableName { get; set; } = string.Empty;
        public Color Colour { get; set; } = DEFAULT_COLOUR;
        public Guid UUID { get; } = Guid.NewGuid();

        public Character()
        {

        }

        public object Interpret()
        {
            return this;
        }

        public override string ToString()
        {
            string name = Name?.Interpret()?.ToString() ?? string.Empty;
            return name;
            //return $"Character(name={name}, color={Colour})"; //uncomment and replace to this if debugging
        }

        public int CompareTo(Character obj)
        {
            return UUID.CompareTo(obj.UUID);
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}