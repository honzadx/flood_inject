namespace FloodInject.SourceGen.models;

internal record NewLineModel : BaseTypeElementModel
{
    public override void Build(CodeWriter codeWriter)
    {
        codeWriter.WriteLine();
    }
}