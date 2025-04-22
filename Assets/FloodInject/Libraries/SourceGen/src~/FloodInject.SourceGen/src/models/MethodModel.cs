using System.Collections.Immutable;

internal record MethodModel(
    ImmutableArray<string> keywords, 
    string type, 
    string name, 
    ImmutableArray<VariableModel> parameters,
    bool lambda,
    ImmutableArray<string> lines) : BaseElementModel
{
    public ImmutableArray<string> keywords { get; } = keywords;
    public string type { get; } = type;
    public string name { get; } = name;
    public ImmutableArray<VariableModel> parameters { get; } = parameters;
    public bool lambda { get; } = lambda;
    public ImmutableArray<string> lines { get; } = lines;

    public override void Build(CodeWriter codeWriter)
    {
        foreach (var keyword in keywords)
        {
            codeWriter.Write($"{keyword} ");
        }
        if (!string.IsNullOrEmpty(type))
        {
            codeWriter.Write($"{type} ");
        }
        codeWriter.Write($"{name}(");
        for (int i = 0; i < parameters.Length; i++)
        {
            parameters[i].Build(codeWriter);
            if (i < parameters.Length - 1)
            {
                codeWriter.Write(", ");
            }
        }

        if (lines.Length == 0)
        {
            codeWriter.WriteLine(");");
            return;
        }
        codeWriter.WriteLine(")");
        if (lambda)
        {
            codeWriter.WriteLine(" => ");
            if (lines.Length == 1)
            {
                codeWriter.WriteLine(lines[0]);
            }
            else if (lines.Length > 1)
            {
                using (codeWriter.CreateScope())
                {
                    foreach (var line in lines)
                    {
                        codeWriter.WriteLine(line);
                    }
                }
            }
        }
        else
        {
            using (codeWriter.CreateScope())
            {
                if (lines.Length == 1)
                {
                    codeWriter.WriteLine(lines[0]);
                }
                else if (lines.Length > 1)
                {
                    foreach (var line in lines)
                    {
                        codeWriter.WriteLine(line);
                    }
                }
            }
        }
    }
}