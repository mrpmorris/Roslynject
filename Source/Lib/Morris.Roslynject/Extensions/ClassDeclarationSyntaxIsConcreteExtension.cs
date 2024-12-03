using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Runtime.CompilerServices;

namespace Morris.Roslynject.Extensions;

internal static class ClassDeclarationSyntaxIsConcreteExtension
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsConcrete(
		this ClassDeclarationSyntax source)
	=>
		!source.Modifiers.Any(SyntaxKind.AbstractKeyword)
		&& !source.Modifiers.Any(SyntaxKind.StaticKeyword);
}

