using System.Collections.Immutable;

internal record MethodModel : BaseTypeElementModel
{
    public ImmutableArray<string> Keywords { get; }
    public string ReturnType { get; }
    public string Name { get; }
    public ImmutableArray<VariableModel> Parameters { get; }
    public bool Lambda { get; }
    public string[] Lines { get; }

    public MethodModel(
        string[] keywords, 
        string returnType, 
        string name, 
        VariableModel[] parameters,
        bool lambda,
        string[] lines)
    {
        Keywords = ImmutableArray.Create(keywords);
        ReturnType = returnType;
        Name = name;
        Parameters = ImmutableArray.Create(parameters);
        Lambda = lambda;
        Lines = lines;
    }

    public override void Build(CodeWriter codeWriter)
    {
        foreach (var keyword in Keywords)
        {
            codeWriter.Write($"{keyword} ");
        }
        if (!string.IsNullOrEmpty(ReturnType))
        {
            codeWriter.Write($"{ReturnType} ");
        }
        codeWriter.Write($"{Name}(");
        for (int i = 0; i < Parameters.Length; i++)
        {
            Parameters[i].Build(codeWriter);
            if (i < Parameters.Length - 1)
            {
                codeWriter.Write(", ");
            }
        }
        codeWriter.WriteLine(")");
        if (Lambda)
        {
            codeWriter.WriteLine(" => ");
            if (Lines.Length == 1)
            {
                codeWriter.WriteLine(Lines[0]);
            }
            else if (Lines.Length > 1)
            {
                using (codeWriter.CreateScope())
                {
                    foreach (var line in Lines)
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
                if (Lines.Length == 1)
                {
                    codeWriter.WriteLine(Lines[0]);
                }
                else if (Lines.Length > 1)
                {
                    foreach (var line in Lines)
                    {
                        codeWriter.WriteLine(line);
                    }
                }
            }
        }
    }
}