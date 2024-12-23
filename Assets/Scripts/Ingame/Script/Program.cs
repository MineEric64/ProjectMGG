using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Program
{
    public List<Function> Functions { get; set; } = new List<Function>();
    public List<IStatement> Blocks { get; set; } = new List<IStatement>();
}