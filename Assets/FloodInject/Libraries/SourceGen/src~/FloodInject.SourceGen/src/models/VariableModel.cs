internal record VariableModel : BaseElementModel
{
    public string Name { get; }
    public string Type { get; }

    public VariableModel(string type, string name)
    {
        Type = type;
        Name = name;
    }

    public override void Build(CodeWriter codeWriter)
    {
        codeWriter.Write($"{Type} {Name}");
    }
}