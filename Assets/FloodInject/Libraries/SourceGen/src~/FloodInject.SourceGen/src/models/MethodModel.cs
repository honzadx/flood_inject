internal record MethodModel : BaseTypeElementModel
{
    internal string[] Keywords { get; }
    internal string ReturnType { get; }
    internal string Name { get; }
    internal VariableModel[] Parameters { get; }
    internal bool Lambda { get; }
    internal string[] Lines { get; }

    public MethodModel(
        string[] keywords, 
        string returnType, 
        string name, 
        VariableModel[] parameters,
        bool lambda,
        string[] lines)
    {
        Name = name;
        ReturnType = returnType;
        Keywords = keywords;
        Parameters = parameters;
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