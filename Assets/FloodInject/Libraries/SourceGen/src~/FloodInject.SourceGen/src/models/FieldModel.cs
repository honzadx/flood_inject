using System.Collections.Immutable;

internal record FieldModel : BaseTypeElementModel
{
    public ImmutableArray<string> Keywords { get; }
    public string Type { get; }
    public string Name { get; }
    public string Value { get; }

    public FieldModel(string[] keywords, string type, string name, string value)
    {
        Keywords = ImmutableArray.Create(keywords);
        Type = type;
        Name = name;
        Value = value;
    }
    
    public override void Build(CodeWriter codeWriter)
    {
        foreach (var keyword in Keywords)
        {
            codeWriter.Write($"{keyword} ");
        }

        if (string.IsNullOrEmpty(Value))
        {
            codeWriter.WriteLine($"{Type} {Name};");
        }
        else
        {
            codeWriter.WriteLine($"{Type} {Name} = {Value};");
        }
    }
}