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

        public HashSet<TextTagData> PrefixPredefined { get; set; } = new HashSet<TextTagData>();
        public HashSet<TextTagData> PrefixPredefinedCustom { get; set; } = new HashSet<TextTagData>();

        public TextTag()
        {

        }

        public TextTag(string text)
        {
            Text = text;
        }

        public override string ToString()
        {
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

        public string GetTextWithPredefined()
        {
            var sb = new StringBuilder();
            
            foreach (TextTagData data in PrefixPredefined)
            {
                if (!string.IsNullOrEmpty(data.Tag))
                {
                    sb.Append("<");
                    sb.Append(data.Tag);

                    if (data.TagArgument != null)
                    {
                        sb.Append("=");
                        sb.Append(data.TagArgument);
                    }
                    sb.Append(">");
                }
            }

            sb.Append(Text);

            foreach (TextTagData data in PrefixPredefined)
            {
                if (!string.IsNullOrEmpty(data.Tag))
                {
                    sb.Append("</");
                    sb.Append(data.Tag);
                    sb.Append(">");
                }
            }
            
            return sb.ToString();
        }
    }
}