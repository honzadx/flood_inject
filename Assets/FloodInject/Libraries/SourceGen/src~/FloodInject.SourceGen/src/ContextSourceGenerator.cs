using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FloodInject.SourceGen.models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

[Generator]
public class ContextSourceGenerator : IIncrementalGenerator
{
    private enum ContextType
    {
        Volatile,
        Protected
    }
    
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
        var validModifiers = syntax.HasModifiers(["public", "partial"]) && !syntax.HasModifier("abstract");
        
        if(!validModifiers)
        {
            return null;
        }

        var attribute = syntax.GetAttribute("FloodInject.Runtime.GenerateContextAttribute", context);
        var attributeArgumentList = attribute.GetFirstChildOfType<AttributeArgumentListSyntax>();
        if (attributeArgumentList == null)
        {
            return null;
            
        }

        ContextType type = attributeArgumentList.Arguments[0].Expression.ToString() switch
        {
            "FloodInject.Runtime.ContextType.Protected" => ContextType.Protected,
            "ContextType.Protected" => ContextType.Protected,
                
            "FloodInject.Runtime.ContextType.Volatile" => ContextType.Volatile,
            "ContextType.Volatile" => ContextType.Volatile,
            _ => ContextType.Volatile
        };
        
        List<BaseTypeElementModel> elementModelList = new ();

        switch (type)
        {
            case ContextType.Volatile:
                elementModelList.AddRange(
        [
                    new MethodModel(
                            keywords: ["public", "override"],
                            returnType: "void",
                            name: "Bind<T>",
                            parameters: [new VariableModel(
                                type: "T",
                                name: "instance"
                                )],
                            lambda: false,
                            lines: ["BindInternal(instance);"]),
                    new NewLineModel(),
                    new MethodModel(
                        keywords: ["public", "override"],
                        returnType: "void",
                        name: "Bind<T>",
                        parameters: [new VariableModel(
                            type: "global::System.Func<T>",
                            name: "factoryMethod"
                        )],
                        lambda: false,
                        lines: ["BindInternal(factoryMethod);"]),
                    new NewLineModel(),
                    new MethodModel(
                        keywords: ["public", "override"],
                        returnType: "void",
                        name: "Rebind<T>",
                        parameters: [new VariableModel(
                            type: "T",
                            name: "instance"
                        )],
                        lambda: false,
                        lines: ["RebindInternal(instance);"]),
                    new NewLineModel(),
                    new MethodModel(
                        keywords: ["public", "override"],
                        returnType: "void",
                        name: "Rebind<T>",
                        parameters: [new VariableModel(
                            type: "global::System.Func<T>",
                            name: "factoryMethod"
                        )],
                        lambda: false,
                        lines: ["RebindInternal(factoryMethod);"]),
                    new NewLineModel(),
                    new MethodModel(
                        keywords: ["public", "override"],
                        returnType: "void",
                        name: "Unbind<T>",
                        parameters: [],
                        lambda: false,
                        lines: ["Unbind<T>();"]),
                    new NewLineModel(),
                    new MethodModel(
                        keywords: ["public", "override"],
                        returnType: "void",
                        name: "Reset",
                        parameters: [],
                        lambda: false,
                        lines: ["ResetInternal();"]),
                    new NewLineModel(),
                    new MethodModel(
                        keywords: ["public", "override"],
                        returnType: "T",
                        name: "Get<T>",
                        parameters: [],
                        lambda: false,
                        lines: ["return GetInternal<T>();"])
                ]);
                break;
            case ContextType.Protected:
                var unityAssertionsDefine = "#if UNITY_ASSERTIONS";
                var endDefine = "#endif";
                var elseDefine = "#else";
                
                var unityEnsureLocked = """global::UnityEngine.Assertions.Assert.IsTrue(_isLocked, $"Protected context has to be locked to retrieve contracts {this}");""";
                var unityEnsureUnlocked = """global::UnityEngine.Assertions.Assert.IsTrue(!_isLocked, $"Protected context has to be unlocked to modify contracts {this}");""";
                
                var ensureLocked = """global::System.Diagnostics.Debug.Assert(_isLocked, $"Protected context has to be locked to retrieve contracts {this}");""";
                var ensureUnlocked = """global::System.Diagnostics.Debug.Assert(!_isLocked, $"Protected context has to be unlocked to modify contracts {this}");""";
                elementModelList.AddRange(
        [
                    new FieldModel(
                        keywords: ["private"],
                        name: "_isLocked",
                        type: "bool",
                        value: "false"),
                    new NewLineModel(),
                    new PropertyModel(
                        keywords: ["public"],
                        type: "bool",
                        name: "IsLocked",
                        returnValue: "_isLocked"),
                    new NewLineModel(),
                    new MethodModel(
                            keywords: ["public", "override"],
                            returnType: "void",
                            name: "Bind<T>",
                            parameters: [new VariableModel(
                                type: "T",
                                name: "instance"
                                )],
                            lambda: false,
                            lines: 
                            [
                                unityAssertionsDefine,
                                unityEnsureUnlocked,
                                elseDefine,
                                ensureUnlocked,
                                endDefine,
                                "BindInternal(instance);"
                            ]),
                    new NewLineModel(),
                    new MethodModel(
                        keywords: ["public", "override"],
                        returnType: "void",
                        name: "Bind<T>",
                        parameters: [new VariableModel(
                            type: "global::System.Func<T>",
                            name: "factoryMethod"
                        )],
                        lambda: false,
                        lines: 
                        [
                            unityAssertionsDefine,
                            unityEnsureUnlocked,
                            elseDefine,
                            ensureUnlocked,
                            endDefine,
                            "BindInternal(factoryMethod);"
                        ]),
                    new NewLineModel(),
                    new MethodModel(
                        keywords: ["public", "override"],
                        returnType: "void",
                        name: "Rebind<T>",
                        parameters: [new VariableModel(
                            type: "T",
                            name: "instance"
                        )],
                        lambda: false,
                        lines: 
                        [
                            unityAssertionsDefine,
                            unityEnsureUnlocked,
                            elseDefine,
                            ensureUnlocked,
                            endDefine,
                            "RebindInternal(instance);"
                        ]),
                    new NewLineModel(),
                    new MethodModel(
                        keywords: ["public", "override"],
                        returnType: "void",
                        name: "Rebind<T>",
                        parameters: [new VariableModel(
                            type: "global::System.Func<T>",
                            name: "factoryMethod"
                        )],
                        lambda: false,
                        lines: 
                        [
                            unityAssertionsDefine,
                            unityEnsureUnlocked,
                            elseDefine,
                            ensureUnlocked,
                            endDefine,
                            "RebindInternal(factoryMethod);"
                        ]),
                    new NewLineModel(),
                    new MethodModel(
                        keywords: ["public", "override"],
                        returnType: "void",
                        name: "Unbind<T>",
                        parameters: [],
                        lambda: false,
                        lines: 
                        [
                            unityAssertionsDefine,
                            unityEnsureUnlocked,
                            elseDefine,
                            ensureUnlocked,
                            endDefine,
                            "Unbind<T>();"
                        ]),
                    new NewLineModel(),
                    new MethodModel(
                        keywords: ["public", "override"],
                        returnType: "T",
                        name: "Get<T>",
                        parameters: [],
                        lambda: false,
                        lines: 
                        [
                            unityAssertionsDefine,
                            unityEnsureLocked,
                            elseDefine,
                            ensureLocked,
                            endDefine,
                            "return GetInternal<T>();"
                        ]),
                    new NewLineModel(),
                    new MethodModel(
                        keywords: ["public", "override"],
                        returnType: "void",
                        name: "Reset",
                        parameters: [],
                        lambda: false,
                        lines: ["ResetInternal();", "_isLocked = false;"]),
                    new NewLineModel(),
                    new MethodModel(
                        keywords: ["public"],
                        returnType: "void",
                        name: "Lock",
                        parameters: [],
                        lambda: false,
                        lines: ["_isLocked = true;"]),
                ]);
                break;
        }

        TypeModel typeModel = new TypeModel(
            pragmaDisables: [],
            usings: ["global::FloodInject.Runtime"],
            @namespace: syntax.GetNamespaceName(),
            keywords: ["partial"],
            kind: "class",
            name: syntax.Identifier.Text,
            implements: "BaseContext",
            elements: elementModelList.ToArray());
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