using Microsoft.CodeAnalysis;
using Morris.Roslynject.Extensions;

namespace Morris.Roslynject;

internal class TypeSignatureComparer : IEqualityComparer<INamedTypeSymbol>
{
	public static readonly TypeSignatureComparer Default = new();

	public bool Equals(INamedTypeSymbol? x, INamedTypeSymbol? y) =>
		(x, y) switch {
			(INamedTypeSymbol left, INamedTypeSymbol right) =>
			TypeHierarchyComparer.Default.Equals(left, right)
			&& left.AllInterfaces.SequenceEqual(right.AllInterfaces, TypeIdentityComparer.Default),
			(null, null) => true,
			(INamedTypeSymbol left, null) => false,
			(null, INamedTypeSymbol right) => false
		};

	public int GetHashCode(INamedTypeSymbol? obj) =>
		obj is null
		? 0
		: HashCode.Combine(
			obj.Name,
			obj.ContainingNamespace.ToDisplayString(),
			obj.AllInterfaces.GetContentsHashCode(TypeIdentityComparer.Default)
		  );
}
