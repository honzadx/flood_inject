using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

internal static class SyntaxNodeExtensions
{
    public static IEnumerable<UsingDirectiveSyntax> GetUsingDirectives(this SyntaxNode self)
    {
        var parent = self.Parent;
        while (parent != null)
        {
            foreach (var node in parent.ChildNodes())
            {
                if (node is not UsingDirectiveSyntax usingDirective)
                {
                   continue;
                }
                yield return usingDirective;
            }
            parent = parent.Parent;
        }
    }

    public static T GetParentOfType<T>(this SyntaxNode self) where T : SyntaxNode
    {
        T target = null;
        var parent = self.Parent;
        while (parent != null)
        {
            if (parent is T)
            {
                target = (T)parent;
                break;
            }
            parent = parent.Parent;
        }
        return target;
    }
    
    public static IEnumerable<T> GetChildrenOfType<T>(this SyntaxNode self) where T : SyntaxNode
    {
        foreach (var node in self.ChildNodes())
        {
            if (node is T target)
            {
                yield return target;
            }
        }
    }
    
    public static T GetFirstChildOfType<T>(this SyntaxNode self) where T : SyntaxNode
    {
        foreach (var node in self.ChildNodes())
        {
            if (node is T target)
            {
                return target;
            }
        }

        return default;
    }
    
    public static string GetNamespaceName(this SyntaxNode self)
    {
        var parent = self.Parent;
        StringBuilder sb = new();
        while (parent is BaseNamespaceDeclarationSyntax or BaseTypeDeclarationSyntax)
        {
            switch (parent)
            {
                case BaseTypeDeclarationSyntax parentClass:
                    sb.Append(parentClass.Identifier.Text);
                    break;
                case BaseNamespaceDeclarationSyntax parentNamespace:
                    sb.Append(parentNamespace.Name );
                    break;
            }
            sb.Append('.');
            parent = parent.Parent;
        }
        if (sb.Length > 0) 
        {
            sb.Remove(sb.Length - 1, 1);
        }
        return sb.ToString();
    }
    
    public static bool HasAttribute<T>(this T self, string attributeName, GeneratorSyntaxContext context) where T : BaseTypeDeclarationSyntax
    {
        return HasAttribute(self.AttributeLists, attributeName, context.SemanticModel);
    }
    
    public static bool HasAttribute(this FieldDeclarationSyntax self, string attributeName, GeneratorSyntaxContext context)
    {
        return HasAttribute(self.AttributeLists, attributeName, context.SemanticModel);
    }
    
    public static bool HasAttribute(this MethodDeclarationSyntax self, string attributeName, GeneratorSyntaxContext context)
    {
        return HasAttribute(self.AttributeLists, attributeName, context.SemanticModel);
    }
    
    public static bool HasAttribute<T>(this T self, string attributeName, GeneratorAttributeSyntaxContext context) where T : BaseTypeDeclarationSyntax
    {
        return HasAttribute(self.AttributeLists, attributeName, context.SemanticModel);
    }
    
    public static bool HasAttribute(this FieldDeclarationSyntax self, string attributeName, GeneratorAttributeSyntaxContext context)
    {
        return HasAttribute(self.AttributeLists, attributeName, context.SemanticModel);
    }
    
    public static bool HasAttribute(this MethodDeclarationSyntax self, string attributeName, GeneratorAttributeSyntaxContext context)
    {
        return HasAttribute(self.AttributeLists, attributeName, context.SemanticModel);
    }
    
    public static bool HasAttribute(SyntaxList<AttributeListSyntax> attributeLists, string attributeName, SemanticModel semanticModel)
    {
        foreach (var attributeList in attributeLists)
        foreach (var attribute in attributeList.Attributes)
        {
            if (semanticModel.GetSymbolInfo(attribute).Symbol is not IMethodSymbol
                attributeSymbol)
            {
                continue;
            }
            if (attributeSymbol.ContainingType.ToDisplayString() == attributeName)
            {
                return true;
            }
        }
        return false;
    }
    
    public static AttributeSyntax GetAttribute<T>(this T self, string attributeName, GeneratorSyntaxContext context) where T : BaseTypeDeclarationSyntax
    {
        return GetAttribute(self.AttributeLists, attributeName, context.SemanticModel);
    }
    
    public static AttributeSyntax GetAttribute(this FieldDeclarationSyntax self, string attributeName, GeneratorSyntaxContext context)
    {
        return GetAttribute(self.AttributeLists, attributeName, context.SemanticModel);
    }
    
    public static AttributeSyntax GetAttribute(this MethodDeclarationSyntax self, string attributeName, GeneratorSyntaxContext context)
    {
        return GetAttribute(self.AttributeLists, attributeName, context.SemanticModel);
    }
    
    public static AttributeSyntax GetAttribute<T>(this T self, string attributeName, GeneratorAttributeSyntaxContext context) where T : BaseTypeDeclarationSyntax
    {
        return GetAttribute(self.AttributeLists, attributeName, context.SemanticModel);
    }
    
    public static AttributeSyntax GetAttribute(this FieldDeclarationSyntax self, string attributeName, GeneratorAttributeSyntaxContext context)
    {
        return GetAttribute(self.AttributeLists, attributeName, context.SemanticModel);
    }
    
    public static AttributeSyntax GetAttribute(this MethodDeclarationSyntax self, string attributeName, GeneratorAttributeSyntaxContext context)
    {
        return GetAttribute(self.AttributeLists, attributeName, context.SemanticModel);
    }
    
    public static AttributeSyntax GetAttribute(SyntaxList<AttributeListSyntax> attributeLists, string attributeName, SemanticModel semanticModel)
    {
        foreach (var attributeList in attributeLists)
        foreach (var attribute in attributeList.Attributes)
        {
            if (semanticModel.GetSymbolInfo(attribute).Symbol is not IMethodSymbol
                attributeSymbol)
            {
                continue;
            }
            if (attributeSymbol.ContainingType.ToDisplayString() == attributeName)
            {
                return attribute;
            }
        }
        return default;
    }
    
    public static bool HasModifiers<T>(this T self, string[] modifierNames) where T : BaseTypeDeclarationSyntax
    {
        return HasModifiers(self.Modifiers, modifierNames);
    }

    public static bool HasModifier<T>(this T self, string modifierName) where T : BaseTypeDeclarationSyntax
    {
        return HasModifier(self.Modifiers, modifierName);
    }

    public static bool HasModifier(SyntaxTokenList modifiers, string modifierName)
    {
        foreach (var modifier in modifiers)
        {
            if (modifier.ToString() == modifierName)
            {
                return true;
            }
        }
        return false;
    }
    
    public static bool HasModifiers(SyntaxTokenList modifiers, string[] modifierNames)
    {
        int matchingModifierCount = 0;
        foreach (var modifierName in modifierNames)
        foreach (var modifier in modifiers)
        {
            if (modifier.ToString() == modifierName)
            {
                matchingModifierCount++;
                if (matchingModifierCount == modifierNames.Length)
                {
                    return true;
                }
                break;
            }
        }
        return false;
    }

    // TODO: (hdx) Find out how to check properly for inheritance
    
    /// <summary>
    /// Tries to check if owner inherits from baseTypeName. Doesn't handle namespaces correctly, so baseTypeName could be either "System.Attribute" or "Attribute" depending on the SyntaxTree specifics. Can also check only depth 1 of inheritance.
    /// </summary>
    /// <param name="self">Owner.</param>
    /// <param name="baseTypeName">Type name to check for a match.</param>
    public static bool HasBaseType(this TypeDeclarationSyntax self, string baseTypeName)
    {
        return HasBaseType(self.BaseList, baseTypeName);
    }
    
    /// <summary>
    /// Tries to check if owner inherits from baseTypeName. Doesn't handle namespaces correctly, so baseTypeName could be either "System.Attribute" or "Attribute" depending on the SyntaxTree specifics. Can also check only depth 1 of inheritance.
    /// </summary>
    /// <param name="baseList">Container.</param>
    /// <param name="baseTypeName">Type name to check for a match.</param>
    public static bool HasBaseType(BaseListSyntax baseList, string baseTypeName)
    {
        if (baseList == null)
        {
            return false;
        }
        foreach (var node in baseList.ChildNodes())
        {
            if (node is not SimpleBaseTypeSyntax baseType)
            {
                continue;
            }
            if (baseType.Type.ToString() == baseTypeName)
            {
                return true;
            }
        }
        return false;
    }
}