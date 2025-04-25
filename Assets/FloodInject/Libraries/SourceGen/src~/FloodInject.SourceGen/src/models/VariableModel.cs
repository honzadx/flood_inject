internal record VariableModel(
    string name, 
    string type) : AElementModel
{
    public string name { get; } = name;
    public string type { get; } = type;

    public override void Build(CodeWriter codeWriter)
    {
        codeWriter.Write($"{type} {name}");
    }
}