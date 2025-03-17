using System;
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
    struct Metadata
    {
        public bool isValid;
        public bool isOverride;
        public string @namespace;
        public ClassDeclarationSyntax @class;
        public UsingDirectiveSyntax[] usingDirectives;
        public InjectMetadata[] fields;
    }

    struct InjectMetadata
    {
        public FieldDeclarationSyntax field;
        public TypeSyntax context;
    }
    
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var provider = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                "FloodInject.Runtime.ContextListenerAttribute",
                predicate: static (s, _) => s is ClassDeclarationSyntax,
                transform: static (ctx, _) => Transform(ctx))
            .Where(m => m.isValid);
        
        context.RegisterSourceOutput(provider, Generate);
    }

    private static Metadata Transform(GeneratorAttributeSyntaxContext context)
    {
        Metadata metadata = default;
        
        var syntax = (ClassDeclarationSyntax)context.TargetNode;
        var modifiersValid = syntax.HasModifiers(["public", "partial"]);
        var allFields = syntax.GetChildrenOfType<FieldDeclarationSyntax>().ToArray();
        List<InjectMetadata> injectFields = new List<InjectMetadata>();

        var attribute = syntax.GetAttribute("FloodInject.Runtime.ContextListenerAttribute", context);
        var attributeArgumentList = attribute.GetFirstChildOfType<AttributeArgumentListSyntax>();
        if (attributeArgumentList != null)
        {
            bool.TryParse(attributeArgumentList.Arguments[0].Expression.GetFirstToken().Text, out metadata.isOverride);
        }
        else
        {
            metadata.isOverride = false;
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
            InjectMetadata injectMetadata = default;
            injectMetadata.field = field;
            injectMetadata.context = type;
            injectFields.Add(injectMetadata);
        }
        
        metadata.@class = syntax;
        metadata.@namespace = syntax.GetNamespaceName();
        metadata.usingDirectives = syntax.GetUsingDirectives().ToArray();
        metadata.fields = injectFields.ToArray();
        metadata.isValid = modifiersValid && metadata.fields.Length > 0;
        return metadata;
    }

    private static void Generate(SourceProductionContext context, Metadata metadata)
    {
        using MemoryStream sourceStream = new();
        using StreamWriter sourceStreamWriter = new StreamWriter(sourceStream);
        using CodeWriter codeWriter = new CodeWriter(sourceStreamWriter);

        codeWriter.WriteLine("// <auto-generated />");
        foreach (var usingDirective in metadata.usingDirectives)
        {
            codeWriter.WriteLine(usingDirective.ToFullString());
        }

        codeWriter.StartNamespace(metadata.@namespace);

        foreach (var modifier in metadata.@class.Modifiers)
        {
            codeWriter.Write(modifier.Text + " ");
        }
        codeWriter.Write(metadata.@class.Keyword.Text + " ");
        
        using (codeWriter.CreateScope(prefix: metadata.@class.Identifier.Text))
        {
            var injectMethod = metadata.isOverride ? "public override void Inject()" : "public virtual void Inject()";
            using (codeWriter.CreateScope(prefix: injectMethod))
            {
                if (metadata.isOverride)
                {
                    codeWriter.WriteLine("base.Inject();");
                }
                foreach (var fieldMetadata in metadata.fields)
                {
                    var variable = fieldMetadata.field.Declaration.Variables[0].Identifier.Text;
                    var type = fieldMetadata.field.Declaration.Type.ToString();
                    codeWriter.WriteLine(variable + " = ContextProvider.GetContext(typeof(" + fieldMetadata.context + ")).Get<" + type + ">();");
                }
            }
        }
        
        codeWriter.EndNamespace(metadata.@namespace);
        codeWriter.Flush();
        context.AddSource($"{metadata.@class.Identifier.Text}.g.cs", SourceText.From(sourceStream, Encoding.UTF8, canBeEmbedded: true));
    }
}