using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectMGG.Ingame.Script.Keywords
{
    public class Or : IExpression
    {
        public IExpression Lhs { get; set; }
        public IExpression Rhs { get; set; }

        public object Interpret()
        {
            return Datatype.IsTrue(Lhs.Interpret()) ? true : Rhs.Interpret();
        }
    }
}