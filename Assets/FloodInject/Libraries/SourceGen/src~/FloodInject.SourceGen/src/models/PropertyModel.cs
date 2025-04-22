using System.Collections.Immutable;

internal record PropertyModel(
    ImmutableArray<string> keywords,
    string type,
    string name,
    string returnExpression) : BaseElementModel
{
    public ImmutableArray<string> keywords { get; } = keywords;
    public string type { get; } = type;
    public string name { get; } = name;
    public string returnExpression { get; } = returnExpression;
    
    public override void Build(CodeWriter codeWriter)
    {
        foreach (var keyword in keywords)
        {
            codeWriter.Write($"{keyword} ");
        }
        codeWriter.WriteLine($"{type} {name} => {returnExpression};");
    }
}