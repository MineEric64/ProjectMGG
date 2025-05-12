namespace ProjectMGG.Ingame.Script.Keywords.Renpy
{
    public class Return : IStatement
    {
        public int Line { get; set; } = 0;

        public void Interpret()
        {
            //TODO: if return is not executed, it works like case block (sequential execution)
            if (Interpreter.FramePointers.Count > 0)
            {
                Function function = Interpreter.FramePointers.Pop();

                if (function != null) Interpreter.CurrentPoint = function;
                else Interpret();
            }
            else
            {
                //It means the entry point (start) has ended
            }
        }
    }
}