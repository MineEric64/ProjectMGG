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

    public string FindArgument(string syntax, string strIfNotFound = "")
    {
        for (int i = 0; i < Arguments.Count; i++)
        {
            string arg = Arguments[i];
            if (arg.StartsWith(syntax)) return arg.Substring(syntax.Length);
        }
        return strIfNotFound;
    }
}
