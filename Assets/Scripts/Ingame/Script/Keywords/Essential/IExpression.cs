using System.Collections;
using System.Collections.Generic;

namespace ProjectMGG.Ingame.Script.Keywords
{
    public interface IExpression
    {
        void Print(int depth);
        object Interpret();
    }
    // ½Ä
}