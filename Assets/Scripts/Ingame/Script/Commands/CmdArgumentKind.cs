using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ProjectMGG.Ingame.Script.Commands
{
    public enum CmdArgumentKind
    {
        [KindAttr("#unknown")] Unknown,
        [KindAttr("#EndOfToken")] EndOfToken,
        [KindAttr("#String")] StringLiteral,
        [KindAttr("#identifier")] Identifier,

        [KindAttr("day")] Day,

        [KindAttr(",")] Comma,
        [KindAttr(":")] Colon,
        [KindAttr("(")] LeftParen,
        [KindAttr(")")] RightParen,
        [KindAttr("{")] LeftBrace,
        [KindAttr("}")] RightBrace,
        [KindAttr("[")] LeftBracket,
        [KindAttr("]")] RightBracket
    }
    public static class CmdArgumentKinds
    {
        private static Dictionary<string, CmdArgumentKind> map = new Dictionary<string, CmdArgumentKind>();

        public static bool IsInitialized() => map.Count > 0;
        public static void Initialize()
        {
            var values = Enum.GetValues(typeof(CmdArgumentKind)).Cast<CmdArgumentKind>();

            foreach (var k in values)
            {
                k.Register();
            }
        }

        public static void Register(this CmdArgumentKind k)
        {
            var attr = GetAttr(k);
            map.Add(attr.Content, k);
        }

        public static CmdArgumentKind ToKind(string name)
        {
            if (map.ContainsKey(name))
            {
                return map[name];
            }
            return CmdArgumentKind.Unknown;
        }

        public static string GetContent(this CmdArgumentKind k)
        {
            var attr = GetAttr(k);
            return attr.Content;
        }

        private static KindAttr GetAttr(CmdArgumentKind k)
        {
            return (KindAttr)Attribute.GetCustomAttribute(ForValue(k), typeof(KindAttr));
        }

        private static MemberInfo ForValue(CmdArgumentKind k)
        {
            return typeof(CmdArgumentKind).GetField(Enum.GetName(typeof(CmdArgumentKind), k));
        }
    }
}