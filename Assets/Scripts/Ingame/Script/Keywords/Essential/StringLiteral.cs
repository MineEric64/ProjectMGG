using ProjectMGG.Ingame.Script.Keywords.Renpy;
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
        public static void ApplyTag(string value, ref List<TextTag> textTags)
        {
            bool hasTag = value.Contains('{') && value.Contains('}');
            if (!hasTag)
            {
                textTags.Add(new TextTag(value));
                return;
            }

            StringBuilder sb = new StringBuilder();
            StringBuilder tag = new StringBuilder();
            HashSet<TextTagData> prefixes = new HashSet<TextTagData>();
            HashSet<TextTagData> predefined = new HashSet<TextTagData>();

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
                            var textTag = InterpretTag(sb.ToString(), tag.ToString(), ref prefixes, ref predefined);
                            if (textTag != null) textTags.Add(textTag);

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
                var textTag = new TextTag(sb.ToString());
                textTag.PrefixDatas = new HashSet<TextTagData>(prefixes);
                textTags.Add(textTag);
                sb.Clear();
            }
        }

        private static string[] _predefinedTags = new string[] { "b", "color", "font", "i", "size", "space", "s", "u" };
        private static string[] _tagType1 = new string[] { "a", "alpha", "alt", "art", "b", "color", "cps", "font", "i", "image", "k", "noalt", "outlinecolor", "plain", "rb", "rt", "s", "shader", "size", "space", "u", "vspace", "#"};
        private static string[] _tagType2 = new string[] { "w", "p", "nw", "fast", "done", "clear", "ease" };

        /// <summary>
        /// Interpret each text and tag like: {tag}<tag3>{text}</tag3>{tag2}{/tag}
        /// </summary>
        private static TextTag InterpretTag(string text, string tag, ref HashSet<TextTagData> prefixes, ref HashSet<TextTagData> predefined)
        {
            TextTag textTag = new TextTag();
            TextTagData primary = new TextTagData();
            TextTagData tag2 = InterpretTagOnly(tag);
            int tagType = ParseTextTagType(tag2.Tag);

            if (tagType == 1) //Dialogue
            {
                primary = tag2;
            }

            textTag.Text = text;
            textTag.PrimaryData = primary;
            textTag.PrefixDatas = new HashSet<TextTagData>(prefixes);
            textTag.PrefixPredefined = new HashSet<TextTagData>(predefined);

            if (tag.StartsWith("/")) //Closed Tag
            {
                string tag3 = tag.Substring(1);
                
                if (tagType == 0) prefixes.RemoveWhere(x => x.Tag == tag3);
                else if (tagType == 2) predefined.RemoveWhere(x => x.Tag == tag3);
            }
            else if (tagType == 0) //General
            {
                prefixes.Add(tag2);
                if (string.IsNullOrWhiteSpace(text)) return null;
            }
            else if (tagType == 2) //Predefined
            {
                predefined.Add(tag2);
                if (string.IsNullOrWhiteSpace(text)) return null;
            }

            return textTag;
        }

        /// <summary>
        /// If tag arguments exist, return it with them
        /// </summary>
        private static TextTagData InterpretTagOnly(string tagContent)
        {
            var tag = new TextTagData();
            var scanner = new Scanner();
            var tokens = scanner.Scan(tagContent);

            bool isAssignment = false;

            foreach (var token in tokens)
            {
                switch (token.Kind)
                {
                    case ArgumentKind.Identifier:
                        {
                            if (isAssignment)
                            {
                                var arg = ReparseTagArgument(tagContent) ?? token;
                                tag.TagArgument = ParseTagArgument(arg);
                                break;
                            }

                            tag.Tag = token.Content;
                            break;
                        }

                    case ArgumentKind.Assignment:
                        {
                            isAssignment = true;
                            break;
                        }

                    case ArgumentKind.EndOfToken:
                        break;

                    default:
                        {
                            if (isAssignment)
                            {
                                tag.TagArgument = ParseTagArgument(token);

                                if (tag.TagArgument == null)
                                {
                                    var arg = ReparseTagArgument(tagContent);
                                    if (arg != null) tag.TagArgument = ParseTagArgument(arg);
                                }
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

        private static Token ReparseTagArgument(string tagContent)
        {
            string arg = tagContent.Substring(tagContent.IndexOf('=') + 1);
            arg = arg.Insert(0, "\"");
            arg += "\"";
            var scanner = new Scanner();
            var tokens = scanner.Scan(arg);

            if (tokens.Count > 0) return tokens[0];
            return null;
        }

        private static int ParseTextTagType(string tag1)
        {
            var tags = new List<string>() { tag1 };
            if (tag1.StartsWith("/")) tags.Add(tag1.Substring(1));

            foreach (string tag in tags)
            {
                for (int i = 0; i < _predefinedTags.Length; i++) if (tag == _predefinedTags[i]) return 2; //Predefined
                for (int i = 0; i < _tagType1.Length; i++) if (tag == _tagType1[i]) return 0; //General
                for (int i = 0; i < _tagType2.Length; i++) if (tag == _tagType2[i]) return 1; //Dialogue
            }
            return -1;
        }
    }
}