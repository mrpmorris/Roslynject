using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Runtime.CompilerServices;

namespace Morris.Roslynject.Extensions;

internal static class AttributeSyntaxIsRegistrationAttributeExtension
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsRegistrationAttribute(this AttributeSyntax attribute)
	{
		string attributeFullName = attribute.Name.ToFullString();
		if (attributeFullName.EndsWith("Attribute"))
			attributeFullName = attributeFullName.Substring(0, attributeFullName.Length - 9);
		return AttributeNames
			.ShortNames
			.Any(x =>
				x == attributeFullName
				|| attributeFullName.EndsWith($".{x}")
			);
	}
}
