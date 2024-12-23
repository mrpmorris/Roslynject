using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace Morris.Roslynject;

internal class TypeIdentityComparer : IEqualityComparer<INamedTypeSymbol>
{
	public static readonly TypeHierarchyComparer Default = new();

	public bool Equals(INamedTypeSymbol? x, INamedTypeSymbol? y) =>
		(x, y) switch {
			(INamedTypeSymbol left, INamedTypeSymbol right) =>
				left.Name == right.Name
				&& left.ContainingNamespace.ToDisplayString() == right.ContainingNamespace.ToDisplayString(),
			(null, null) => true,
			(INamedTypeSymbol left, null) => false,
			(null, INamedTypeSymbol right) => false
		};

	public int GetHashCode(INamedTypeSymbol? obj) =>
		obj is null
		? 0
		: HashCode.Combine(
			obj.Name,
			obj.ContainingNamespace.ToDisplayString()
		  );
}
