using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectMGG.Ingame.Script.Keywords
{
    public class GetElement : IExpression
    {
        public IExpression Sub { get; set; }
        public IExpression Index { get; set; }

        public object Interpret()
        {
            object obj = Sub.Interpret();
            object index = Index.Interpret();

            if (Datatype.IsArray(obj) && index is float)
            {
                return Datatype.GetValueOfArray(obj, index);
            }
            if (Datatype.IsMap(obj) && index is string)
            {
                return Datatype.GetValueOfMap(obj, index);
            }
            return null;
        }
    }
}