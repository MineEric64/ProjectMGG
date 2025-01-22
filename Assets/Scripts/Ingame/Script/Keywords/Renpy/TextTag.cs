public class TextTag
{
    public string Text { get; set; } = string.Empty;
    public string Tag { get; set; } = string.Empty;
    public object TagArgument { get; set; } = null;

    public TextTag()
    {

    }

    public TextTag(string text)
    {
        Text = text;
    }

    public TextTag(string text, string tag)
    {
        Text = text;
        Tag = tag;
    }

    public TextTag(string text, string tag, object tagArgument) : this(text, tag)
    {
        TagArgument = tagArgument;
    }

    public override string ToString()
    {
        string arg = TagArgument == null ? string.Empty : TagArgument.ToString();
        return $"'{Text}'\n'{Tag}'\n'{arg}'";
    }
}
