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
    internal enum ContextKind
    {
        Global,
        Scriptable,
        Scene
    }
    
    private record ResolveMetadata(string FieldType, string FieldName, string Context, ContextKind ContextKind)
    {
        internal string FieldType { get; } = FieldType;
        internal string FieldName { get; } = FieldName;
        internal string Context { get; } = Context;
        internal ContextKind ContextKind { get; } = ContextKind;
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
        List<ResolveMetadata> resolveCtxMetadataList = new();
        var isOverride = false;
        var attribute = syntax.GetAttribute("FloodInject.Runtime.FloodAttribute", context);
        var attributeArgumentList = attribute.GetFirstChildOfType<AttributeArgumentListSyntax>();
        if (attributeArgumentList != null)
        {
            foreach (var argument in attributeArgumentList.Arguments)
            {
                if (bool.TryParse(argument.Expression.GetFirstToken().Text, out bool outIsOverride))
                {
                    isOverride = outIsOverride;
                }
            }
        }
        
        foreach (var field in allFields)
        {
            if (!field.HasAttribute("FloodInject.Runtime.ResolveAttribute", context))
            {
                continue;
            }

            var attributeArgs = field
                .GetFirstChildOfType<AttributeListSyntax>()
                .GetFirstChildOfType<AttributeSyntax>()
                .GetFirstChildOfType<AttributeArgumentListSyntax>();

            if (attributeArgs == null || attributeArgs.Arguments.Count == 0)
            {
                ResolveMetadata resolveMetadata = new ResolveMetadata(
                    FieldType: field.Declaration.Type.ToString(),
                    FieldName: field.Declaration.Variables[0].Identifier.Text, 
                    Context: null,
                    ContextKind: ContextKind.Global);
                resolveCtxMetadataList.Add(resolveMetadata);
            }
            else
            {
                var expression = attributeArgs
                    .Arguments[0]
                    .Expression;
                
                var scriptableContextType = expression.GetFirstChildOfType<TypeSyntax>();
                if (scriptableContextType != null)
                {
                    ResolveMetadata resolveMetadata = new ResolveMetadata(
                        FieldType: field.Declaration.Type.ToString(),
                        FieldName: field.Declaration.Variables[0].Identifier.Text, 
                        Context: scriptableContextType.ToString(),
                        ContextKind: ContextKind.Scriptable);
                    resolveCtxMetadataList.Add(resolveMetadata);
                }
                else
                {
                    ResolveMetadata resolveMetadata = new ResolveMetadata(
                        FieldType: field.Declaration.Type.ToString(),
                        FieldName: field.Declaration.Variables[0].Identifier.Text, 
                        Context: expression.ToString(),
                        ContextKind: ContextKind.Scene);
                    resolveCtxMetadataList.Add(resolveMetadata);
                }
            }
        }

        if (resolveCtxMetadataList.Count == 0)
        {
            return null;
        }
        
        string[] constructMethodLines = new string[isOverride ? resolveCtxMetadataList.Count + 3 : resolveCtxMetadataList.Count + 1];
        constructMethodLines[0] = "PreConstruct();";
        constructMethodLines[constructMethodLines.Length - 2] = "base.Construct();";
        constructMethodLines[constructMethodLines.Length - 1] = "PostConstruct();";

        int index = 0;
        foreach (var metadata in resolveCtxMetadataList)
        {
            constructMethodLines[index++] = metadata.ContextKind switch
            {
                ContextKind.Global => 
                    $"{metadata.FieldName} = ContextProvider.GetGlobalContext().Get<{metadata.FieldType}>();",
                ContextKind.Scriptable =>
                    $"{metadata.FieldName} = ContextProvider.GetScriptableContext<{metadata.Context}>().Get<{metadata.FieldType}>();",
                ContextKind.Scene =>
                    $"{metadata.FieldName} = ContextProvider.GetSceneContext({metadata.Context}).Get<{metadata.FieldType}>();",
            };
        }

        var constructMethod = new MethodModel(
            keywords: isOverride ? ["public", "override"] : ["public", "virtual"],
            returnType: "void",
            name: "Construct",
            parameters: [],
            lambda: false,
            lines: constructMethodLines
        );
        methodModelList.Add(constructMethod);
        methodModelList.Add(new MethodModel(
            keywords: [ "partial" ],
            returnType: "void",
            name: "PreConstruct",
            parameters: [],
            lambda: false,
            lines: []
        ));
        methodModelList.Add(new MethodModel(
            keywords: [ "partial" ],
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