using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

[Generator]
public class InjectSourceGenerator : IIncrementalGenerator
{
    private record InjectMetadata
    {
        internal string FieldType { get; }
        internal string FieldName { get; }
        internal string Context { get; }

        public InjectMetadata(string fieldType, string fieldName, string context)
        {
            FieldType = fieldType;
            FieldName = fieldName;
            Context = context;
        }
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

        List<MethodModel> methodModelList = new();
        List<InjectMetadata> injectMetadataList = new();
        var injectIsOverride = false;
        var attribute = syntax.GetAttribute("FloodInject.Runtime.ContextListenerAttribute", context);
        var attributeArgumentList = attribute.GetFirstChildOfType<AttributeArgumentListSyntax>();
        if (attributeArgumentList != null)
        {
            foreach (var argument in attributeArgumentList.Arguments)
            {
                if (bool.TryParse(argument.Expression.GetFirstToken().Text, out bool isOverride))
                {
                    injectIsOverride = isOverride;
                }
                switch (argument.Expression.ToString())
                {
                    case "AutoInjectType.Constructor":
                        methodModelList.Add(new MethodModel(
                            keywords: ["public"],
                            returnType: null,
                            name: syntax.Identifier.ValueText,
                            parameters: [],
                            lambda: false,
                            lines: ["Inject();"]));
                        break;
                    case "AutoInjectType.Unity":
                        methodModelList.Add(new MethodModel(
                            keywords: ["protected", "new"],
                            returnType: "void",
                            name: "Start",
                            parameters: [],
                            lambda: false,
                            lines: ["Inject();"]));
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
                fieldType: field.Declaration.Type.ToString(),
                fieldName: field.Declaration.Variables[0].Identifier.Text, 
                context: type.ToString());
            injectMetadataList.Add(injectMetadata);
        }

        if (injectMetadataList.Count == 0)
        {
            return null;
        }
        
        string[] injectMethodLines = new string[injectIsOverride ? injectMetadataList.Count + 1 : injectMetadataList.Count];
        injectMethodLines[injectMethodLines.Length - 1] = "base.Inject();";

        int index = 0;
        foreach (var injectMetadata in injectMetadataList)
        {
            injectMethodLines[index++] =
                $"{injectMetadata.FieldName} = ContextProvider.GetContext<{injectMetadata.Context}>().Get<{injectMetadata.FieldType}>();";
        }

        var injectMethod = new MethodModel(
            keywords: injectIsOverride ? ["public", "override"] : ["public", "virtual"],
            returnType: "void",
            name: "Inject",
            parameters: [],
            lambda: false,
            lines: injectMethodLines
        );
        methodModelList.Add(injectMethod);

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