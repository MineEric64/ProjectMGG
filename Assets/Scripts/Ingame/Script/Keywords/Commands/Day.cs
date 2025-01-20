using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace ProjectMGG.Ingame.Script.Keywords.Commands
{
    public class Day : IStatement
    {
        public IExpression FileName { get; set; } = null;

        public void Interpret()
        {
            string name = (string)FileName.Interpret();
            IntroPlayer.Instance.GoDay(name);
        }
    }
}