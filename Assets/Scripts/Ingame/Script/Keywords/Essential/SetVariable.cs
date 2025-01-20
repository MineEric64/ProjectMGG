using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectMGG.Ingame.Script.Keywords
{
    public class SetVariable : IExpression
    {
        public string Name { get; set; }
        public IExpression Value { get; set; }

        public object Interpret()
        {
            if (IngameManagerV2.Local.Others.ContainsKey(Name)) IngameManagerV2.Local.Others[Name] = Value.Interpret();
            else if (IngameManagerV2.Global.Others.ContainsKey(Name)) IngameManagerV2.Global.Others[Name] = Value.Interpret();

            return IngameManagerV2.GetVariable(Name, ref IngameManagerV2.Local.Others, ref IngameManagerV2.Global.Others);
        }
    }
}