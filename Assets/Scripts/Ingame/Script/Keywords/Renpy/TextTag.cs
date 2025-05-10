using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectMGG.Ingame.Script.Keywords.Renpy
{
    public class TextTag
    {
        public string Text { get; set; } = string.Empty;
        public TextTagData PrimaryData { get; set; } = new TextTagData();
        public HashSet<TextTagData> PrefixDatas { get; set; } = new HashSet<TextTagData>();

        public TextTag()
        {

        }

        public TextTag(string text)
        {
            Text = text;
        }

        public override string ToString()
        {
            //string tags = string.Join(";", PrefixDatas.Select(x => x.Tag).ToList());
            //string args = string.Join(";", PrefixDatas.Select(x => x.TagArgument).ToList());
            //return $"TextTag(text={Text}, primary_tag='{PrimaryData.Tag}', primary_tag_arg='{PrimaryData.TagArgument}', tag='{tags}', arg='{args}')";
            StringBuilder sb = new StringBuilder();

            foreach (TextTagData data in PrefixDatas) {
                sb.Append("{");
                sb.Append(data.Tag);
                if (data.TagArgument != null)
                {
                    sb.Append("=");
                    sb.Append(data.TagArgument);
                }
                sb.Append("}");
            }
            sb.Append(Text);
            if (!string.IsNullOrWhiteSpace(PrimaryData.Tag))
            {
                sb.Append("{");
                sb.Append(PrimaryData.Tag);
                if (PrimaryData.TagArgument != null)
                {
                    sb.Append("=");
                    sb.Append(PrimaryData.TagArgument.ToString());
                }
                sb.Append("}");
            }

            return sb.ToString();
        }
    }
}