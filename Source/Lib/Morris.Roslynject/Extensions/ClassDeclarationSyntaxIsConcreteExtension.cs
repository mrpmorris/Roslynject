using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Xml.Schema;

namespace Morris.Roslynject.Extensions;

internal static class ClassDeclarationSyntaxIsConcreteExtension
{
	public static bool IsConcrete(
		this ClassDeclarationSyntax source)
	=>
		!source.Modifiers.Any(SyntaxKind.AbstractKeyword)
		&& !source.Modifiers.Any(SyntaxKind.StaticKeyword);
}

