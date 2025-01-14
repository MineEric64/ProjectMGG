using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectMGG.Ingame.Script.Commands
{
    public class CmdToken
    {
        public CmdArgumentKind Kind { get; set; } = CmdArgumentKind.Unknown;
        public string Content { get; set; }

        public CmdToken(CmdArgumentKind kind)
        {
            this.Kind = kind;
        }

        public CmdToken(CmdArgumentKind kind, string content)
        {
            this.Kind = kind;
            this.Content = content;
        }

        public override string ToString()
        {
            return $"Token(kind={Kind}, content='{Content}')";
        }
    }
}