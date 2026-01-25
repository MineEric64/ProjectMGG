using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProjectMGG.Ingame.Script.Keywords.Renpy
{
    public class FX : IStatement
    {
        public int Line { get; set; } = 0;
        public IExpression Name { get; set; } = null;
        public string At { get; set; } = string.Empty; //Transform Name

        public void Interpret()
        {
            string realName = string.Empty;

            if (Name != null)
            {
                if (Name is GetVariable t) realName = t.Name;
                else if (Name is StringLiteral s) realName = s.Value;
            }

            IngameManagerV2.Instance.StartCoroutine(IngameManagerV2.Instance.LetsFX(realName, At));
        }
    }
}