using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptSyntax
{
    public string EssentialSyntax { get; set; }
    public List<string> Arguments { get; set; }
    //Option

    public ScriptSyntax(string essentialSyntax)
    {
        EssentialSyntax = essentialSyntax;
        Arguments = new List<string>();
    }
}
