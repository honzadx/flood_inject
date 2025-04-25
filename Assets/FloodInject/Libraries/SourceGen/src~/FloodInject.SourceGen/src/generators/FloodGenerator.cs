using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

[Generator]
public class FloodGenerator : IIncrementalGenerator
{
    private record ResolveMetadata(string fieldType, string fieldName, string context)
    {
        internal string fieldType { get; } = fieldType;
        internal string fieldName { get; } = fieldName;
        internal string context { get; } = context;
    }
    
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var provider = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                "FloodInject.Runtime.FloodAttribute",
                predicate: static (s, _) => s is ClassDeclarationSyntax,
                transform: static (ctx, _) => Transform(ctx))
            .Where(t => t != null);
        
        context.RegisterSourceOutput(provider, SourceGeneratorsUtils.Generate);
    }

    private static TypeModel Transform(GeneratorAttributeSyntaxContext context)
    {
        var syntax = (ClassDeclarationSyntax)context.TargetNode;
        var modifiersValid = syntax.HasModifiers(["public", "partial"]);

        if (!modifiersValid)
        {
            return null;
        }

        var allFields = syntax.GetChildrenOfType<FieldDeclarationSyntax>().ToArray();
        List<MethodModel> methodModelList = new();
        List<ResolveMetadata> resolveMetadataList = new();
        var isOverride = IsOverride(context.SemanticModel, syntax);
        
        foreach (var field in allFields)
        {
            if (!field.HasAttribute("FloodInject.Runtime.FloodResolveAttribute", context))
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
                    fieldType: field.Declaration.Type.ToString(),
                    fieldName: field.Declaration.Variables[0].Identifier.Text,
                    context: "SharedStreamSO"
                ));
            }
            else
            {
                var type = argumentListSyntax.Arguments[0].Expression.GetFirstChildOfType<TypeSyntax>();
                ResolveMetadata resolveMetadata = new ResolveMetadata(
                    fieldType: field.Declaration.Type.ToString(),
                    fieldName: field.Declaration.Variables[0].Identifier.Text, 
                    context: type.ToString()
                );
                resolveMetadataList.Add(resolveMetadata);
            }
        }

        if (resolveMetadataList.Count == 0)
        {
            return null;
        }
        
        string[] methodEntries = new string[isOverride ? resolveMetadataList.Count + 4 : resolveMetadataList.Count + 3];
        methodEntries[0] = "PreConstruct();";
        methodEntries[1] = "var instance = StreamManager.instance;";
        methodEntries[methodEntries.Length - 2] = "base.Construct();";
        methodEntries[methodEntries.Length - 1] = "PostConstruct();";

        int index = 2;
        foreach (var injectMetadata in resolveMetadataList)
        {
            methodEntries[index++] =
                $"{injectMetadata.fieldName} = instance.GetStream<{injectMetadata.context}>().Resolve<{injectMetadata.fieldType}>();";
        }

        var constructMethod = new MethodModel(
            keywords: isOverride 
                ? ImmutableArray.Create(["public", "override"]) 
                : ImmutableArray.Create(["public", "virtual"]),
            type: "void",
            name: "Construct",
            parameters: ImmutableArray<VariableModel>.Empty, 
            lambda: false,
            lines: ImmutableArray.Create(methodEntries)
        );
        methodModelList.Add(constructMethod);
        methodModelList.Add(new MethodModel(
            keywords: ImmutableArray.Create(["partial"]),
            type: "void",
            name: "PreConstruct",
            parameters: ImmutableArray<VariableModel>.Empty, 
            lambda: false,
            lines: ImmutableArray<string>.Empty
        ));
        methodModelList.Add(new MethodModel(
            keywords: ImmutableArray.Create(["partial"]),
            type: "void",
            name: "PostConstruct",
            parameters: ImmutableArray<VariableModel>.Empty, 
            lambda: false,
            lines: ImmutableArray<string>.Empty
        ));

        var usings = syntax.GetUsingDirectives().Select(s => s.Name.ToString()).ToArray();
        var elements = methodModelList.Select(m => m as AElementModel).ToArray();
        
        TypeModel typeModel = new TypeModel(
            usings: ImmutableArray.Create(usings),
            @namespace: syntax.GetNamespaceName(),
            keywords: ImmutableArray.Create(["partial"]),
            kind: "class",
            name: syntax.Identifier.ValueText,
            elements: ImmutableArray.Create(elements));
        
        return typeModel;
    }

    private static bool IsOverride(SemanticModel semanticModel, ClassDeclarationSyntax classSyntax)
    {
        var attributeName = "FloodAttribute";

        var type = (INamedTypeSymbol)semanticModel.GetDeclaredSymbol(classSyntax);
        var baseType = type!.BaseType;
        while (baseType != null)
        {
            foreach (var attribute in baseType.GetAttributes())
            {
                var attributeClass = attribute.AttributeClass;
                if (attributeClass != null && attributeClass.Name == attributeName)
                {
                    return true;
                }
            }
            baseType = baseType.BaseType;
        }
    
        return false;
    }
}