using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectMGG.Ingame.Script.Keywords
{
    public class SetElement : IExpression
    {
        public IExpression Sub { get; set; }
        public IExpression Index { get; set; }
        public IExpression Value { get; set; }

        public object Interpret()
        {
            object obj = Sub.Interpret();
            object index = Index.Interpret();
            object value = Value.Interpret();

            if (Datatype.IsArray(obj) && Datatype.IsNumber(index))
            {
                return Datatype.SetValueOfArray(obj, index, value);
            }
            if (Datatype.IsMap(obj) && Datatype.IsString(index))
            {
                return Datatype.SetValueOfMap(obj, index, value);
            }
            return null;
        }
    }
}