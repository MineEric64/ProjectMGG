using System.Collections;
using System.Collections.Generic;

public interface IExpression
{
    void Print(int depth);
    object Interpret();
}
// ½Ä