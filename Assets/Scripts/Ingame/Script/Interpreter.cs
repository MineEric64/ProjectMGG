using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Interpreter
{
    private const string ENTRY_POINT = "start";

    public static Dictionary<string, object> Global { get; set; } = new Dictionary<string, object>();
    public static List<List<Dictionary<string, object>>> Local { get; set; } = new List<List<Dictionary<string, object>>>();
    public static Dictionary<string, Function> FunctionTable { get; set; } = new Dictionary<string, Function>();

    public static Function EntryPoint { get; private set; }
    public static Function CurrentPoint { get; set; }

    public void Interpret(Program program)
    {
        FunctionTable.Clear();
        Global.Clear();
        Local.Clear();

        foreach (Function node in program.Functions)
        {
            FunctionTable[node.Name] = node;
        }

        Local.Add(new List<Dictionary<string, object>>()); //Function (Stack)
        Local[Local.Count - 1].Insert(0, new Dictionary<string, object>()); //Statement (Queue)

        if (!FunctionTable.ContainsKey(ENTRY_POINT))
        {
            ExceptionManager.Throw("Doesn't have a entry point 'start'.", "Script/Interpreter");
            return;
        }

        EntryPoint = FunctionTable[ENTRY_POINT];
        CurrentPoint = EntryPoint;

        //EntryPoint.Interpret();
        //Local.RemoveAt(Local.Count - 1);
    }

}