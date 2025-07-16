using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace RenpyScriptor
{
    public enum Methods
    {
        Dialog,
        Narration,
        Comment,
    }

    public class ScriptSyntax
    {
        public Methods Method { get; set; }
        public string Name { get; set; }
        public string NameVar { get; set; }
        public string Content { get; set; }

        public ScriptSyntax(Methods method, string name, string nameVar, string content)
        {
            Method = method;
            Name = name;
            NameVar = nameVar;
            Content = content;
        }

        public static ScriptSyntax Interpret(string text)
        {
            var script = new ScriptSyntax(Methods.Narration, "", "", "");

            if (text.Contains(":") && (text.Contains("\"") || text.Contains("”") || text.Contains("“"))) {
                string[] splitted = text.Split(":");

                if (splitted.Length > 1)
                {
                    script.Method = Methods.Dialog;
                    script.Name = splitted[0];
                    script.Content = splitted[1].Replace("\"", "").Replace("”", "").Replace("“", "").Trim();
                }
            }
            else if (text.Contains("(") && text.Contains(")"))
            {
                script.Method = Methods.Comment;
                script.Content = text.Replace("(", "").Replace(")", "");
            }
            else
            {
                script.Content = text;
            }

            return script;
        }

        public string ToRenpy(bool tab = true)
        {
            var sb = new StringBuilder();
            if (tab) sb.Append("\t");

            switch (Method)
            {
                case Methods.Dialog:
                    sb.Append(NameVar);
                    sb.Append(" ");
                    sb.Append("\"");
                    sb.Append(Content);
                    sb.Append("\"");
                    break;

                case Methods.Narration:
                    sb.Append("\"");
                    sb.Append(Content);
                    sb.Append("\"");
                    break;

                case Methods.Comment:
                    sb.Append("# ");
                    sb.Append(Content);
                    break;
            }

            return sb.ToString();
        }
    }
}
