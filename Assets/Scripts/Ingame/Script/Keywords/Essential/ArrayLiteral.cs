using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectMGG.Ingame.Script.Keywords
{
    public class ArrayLiteral : IExpression
    {
        public List<IExpression> Values { get; set; } = new List<IExpression>();

        public void Print(int depth)
        {
            Debug.Log("Script/Print: " + new string(' ', depth * 2));
            Debug.Log("Script/Print: [");

            foreach (IExpression node in Values)
            {
                node.Print(depth + 1);
            }

            Debug.Log("Script/Print: " + new string(' ', depth * 2));
            Debug.Log("Script/Print: ]");
        }

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