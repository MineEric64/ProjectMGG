using System.Collections.Generic;
using System.Linq;

namespace ProjectMGG.Ingame.Script.Keywords.Renpy
{
    public class TextTag
    {
        public string Text { get; set; } = string.Empty;
        public HashSet<TextTagData> Datas { get; set; } = new HashSet<TextTagData>();

        public TextTag()
        {

        }

        public TextTag(string text)
        {
            Text = text;
        }

        public override string ToString()
        {
            string tags = string.Join(";", Datas.Select(x => x.Tag).ToList());
            string args = string.Join(";", Datas.Select(x => x.TagArgument).ToList());
            return $"TextTag(text={Text}, tag='{tags}', arg='{args}')";
        }
    }
}