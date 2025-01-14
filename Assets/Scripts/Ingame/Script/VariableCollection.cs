using System.Collections;
using System.Collections.Generic;

using ProjectMGG.Ingame.Script.Keywords.Renpy;

namespace ProjectMGG.Ingame.Script
{
    public class VariableCollection
    {
        public Dictionary<string, Character> Characters = new Dictionary<string, Character>();
        public Dictionary<string, Attributes> Images = new Dictionary<string, Attributes>();
        public Dictionary<string, RpyTransform> Transforms = new Dictionary<string, RpyTransform>();
        public Dictionary<string, object> Others = new Dictionary<string, object>();

        public VariableCollection()
        {

        }
    }
}