using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectMGG.Ingame.Script.Keywords
{
    public class ArrayLiteral : IExpression
    {
        public List<IExpression> Values { get; set; } = new List<IExpression>();

        public object Interpret()
        {
            List<object> result = new List<object>();

            foreach (IExpression node in Values)
            {
                result.Add(node.Interpret());
            }
            return result;
        }
    }
}