using System.Collections;
using System.Collections.Generic;

namespace ProjectMGG.Ingame.Script.Keywords
{
    public interface IStatement
    {
        int Line { get; set; }
        void Interpret();
    }
    // ½Ä
}