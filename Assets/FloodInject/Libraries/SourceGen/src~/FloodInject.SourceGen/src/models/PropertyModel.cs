internal record PropertyModel : BaseTypeElementModel
{
    internal string[] Keywords { get; }
    internal string Type { get; }
    internal string Name { get; }
    internal string ReturnValue { get; }

    public PropertyModel(
        string[] keywords,
        string type,
        string name,
        string returnValue)
    {
        Keywords = keywords;
        Type = type;
        Name = name;
        ReturnValue = returnValue;
    }
    
    public override void Build(CodeWriter codeWriter)
    {
        foreach (var keyword in Keywords)
        {
            codeWriter.Write($"{keyword} ");
        }
        codeWriter.WriteLine($"{Type} {Name} => {ReturnValue};");
    }
}