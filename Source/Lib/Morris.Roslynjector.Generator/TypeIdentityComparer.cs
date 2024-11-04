using Microsoft.CodeAnalysis;

namespace Morris.Roslynjector.Generator;

internal class TypeIdentityComparer : IEqualityComparer<INamedTypeSymbol>
{
    public static readonly TypeIdentifyWithInheritanceComparer Default = new();

    public bool Equals(INamedTypeSymbol? x, INamedTypeSymbol? y) =>
        (x, y) switch
        {
            (INamedTypeSymbol left, INamedTypeSymbol right) =>
                left.Name == right.Name
                && left.ContainingNamespace.ToDisplayString() == right.ContainingNamespace.ToDisplayString(),
            (null, null) => true,
            (INamedTypeSymbol left, null) => false,
            (null, INamedTypeSymbol right) => false
        };

    public int GetHashCode(INamedTypeSymbol obj) =>
        obj is null
        ? 0
        : HashCode.Combine(
            obj.Name,
            obj.ContainingNamespace.ToDisplayString()
          );
}
