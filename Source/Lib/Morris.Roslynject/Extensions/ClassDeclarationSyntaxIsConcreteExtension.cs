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

internal static class ClassDeclarationSyntaxHasAttributeExtension
{
	public static bool HasAttribute(
		this ClassDeclarationSyntax source,
		string attributeName)
	{
		if (source?.AttributeLists == null)
			return false;
		if (attributeName.EndsWith("Attribute"))
			attributeName = attributeName.Substring(0, attributeName.Length - 9);

		return source.AttributeLists
			.SelectMany(list => list.Attributes)
			.Select(attribute => attribute.Name.ToString())
			.Any(name =>
				name == attributeName
				|| name == $"{attributeName}Attribute"
				|| name.EndsWith($".{attributeName}")
				|| name.EndsWith($".{attributeName}Attribute")
			);
	}

}
