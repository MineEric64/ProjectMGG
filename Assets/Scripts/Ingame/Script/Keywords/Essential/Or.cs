using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectMGG.Ingame.Script.Keywords
{
    public class Or : IExpression
    {
        public IExpression Lhs { get; set; }
        public IExpression Rhs { get; set; }

        public void Print(int depth)
        {
            Debug.Log("Script/Print: " + new string(' ', depth * 2));
            Debug.Log("Script/Print: OR:");

            Debug.Log("Script/Print: " + new string(' ', (depth + 1) * 2));
            Debug.Log("Script/Print: LHS:");
            Lhs.Print(depth + 2);

            Debug.Log("Script/Print: " + new string(' ', (depth + 1) * 2));
            Debug.Log("Script/Print: RHS:");
            Rhs.Print(depth + 2);
        }

        public object Interpret()
        {
            return Datatype.IsTrue(Lhs.Interpret()) ? true : Rhs.Interpret();
        }
    }
}