using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ProjectMGG.Ingame.Script
{
    class KindAttr : Attribute
    {
        internal KindAttr(string content)
        {
            this.Content = content;
        }
        public string Content { get; private set; }
    }
    public enum ArgumentKind
    {
        [KindAttr("#unknown")] Unknown,
        [KindAttr("#EndOfToken")] EndOfToken,

        [KindAttr("null")] NullLiteral,
        [KindAttr("true")] TrueLiteral,
        [KindAttr("false")] FalseLiteral,
        [KindAttr("#Number")] NumberLiteral,
        [KindAttr("#String")] StringLiteral,
        [KindAttr("#identifier")] Identifier,

        [KindAttr("label")] Function,
        [KindAttr("return")] Return,
        [KindAttr("define")] Variable,
        [KindAttr("image")] Image,
        [KindAttr("while")] While,
        [KindAttr("pass")] Pass,
        [KindAttr("if")] If,
        [KindAttr("elif")] Elif,
        [KindAttr("else")] Else,
        [KindAttr("#")] Comment,
        [KindAttr("jump")] Jump,

        [KindAttr("Character")] Character,

        [KindAttr("transform")] Transform,
        [KindAttr("xalign")] XAlign,
        [KindAttr("yalign")] YAlign,
        [KindAttr("zoom")] Zoom,

        [KindAttr("show")] Show,
        [KindAttr("at")] At,
        [KindAttr("with")] With,

        [KindAttr("play")] Play,
        [KindAttr("reeverb")] Reeverb,

        [KindAttr("and")] LogicalAnd,
        [KindAttr("or")] LogicalOr,
        [KindAttr("=")] Assignment,
        [KindAttr("+")] Add,
        [KindAttr("-")] Subtract,
        [KindAttr("*")] Multiply,
        [KindAttr("/")] Divide,
        [KindAttr("%")] Modulo,
        [KindAttr("==")] Equal,
        [KindAttr("!=")] NotEqual,
        [KindAttr("<")] LessThan,
        [KindAttr(">")] GreaterThan,
        [KindAttr("<=")] LessOrEqual,
        [KindAttr(">=")] GreaterOrEqual,
        [KindAttr("+=")] Increment,
        [KindAttr("-=")] Decrement,

        [KindAttr(",")] Comma,
        [KindAttr(":")] Colon,
        [KindAttr("(")] LeftParen,
        [KindAttr(")")] RightParen,
        [KindAttr("{")] LeftBrace,
        [KindAttr("}")] RightBrace,
        [KindAttr("[")] LeftBracket,
        [KindAttr("]")] RightBracket
    }
    public static class ArgumentKinds
    {
        private static Dictionary<string, ArgumentKind> map = new Dictionary<string, ArgumentKind>();

        public static bool IsInitialized() => map.Count > 0;
        public static void Initialize()
        {
            var values = Enum.GetValues(typeof(ArgumentKind)).Cast<ArgumentKind>();

            foreach (var k in values)
            {
                k.Register();
            }
        }

        public static void Register(this ArgumentKind k)
        {
            var attr = GetAttr(k);
            map.Add(attr.Content, k);
        }

        public static ArgumentKind ToKind(string name)
        {
            if (map.ContainsKey(name))
            {
                return map[name];
            }
            return ArgumentKind.Unknown;
        }

        public static string GetContent(this ArgumentKind k)
        {
            var attr = GetAttr(k);
            return attr.Content;
        }

        private static KindAttr GetAttr(ArgumentKind k)
        {
            return (KindAttr)Attribute.GetCustomAttribute(ForValue(k), typeof(KindAttr));
        }

        private static MemberInfo ForValue(ArgumentKind k)
        {
            return typeof(ArgumentKind).GetField(Enum.GetName(typeof(ArgumentKind), k));
        }
    }
}