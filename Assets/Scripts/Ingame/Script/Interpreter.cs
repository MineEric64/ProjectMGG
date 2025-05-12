using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

using ProjectMGG.Ingame.Script.Keywords;

namespace ProjectMGG.Ingame.Script
{
    public class Interpreter
    {
        private const string ENTRY_POINT = "start";

        public static Dictionary<string, Function> FunctionTable { get; set; } = new Dictionary<string, Function>();
        
        public static Function EntryPoint { get; private set; }
        public static Function CurrentPoint { get; set; }
        public static Stack<Function> FramePointers { get; private set; } = new Stack<Function>();

        public void Interpret(Program program)
        {
            FunctionTable.Clear();

            foreach (Function node in program.Functions)
            {
                FunctionTable[node.Name] = node;
            }

            if (!FunctionTable.ContainsKey(ENTRY_POINT))
            {
                ExceptionManager.Throw("Doesn't have a entry point 'start'.", "Script/Interpreter");
                return;
            }

            EntryPoint = FunctionTable[ENTRY_POINT];
            IngameManagerV2.Locals.Add(ENTRY_POINT, new VariableCollection());
            CurrentPoint = EntryPoint;

            foreach (var block in program.Blocks)
            {
                block.Interpret();
            }
        }

    }
}