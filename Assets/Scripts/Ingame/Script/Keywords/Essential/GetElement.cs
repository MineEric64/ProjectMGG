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

            if (Datatype.IsArray(obj) && Datatype.IsNumber(index))
            {
                return Datatype.GetValueOfArray(obj, index);
            }
            if (Datatype.IsMap(obj) && Datatype.IsString(index))
            {
                return Datatype.GetValueOfMap(obj, index);
            }
            return null;
        }
    }
}