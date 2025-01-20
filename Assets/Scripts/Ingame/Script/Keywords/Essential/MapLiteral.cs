using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectMGG.Ingame.Script.Keywords
{
    public class MapLiteral : IExpression
    {
        public Dictionary<string, IExpression> Values { get; set; } = new Dictionary<string, IExpression>();

        public object Interpret()
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            foreach (string key in Values.Keys)
            {
                result[key] = Values[key].Interpret();
            }
            return result;
        }
    }
}