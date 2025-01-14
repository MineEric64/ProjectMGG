using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

using ProjectMGG.Ingame.Script.Keywords;

namespace ProjectMGG.Ingame.Script.Commands
{
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
}