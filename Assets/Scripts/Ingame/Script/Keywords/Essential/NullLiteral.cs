using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectMGG.Ingame.Script.Keywords
{
    public class NullLiteral : IExpression
    {
        public object Interpret()
        {
            return null;
        }
    }
}