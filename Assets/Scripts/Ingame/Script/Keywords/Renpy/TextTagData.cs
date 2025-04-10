namespace ProjectMGG.Ingame.Script.Keywords.Renpy
{
    public class TextTagData
    {
        public string Tag { get; set; } = string.Empty;
        public object TagArgument { get; set; } = null;

        public TextTagData()
        {

        }

        public TextTagData(string tag)
        {
            Tag = tag;
        }

        public TextTagData(string tag, object tagArgument) : this(tag)
        {
            TagArgument = tagArgument;
        }
    }
}