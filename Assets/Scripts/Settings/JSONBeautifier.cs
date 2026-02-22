/*
* code based on JSON_Beautify.cs (https://github.com/joedf/JSON_BnU)
* by Joe DF
* 
* Modified by MineEric64
*/
using System;
using System.Text;

namespace ProjectMGG.Settings
{
    public class JSONBeautifier
    {
        public static string Uglify(string JSON)
        {
            JSON = JSON.Trim();
            int len = JSON.Length;
            if (len == 0)
                return "";
            StringBuilder j = new StringBuilder(JSON);
            j.Replace(Environment.NewLine, string.Empty);
            j.Replace("\n", string.Empty);
            j.Replace("\r", string.Empty);
            j.Replace("\t", string.Empty);
            j.Replace("\f", string.Empty);
            j.Replace("\b", string.Empty);
            j.Replace("\\\\", @"\1");  // watchout for escape sequence '\\', convert to '\1'
            JSON = j.ToString();

            string _JSON = string.Empty;
            bool in_str = false;
            int c;
            char ch;
            char l_char = '\0';

            for (c = 0; c < len; c++)
            {
                ch = JSON[c];
                if ((!in_str) && (ch == ' '))
                    continue;
                if ((ch == '\"') && (l_char != '\\'))
                    in_str = (!in_str);
                l_char = ch;
                _JSON += ch.ToString();
            }
            _JSON = _JSON.Replace(@"\1", "\\\\");  // convert '\1' back to '\\'
            return _JSON;
        }

        public static string Beautify(string JSON, string gap = "4")
        {
            //fork of http://pastebin.com/xB0fG9py
            JSON = Uglify(JSON);
            JSON = JSON.Replace("\\\\", @"\1");  // watchout for escape sequence '\\', convert to '\1'

            string indent = string.Empty;

            //gap string parse to int
            int _gap = 0;
            if (int.TryParse(gap, out _gap) == true)
            {
                int i = 0;
                while (i < _gap)
                {
                    indent += " ";
                    i += 1;
                }
            }
            else
            {
                indent = gap;
            }

            //json beautify
            string _JSON = string.Empty;
            bool in_str = false;
            int k = 0; //the number of current depth(indent)
            int c;
            int x;
            string _s = string.Empty;
            char ch = '\0';
            char l_char = '\0'; //previous ch
            int len = JSON.Length;
            string nl = Environment.NewLine;

            for (c = 0; c < len; c++)
            {
                ch = JSON[c];

                if (!in_str)
                {
                    if ((ch == '{') || (ch == '['))
                    {
                        _s = string.Empty;
                        ++k;
                        for (x = 1; x < (k) + 1; x++)
                            _s += indent;

                        _JSON += ch.ToString() + nl + _s;
                        continue;
                    }
                    else if ((ch == '}') || (ch == ']'))
                    {
                        _s = string.Empty;
                        --k;
                        for (x = 1; x < (k) + 1; x++)
                            _s += indent;

                        _JSON += nl + _s + ch.ToString();
                        continue;
                    }
                    else if ((ch == ','))
                    {
                        _s = string.Empty;
                        for (x = 1; x < (k) + 1; x++)
                            _s += indent;

                        _JSON += ch.ToString() + nl + _s;
                        continue;
                    }
                    else if (ch == ':') //optional for prettier print
                    {
                        _JSON += ch.ToString() + ' ';
                        continue;
                    }
                }

                if ((ch == '\"') && (l_char != '\\')) in_str = (!in_str);

                l_char = ch;
                _JSON += ch.ToString();
            }
            _JSON = _JSON.Replace(@"\1", "\\\\");  // convert '\1' back to '\\'
            return _JSON;
        }
    }
}