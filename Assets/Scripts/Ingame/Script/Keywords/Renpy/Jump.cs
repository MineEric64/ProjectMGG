using System.Drawing;

namespace ProjectMGG.Ingame.Script.Keywords.Renpy
{
    public class Jump : IStatement
    {
        public int Line { get; set; } = 0;
        public IExpression Name { get; set; } = null;

        public void Interpret()
        {
            string name = Name.Interpret() as string;

            if (string.IsNullOrEmpty(name))
            {
                ExceptionManager.Throw("The label name to jump can't be null.", "Script/Interpreter", Line);
                return;
            }
            bool success = Interpreter.FunctionTable.TryGetValue(name, out var point);

            if (!success)
            {
                ExceptionManager.Throw($"Can't jump because the label '{name}' doesn't exists.", "Script/Interpreter", Line);
                return;
            }

            IngameManagerV2.Locals.Add(point.Name, new VariableCollection());
            Interpreter.FramePointers.Push(null); //need to inform that it's from jump statement
            Interpreter.CurrentPoint = point;
        }
    }
}