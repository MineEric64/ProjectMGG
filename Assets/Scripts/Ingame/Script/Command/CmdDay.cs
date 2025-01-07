using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CmdDay : IStatement
{
    public IExpression FileName { get; set; } = null;

    public void Print(int depth)
    {
        Debug.Log("Script/Print: " + new string(' ', depth * 2));
        Debug.Log("Script/Print: Day");
    }

    public void Interpret()
    {
        string name = (string)FileName.Interpret();
        IntroPlayer.Instance.GoDay(name);
    }
}
