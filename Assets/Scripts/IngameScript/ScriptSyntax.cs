using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptSyntax
{
    public string EssentialSyntax { get; set; }
    public List<object> Arguments { get; set; }
    //Option

    public ScriptSyntax(string essentialSyntax, object[] arguments)
    {
        EssentialSyntax = essentialSyntax;
        Arguments = new List<object>(arguments);
    }
}
