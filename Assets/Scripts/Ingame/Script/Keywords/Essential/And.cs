using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectMGG.Ingame.Script.Keywords
{
    public class And : IExpression
    {
        public IExpression Lhs { get; set; }
        public IExpression Rhs { get; set; }

        public object Interpret()
        {
            return Datatype.IsFalse(Lhs.Interpret()) ? false : Rhs.Interpret();
        }
    }
}