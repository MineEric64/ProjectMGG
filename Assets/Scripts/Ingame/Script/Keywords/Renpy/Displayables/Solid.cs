using UnityEngine;

namespace ProjectMGG.Ingame.Script.Keywords.Renpy
{
    public class Solid : IExpression
    {
        public Color Colour { get; set; }

        public object Interpret()
        {
            return this;
        }

        public override string ToString()
        {
            return $"Solid(color={Colour})";
        }
    }
}