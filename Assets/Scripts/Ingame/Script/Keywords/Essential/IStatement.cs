using System.Collections;
using System.Collections.Generic;

namespace ProjectMGG.Ingame.Script.Keywords
{
    public interface IStatement
    {
        void Print(int depth);
        void Interpret();
    }
    // ½Ä
}