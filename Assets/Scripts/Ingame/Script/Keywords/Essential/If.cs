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

                if (!Datatype.IsTrue(result))
                {
                    continue;
                }

                Interpreter.Local[Interpreter.Local.Count - 1].Insert(0, new Dictionary<string, object>());
                foreach (IStatement node in Blocks[i])
                {
                    node.Interpret();
                }
                Interpreter.Local[Interpreter.Local.Count - 1].RemoveAt(0);
                return;
            }

            if (ElseBlock.Count == 0)
            {
                return;
            }

            //else
            Interpreter.Local[Interpreter.Local.Count - 1].Insert(0, new Dictionary<string, object>());
            foreach (IStatement node in ElseBlock)
            {
                node.Interpret();
            }
            Interpreter.Local[Interpreter.Local.Count - 1].RemoveAt(0);
        }
    }
}