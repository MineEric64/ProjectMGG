using SmartFormat;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ProjectMGG.Ingame.Script.Keywords
{
    public class StringLiteral : IExpression
    {
        public string Value { get; set; }
        public static implicit operator string(StringLiteral s) => s.Value;

        public StringLiteral()
        {

        }

        public StringLiteral(string value)
        {
            Value = value;
        }

        public object Interpret()
        {
            return Value;
        }

        /// <summary>
        /// for Renpy, Supports Variable []
        /// </summary>
        /// <returns></returns>
        public static string ApplyVariable(string value)
        {
            bool hasVariable = value.Contains('[') && value.Contains(']');
            if (!hasVariable) return value;

            StringBuilder sb = new StringBuilder();
            StringBuilder identifier = new StringBuilder();

            bool isIdentifier = false;

            foreach (char ch in value)
            {
                switch (ch)
                {
                    case '[':
                        isIdentifier = true;
                        break;

                    case ']':
                        if (identifier.Length > 0)
                        {
                            string name = identifier.ToString();

                            if (string.IsNullOrWhiteSpace(name))
                            {
                                identifier.Clear();
                                break;
                            }

                            GetVariable variable = new GetVariable();
                            variable.Name = name;
                            var obj = variable.Interpret();

                            if (obj != null) sb.Append(obj);
                            else ExceptionManager.Throw($"Variable '{identifier}' is not defined.", "Script/Interpreter/StringLiteral");
                            identifier.Clear();
                        }
                        isIdentifier = false;
                        break;

                    default:
                        if (isIdentifier) identifier.Append(ch);
                        else sb.Append(ch);
                        break;
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// for Renpy, Supports Tag {}
        /// </summary>
        /// <returns></returns>
        public static List<TextTag> ApplyTag(string value)
        {
            bool hasTag = value.Contains('{') && value.Contains('}');
            if (!hasTag) return new List<TextTag> { new TextTag(value) };

            List<TextTag> textTags = new List<TextTag>();
            StringBuilder sb = new StringBuilder();
            StringBuilder tag = new StringBuilder();

            bool isTag = false;

            //ExceptionManager.Throw($"The text tag '?' is not defined.", "Script/Interpreter/StringLiteral");
            //TODO: https://www.renpy.org/doc/html/text.html#dialogue-text-tags

            foreach (char ch in value)
            {
                switch (ch)
                {
                    case '{':
                        isTag = true;
                        break;

                    case '}':
                        if (tag.Length > 0)
                        {
                            var textTag = InterpretTag(sb.ToString(), tag.ToString());
                            textTags.Add(textTag);

                            sb.Clear();
                            tag.Clear();
                        }

                        isTag = false;
                        break;

                    default:
                        if (isTag) tag.Append(ch);
                        else sb.Append(ch);
                        break;
                }
            }
            if (sb.Length > 0)
            {
                textTags.Add(new TextTag(sb.ToString()));
                sb.Clear();
            }

            return textTags;
        }

        private static string[] _predefinedTags = new string[] { "b", "color", "font", "i", "size", "space", "s", "u" };

        private static TextTag InterpretTag(string text, string tagContent)
        {
            var tag = new TextTag(text);
            var scanner = new Scanner();
            var tokens = scanner.Scan(tagContent);

            bool isPredefined = false;
            bool isAssignment = false;

            foreach (var token in tokens)
            {
                switch (token.Kind)
                {
                    case ArgumentKind.Identifier:
                        {
                            string predefined = _predefinedTags.Where(x => token.Content == x).FirstOrDefault();
                            string predefined2 = _predefinedTags.Select(x => string.Concat("/", x)).Where(x => token.Content == x).FirstOrDefault();

                            isPredefined = !string.IsNullOrEmpty(predefined) || !string.IsNullOrEmpty(predefined2);
                            tag.Tag = token.Content;
                            break;
                        }

                    case ArgumentKind.Assignment:
                        isAssignment = true;
                        break;

                    default:
                        {
                            if (isAssignment)
                            {
                                if (isPredefined)
                                {
                                    tag.Text = string.Concat(text, "<", tag.Tag, "=", token.Content, ">");
                                    tag.Tag = string.Empty;
                                    break;
                                }
                                tag.TagArgument = ParseTagArgument(token);
                                //https://www.renpy.org/doc/html/text.html#dialogue-text-tags
                            }
                            break;
                        }
                }
            }

            return tag;
        }

        private static object ParseTagArgument(Token token)
        {
            switch (token.Kind)
            {
                case ArgumentKind.NumberLiteral:
                    {
                        NumberLiteral result = new NumberLiteral();
                        result.Value = float.Parse(token.Content);
                        return result.Interpret();
                    }

                case ArgumentKind.StringLiteral:
                    {
                        StringLiteral result = new StringLiteral();
                        result.Value = token.Content;
                        return result.Interpret();
                    }
            }

            return null;
        }
    }
}