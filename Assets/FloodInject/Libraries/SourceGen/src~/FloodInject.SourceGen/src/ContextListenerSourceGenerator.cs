using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FloodInject.SourceGen.models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

[Generator]
public class InjectSourceGenerator : IIncrementalGenerator
{
    private record InjectMetadata(string FieldType, string FieldName, string Context)
    {
        internal string FieldType { get; } = FieldType;
        internal string FieldName { get; } = FieldName;
        internal string Context { get; } = Context;
    }
    
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var provider = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                "FloodInject.Runtime.ContextListenerAttribute",
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

        List<BaseTypeElementModel> elementModelList = new();
        List<InjectMetadata> injectMetadataList = new();
        var constructIsOverride = false;
        var attribute = syntax.GetAttribute("FloodInject.Runtime.ContextListenerAttribute", context);
        var attributeArgumentList = attribute.GetFirstChildOfType<AttributeArgumentListSyntax>();
        if (attributeArgumentList != null)
        {
            foreach (var argument in attributeArgumentList.Arguments)
            {
                if (bool.TryParse(argument.Expression.GetFirstToken().Text, out bool isOverride))
                {
                    constructIsOverride = isOverride;
                }
                switch (argument.Expression.ToString())
                {
                    case "AutoInject.Constructor":
                        elementModelList.Add(new MethodModel(
                            keywords: ["public"],
                            returnType: null,
                            name: syntax.Identifier.ValueText,
                            parameters: [],
                            lambda: false,
                            lines: ["Construct();"]));
                        elementModelList.Add(new NewLineModel());
                        break;
                }
            }
        }
        
        foreach (var field in allFields)
        {
            if (!field.HasAttribute("FloodInject.Runtime.InjectAttribute", context))
            {
                continue;
            }

            var type = field
                .GetFirstChildOfType<AttributeListSyntax>()
                .GetFirstChildOfType<AttributeSyntax>()
                .GetFirstChildOfType<AttributeArgumentListSyntax>()
                .Arguments[0]
                .Expression
                .GetFirstChildOfType<TypeSyntax>();
            
            InjectMetadata injectMetadata = new InjectMetadata(
                FieldType: field.Declaration.Type.ToString(),
                FieldName: field.Declaration.Variables[0].Identifier.Text, 
                Context: type.ToString());
            injectMetadataList.Add(injectMetadata);
        }

        if (injectMetadataList.Count == 0)
        {
            return null;
        }

        List<string> methodLines = new List<string>();
        methodLines.AddRange(["PreConstruct();", ""]);
        if (constructIsOverride)
        {
            methodLines.Add("base.Construct();");
        }
        foreach (var injectMetadata in injectMetadataList)
        {
            methodLines.Add($"{injectMetadata.FieldName} = ContextProvider<{injectMetadata.Context}>.GetContext().Get<{injectMetadata.FieldType}>();");
        }
        methodLines.AddRange(["", "PostConstruct();"]);

        elementModelList.AddRange(
        [
            new MethodModel(
                keywords: constructIsOverride ? ["public", "override"] : ["public", "virtual"],
                returnType: "void",
                name: "Construct",
                parameters: [],
                lambda: false,
                lines: methodLines.ToArray()),
            new NewLineModel(),
            new MethodModel(
                keywords: [ "partial" ], 
                returnType: "void",
                name: "PreConstruct",
                parameters: [],
                lambda: false,
                lines: []),
            new MethodModel(
                keywords: [ "partial" ], 
                returnType: "void",
                name: "PostConstruct",
                parameters: [],
                lambda: false,
                lines: [])
        ]);

        var usings = syntax.GetUsingDirectives().Select(s => s.Name.ToString()).Append("global::FloodInject.Runtime").ToArray();
        var elements = elementModelList.Select(m => m as BaseTypeElementModel).ToArray();
        
        TypeModel typeModel = new TypeModel(
            pragmaDisables: [ "CS0109" ],
            usings: usings,
            @namespace: syntax.GetNamespaceName(),
            keywords: ["partial"],
            kind: "class",
            name: syntax.Identifier.ValueText,
            implements: null,
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