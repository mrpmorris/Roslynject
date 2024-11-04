using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Morris.Roslynject.Generator.Extensions;

internal static class ClassDeclarationSyntaxExtensions
{
    public static bool IsConcrete(
        this ClassDeclarationSyntax source)
    =>
        !source.Modifiers.Any(SyntaxKind.AbstractKeyword)
        && !source.Modifiers.Any(SyntaxKind.StaticKeyword);
}
