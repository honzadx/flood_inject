internal record VariableModel : BaseElementModel
{
    internal string Name { get; }
    internal string Type { get; }

    public VariableModel(string name, string type)
    {
        Name = name;
        Type = type;
    }

    public override void Build(CodeWriter codeWriter)
    {
        codeWriter.Write($"{Type} {Name}");
    }
}