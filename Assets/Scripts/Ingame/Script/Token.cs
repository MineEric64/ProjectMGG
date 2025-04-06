using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectMGG.Ingame.Script
{
    public class Token
    {
        public ArgumentKind Kind { get; set; } = ArgumentKind.Unknown;
        public string Content { get; set; }
        public int Line { get; set; } = 0;

        public Token(ArgumentKind kind)
        {
            this.Kind = kind;
        }

        public Token(ArgumentKind kind, string content)
        {
            this.Kind = kind;
            this.Content = content;
        }

        public Token(ArgumentKind kind, string content, int line)
        {
            this.Kind = kind;
            this.Content = content;
            this.Line = line;
        }

        public override string ToString()
        {
            return $"Token(kind={Kind}, content='{Content}', line='{Line}')";
        }
    }
}