using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectMGG.Ingame.Script.Keywords
{
    public class Call : IExpression
    {
        public IExpression Sub { get; set; }
        public List<IExpression> Arguments { get; set; } = new List<IExpression>();

        public void Print(int depth)
        {

        }

        public object Interpret()
        {
            Debug.Log("Call -Function TODO");
            return null;
        }
    }
}