using System.Collections.Immutable;

internal record FieldModel(
    ImmutableArray<string> keywords, 
    string type, 
    string name, 
    string value) : AElementModel
{
    public ImmutableArray<string> keywords { get; } = keywords;
    public string type { get; } = type;
    public string name { get; } = name;
    public string value { get; } = value;
    
    public override void Build(CodeWriter codeWriter)
    {
        foreach (var keyword in keywords)
        {
            codeWriter.Write($"{keyword} ");
        }

        if (string.IsNullOrEmpty(value))
        {
            codeWriter.WriteLine($"{type} {name};");
        }
        else
        {
            codeWriter.WriteLine($"{type} {name} = {value};");
        }
    }
}