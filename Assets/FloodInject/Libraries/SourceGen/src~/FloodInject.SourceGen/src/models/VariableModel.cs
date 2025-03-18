internal record VariableModel : BaseElementModel
{
    public string Name { get; }
    public string Type { get; }

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