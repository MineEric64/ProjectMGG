using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class CmdInterpreter
{
    public void Interpret(Program program)
    {
        foreach (var block in program.Blocks)
        {
            block.Interpret();
        }
    }
}