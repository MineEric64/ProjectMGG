using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectMGG.Ingame.Script.Keywords
{
    public class Function : IStatement
    {
        public string Name { get; set; }
        public List<string> Parameters { get; set; } = new List<string>();
        public List<IStatement> Block { get; set; }
        private int _index = 0;

        public void Add(string parameter)
        {
            Parameters.Add(parameter);
        }

        public void Interpret()
        {
            if (_index == Block.Count) return;

            Block[_index].Interpret();
            _index++;

            //foreach (IStatement node in Block) {
            //    node.Interpret();
            //}
        }

        public IStatement GetCurrentBlock()
        {
            if (_index == Block.Count) return null;
            return Block[_index];
        }

        public IStatement GetNextBlock()
        {
            if (_index + 1 >= Block.Count) return null;
            return Block[_index + 1];
        }
    }
}