using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

[Generator]
public class FloodSourceGenerator : IIncrementalGenerator
{
    private record ResolveMetadata(string FieldType, string FieldName, string Context)
    {
        internal string FieldType { get; } = FieldType;
        internal string FieldName { get; } = FieldName;
        internal string Context { get; } = Context;
    }
    
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var provider = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                "FloodInject.Runtime.FloodAttribute",
                predicate: static (s, _) => s is ClassDeclarationSyntax,
                transform: static (ctx, _) => Transform(ctx))
            .Where(t => t != null);
        
        context.RegisterSourceOutput(provider, Generate);
    }

    private static TypeModel Transform(GeneratorAttributeSyntaxContext context)
    {
        var syntax = (ClassDeclarationSyntax)context.TargetNode;
        var modifiersValid = syntax.HasModifiers(["public", "partial"]);
        var allFields = syntax.GetChildrenOfType<FieldDeclarationSyntax>().ToArray();

        if (!modifiersValid)
        {
            return null;
        }

        List<MethodModel> methodModelList = new();
        List<ResolveMetadata> resolveMetadataList = new();
        var isOverride = false;
        var attribute = syntax.GetAttribute("FloodInject.Runtime.FloodAttribute", context);
        var attributeArgumentList = attribute.GetFirstChildOfType<AttributeArgumentListSyntax>();
        if (attributeArgumentList != null)
        {
            foreach (var argument in attributeArgumentList.Arguments)
            {
                bool.TryParse(argument.Expression.GetFirstToken().Text, out isOverride);
            }
        }
        
        foreach (var field in allFields)
        {
            if (!field.HasAttribute("FloodInject.Runtime.ResolveAttribute", context))
            {
                continue;
            }

            var argumentListSyntax = field
                .GetFirstChildOfType<AttributeListSyntax>()
                .GetFirstChildOfType<AttributeSyntax>()
                .GetFirstChildOfType<AttributeArgumentListSyntax>();

            if (argumentListSyntax == null)
            {
                resolveMetadataList.Add(new ResolveMetadata(
                    FieldType: field.Declaration.Type.ToString(),
                    FieldName: field.Declaration.Variables[0].Identifier.Text,
                    Context: "GlobalContext"
                ));
            }
            else
            {
                var type = argumentListSyntax.Arguments[0].Expression.GetFirstChildOfType<TypeSyntax>();
                ResolveMetadata resolveMetadata = new ResolveMetadata(
                    FieldType: field.Declaration.Type.ToString(),
                    FieldName: field.Declaration.Variables[0].Identifier.Text, 
                    Context: type.ToString()
                );
                resolveMetadataList.Add(resolveMetadata);
            }
        }

        if (resolveMetadataList.Count == 0)
        {
            return null;
        }
        
        string[] resolveMethodLines = new string[isOverride ? resolveMetadataList.Count + 3 : resolveMetadataList.Count + 2];
        resolveMethodLines[0] = "PreConstruct();";
        resolveMethodLines[resolveMethodLines.Length - 2] = "base.Construct();";
        resolveMethodLines[resolveMethodLines.Length - 1] = "PostConstruct();";

        int index = 1;
        foreach (var injectMetadata in resolveMetadataList)
        {
            resolveMethodLines[index++] =
                $"{injectMetadata.FieldName} = ContextProvider<{injectMetadata.Context}>.Get().Resolve<{injectMetadata.FieldType}>();";
        }

        var constructMethod = new MethodModel(
            keywords: isOverride ? ["public", "override"] : ["public", "virtual"],
            returnType: "void",
            name: "Construct",
            parameters: [],
            lambda: false,
            lines: resolveMethodLines
        );
        methodModelList.Add(constructMethod);
        methodModelList.Add(new MethodModel(
            keywords: ["partial"],
            returnType: "void",
            name: "PreConstruct",
            parameters: [],
            lambda: false,
            lines: []
        ));
        methodModelList.Add(new MethodModel(
            keywords: ["partial"],
            returnType: "void",
            name: "PostConstruct",
            parameters: [],
            lambda: false,
            lines: []
        ));

        var usings = syntax.GetUsingDirectives().Select(s => s.Name.ToString()).ToArray();
        var elements = methodModelList.Select(m => m as BaseTypeElementModel).ToArray();
        
        TypeModel typeModel = new TypeModel(
            usings: usings,
            @namespace: syntax.GetNamespaceName(),
            keywords: ["partial"],
            kind: "class",
            name: syntax.Identifier.ValueText,
            elements: elements);
        
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