using System;
using System.IO;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

[Generator]
public class ContextSourceGenerator : IIncrementalGenerator
{
    private static ClassDeclarationSyntax[] _contextClasses;
    
    struct Metadata
    {
        public bool isValid;
        public string @namespace;
        public ClassDeclarationSyntax @class;
    }
    
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        _contextClasses = [];
        var provider = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                "FloodInject.Runtime.GenerateContextAttribute",
                predicate: static (s, _) => s is ClassDeclarationSyntax,
                transform: static (ctx, _) => Transform(ctx))
            .Where(m => m.isValid);
        
        context.RegisterSourceOutput(provider, Generate);
    }

    private static Metadata Transform(GeneratorAttributeSyntaxContext context)
    {
        Metadata metadata = default;

        var syntax = (ClassDeclarationSyntax)context.TargetNode;
        var isContext = syntax.HasBaseType("FloodInject.Runtime.BaseContext") || syntax.HasBaseType("BaseContext");
        var validModifiers = syntax.HasModifiers(["public", "partial"]) && !syntax.HasModifier("abstract");
        
        metadata.@class = syntax;
        metadata.@namespace = syntax.GetNamespaceName();
        metadata.isValid = isContext && validModifiers;
        
        if(metadata.isValid)
        {
            Array.Resize(ref _contextClasses, _contextClasses.Length + 1);
            _contextClasses[_contextClasses.Length - 1] = syntax;
        }
        return metadata;
    }

    private static void Generate(SourceProductionContext context, Metadata metadata)
    {
        using MemoryStream sourceStream = new();
        using StreamWriter sourceStreamWriter = new StreamWriter(sourceStream);
        using CodeWriter codeWriter = new CodeWriter(sourceStreamWriter);

        codeWriter.WriteLine("// <auto-generated />");
        codeWriter.WriteLine("using FloodInject.Runtime;\n");
        codeWriter.StartNamespace(metadata.@namespace);

        foreach (var modifier in metadata.@class.Modifiers)
        {
            codeWriter.Write(modifier.Text + " ");
        }
        codeWriter.Write(metadata.@class.Keyword.Text + " ");
        
        using (codeWriter.CreateScope(prefix: metadata.@class.Identifier.Text))
        {
            codeWriter.WriteLine("public static System.Type Type => typeof(" + metadata.@class.Identifier.Text + ");");
            codeWriter.WriteLine("public override System.Type ContextType => " + metadata.@class.Identifier.Text + ".Type;\n");
            
            using (codeWriter.CreateScope(prefix: "protected void OnEnable()"))
            {
                codeWriter.WriteLine("ContextProvider.Register(this);");
            }

            codeWriter.WriteLine();
            
            using (codeWriter.CreateScope(prefix: "protected void OnDestroy()"))
            {
                codeWriter.WriteLine("ContextProvider.Unregister(this);");
            }
        }
        
        codeWriter.EndNamespace(metadata.@namespace);
        codeWriter.Flush();
        context.AddSource($"{metadata.@class.Identifier.Text}.g.cs", SourceText.From(sourceStream, Encoding.UTF8, canBeEmbedded: true));
    }
}