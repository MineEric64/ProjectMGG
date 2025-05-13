using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectMGG.Ingame.Script.Keywords
{
    public class If : IStatement
    {
        public int Line { get; set; } = 0;
        public List<IExpression> Conditions { get; set; } = new List<IExpression>();
        public List<List<IStatement>> Blocks { get; set; } = new List<List<IStatement>>();
        public List<IStatement> ElseBlock { get; set; } = new List<IStatement>();

        public void Interpret()
        {
            for (int i = 0; i < Conditions.Count; i++)
            {
                object result = Conditions[i].Interpret();

                if (Datatype.IsFalse(result)) continue;

                IngameManagerV2.Instance.CallInteriorBlock(Blocks[i]);
                return;
            }

            //else
            IngameManagerV2.Instance.CallInteriorBlock(ElseBlock);
        }
    }
}