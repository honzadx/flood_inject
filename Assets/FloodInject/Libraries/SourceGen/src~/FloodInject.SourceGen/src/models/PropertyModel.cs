using System.Collections.Immutable;

internal record PropertyModel : BaseTypeElementModel
{
    public ImmutableArray<string> Keywords { get; }
    public string Type { get; }
    public string Name { get; }
    public string ReturnValue { get; }

    public PropertyModel(
        string[] keywords,
        string type,
        string name,
        string returnValue)
    {
        Keywords = ImmutableArray.Create(keywords);
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