using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

[Generator]
public class FloodStreamRequirementGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var provider = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                "FloodInject.Runtime.FloodStreamRequirementAttribute",
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

        if (ModelExtensions.GetDeclaredSymbol(context.SemanticModel, syntax) is not INamedTypeSymbol classSymbol)
        {
            return null;
        }

        if (classSymbol.BaseType == null)
        {
            return null;
        }

        if (classSymbol.BaseType.Name != "AManagedStreamSO")
        {
            return null;
        }

        List<string> requiredContracts = new();
        foreach (var attributeList in syntax.AttributeLists)
        foreach (var attribute in attributeList.Attributes)
        {
            if (ModelExtensions.GetSymbolInfo(context.SemanticModel, attribute).Symbol is not IMethodSymbol
                attributeSymbol)
            {
                continue;
            }
            if (attributeSymbol.ContainingType.ToDisplayString() == "FloodInject.Runtime.FloodStreamRequirementAttribute")
            {
                if (attribute.ArgumentList == null)
                {
                    continue;
                }

                var typeSyntax = attribute.ArgumentList.Arguments[0].Expression.GetFirstChildOfType<TypeSyntax>();
                requiredContracts.Add(typeSyntax.ToString());
            }
        }
        if (requiredContracts.Count == 0)
        {
            return null;
        }
        
        List<string> methodLines = new();
        methodLines.Add("return new [] {");
        for(int i = 0; i < requiredContracts.Count; i++)
        {
            methodLines.Add($"\ttypeof({requiredContracts[i]}),");
        }
        methodLines[methodLines.Count - 1] += " };";

        MethodModel methodModel = new MethodModel(
            keywords: ImmutableArray.Create(["public", "override"]),
            type: "System.Type[]",
            name: "RequiredContracts",
            parameters: ImmutableArray<VariableModel>.Empty, 
            lambda: false,
            lines: ImmutableArray.Create(methodLines.ToArray()));

        var usings = syntax.GetUsingDirectives().Select(s => s.Name.ToString()).ToArray();

        AElementModel[] elements = { methodModel };
        return new TypeModel(
            usings: ImmutableArray.Create(usings),
            @namespace: syntax.GetNamespaceName(),
            keywords: ImmutableArray.Create(["partial"]),
            kind: "class",
            name: syntax.Identifier.ValueText,
            elements: ImmutableArray.Create(elements));
    }
}