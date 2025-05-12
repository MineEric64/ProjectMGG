using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectMGG.Ingame.Script.Keywords
{
    public class Call : IExpression
    {
        public IExpression Sub { get; set; }
        public List<IExpression> Arguments { get; set; } = new List<IExpression>();

        public object Interpret()
        {
            string name = Sub.Interpret() as string;

            if (string.IsNullOrEmpty(name))
            {
                ExceptionManager.Throw("The label name to call can't be null.", "Script/Interpreter");
                return null;
            }
            bool success = Interpreter.FunctionTable.TryGetValue(name, out var point);

            if (!success)
            {
                ExceptionManager.Throw($"Can't call because the label '{name}' doesn't exists.", "Script/Interpreter");
                return null;
            }

            IngameManagerV2.Locals.Add(point.Name, new VariableCollection());
            Interpreter.FramePointers.Push(Interpreter.CurrentPoint);
            Interpreter.CurrentPoint = point;

            foreach (IExpression arg in Arguments)
            {
                var variable = new Variable();

                variable.Expression = arg;
                //variable.Name = "?";
                //variable.Interpret();
            }
            
            return null;
        }
    }
}