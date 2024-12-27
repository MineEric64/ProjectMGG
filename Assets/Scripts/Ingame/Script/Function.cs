using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Function : IStatement
{
    public string Name { get; set; }
    public List<string> Parameters { get; set; } = new List<string>();
    public List<IStatement> Block { get; set; }
    private int _index = 0;

    public void Add(string parameter)
    {
        Parameters.Add(parameter);
    }

    public void Print(int depth)
    {
        Debug.Log("Script/Print: " + new string(' ', depth * 2));
        Debug.Log("Script/Print: FUNCTION " + Name + ": ");

        if (Parameters.Count > 0)
        {
            Debug.Log("Script/Print: " + new string(' ', (depth + 1) * 2));
            Debug.Log("Script/Print: PARAMETERS:");

            foreach (string name in Parameters) {
                Debug.Log("Script/Print: " + name + " ");
            }
            Debug.Log("Script/Print: ");
        }

        Debug.Log("Script/Print: " + new string(' ', (depth + 1) * 2));
        Debug.Log("Script/Print: BLOCK:");

        foreach (IStatement node in Block)
        {
            node.Print(depth + 2);
        }
    }

    public void Interpret()
    {
        if (_index == Block.Count) return;

        Block[_index].Interpret();
        _index++;

        //foreach (IStatement node in Block) {
        //    node.Interpret();
        //}
    }

    public IStatement GetCurrentBlock()
    {
        if (_index == Block.Count) return null;
        return Block[_index];
    }

    public IStatement GetNextBlock()
    {
        if (_index + 1 >= Block.Count) return null;
        return Block[_index + 1];
    }
}