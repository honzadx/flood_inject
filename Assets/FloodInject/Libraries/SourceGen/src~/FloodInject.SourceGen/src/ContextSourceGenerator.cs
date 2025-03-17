using System;
using System.IO;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

[Generator]
public class ContextSourceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var provider = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                "FloodInject.Runtime.GenerateContextAttribute",
                predicate: static (s, _) => s is ClassDeclarationSyntax,
                transform: static (ctx, _) => Transform(ctx))
            .Where(t => t != null);
        
        context.RegisterSourceOutput(provider, Generate);
    }

    private static TypeModel Transform(GeneratorAttributeSyntaxContext context)
    {
        var syntax = (ClassDeclarationSyntax)context.TargetNode;
        var isContext = syntax.HasBaseType("FloodInject.Runtime.BaseContext") || syntax.HasBaseType("BaseContext");
        var validModifiers = syntax.HasModifiers(["public", "partial"]) && !syntax.HasModifier("abstract");
        
        if(!isContext && validModifiers)
        {
            return null;
        }

        TypeModel typeModel = new TypeModel(
            usings: ["global::FloodInject.Runtime"],
            @namespace: syntax.GetNamespaceName(),
            keywords: ["partial"],
            kind: "class",
            name: syntax.Identifier.Text,
            elements: 
            [
                new PropertyModel(
                    keywords: ["public", "static"],
                    name: "Type",
                    type: "global::System.Type",
                    returnValue: $"typeof({syntax.Identifier.Text})"),
                new PropertyModel(
                    keywords: ["public", "override"],
                    name: "ContextType",
                    type: "global::System.Type",
                    returnValue: $"{syntax.Identifier.Text}.Type"),
            ]
        );
        return typeModel;
    }

    private static void Generate(SourceProductionContext context, TypeModel typeModel)
    {
        using MemoryStream sourceStream = new();
        using StreamWriter sourceStreamWriter = new StreamWriter(sourceStream);
        using CodeWriter codeWriter = new CodeWriter(sourceStreamWriter);
        typeModel.Build(codeWriter);
        codeWriter.Flush();
        context.AddSource($"{typeModel.Name}.g.cs", SourceText.From(sourceStream, Encoding.UTF8, canBeEmbedded: true));
    }
}