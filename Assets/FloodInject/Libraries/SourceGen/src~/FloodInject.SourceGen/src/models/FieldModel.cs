internal record FieldModel : BaseTypeElementModel
{
    internal string[] Keywords { get; }
    internal string Type { get; }
    internal string Name { get; }

    public FieldModel(string[] keywords, string type, string name)
    {
        Keywords = keywords;
        Type = type;
        Name = name;
    }
    
    public override void Build(CodeWriter codeWriter)
    {
        foreach (var keyword in Keywords)
        {
            codeWriter.Write($"{keyword} ");
        }
        codeWriter.WriteLine($"{Type} {Name};");
    }
}