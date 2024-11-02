using Microsoft.CodeAnalysis;
using Morris.Roslynjector.Generator.Extensions;

namespace Morris.Roslynjector.Generator;

internal class ClassSignatureComparer : IEqualityComparer<INamedTypeSymbol>
{
    public static readonly ClassSignatureComparer Instance = new();

    public bool Equals(INamedTypeSymbol? x, INamedTypeSymbol? y) =>
        (x, y) switch
        {
            (INamedTypeSymbol left, INamedTypeSymbol right) =>
                left.Name == right.Name
                && left.ContainingNamespace.ToDisplayString() == right.ContainingNamespace.ToDisplayString()
                && this.Equals(left.BaseType?.ConstructedFrom!, right.BaseType?.ConstructedFrom!),
            (null, null) => true,
            (INamedTypeSymbol left, null) => false,
            (null, INamedTypeSymbol right) => false
        };

    public int GetHashCode(INamedTypeSymbol obj) =>
        obj is null
        ? 0
        : HashCode.Combine(
            obj.Name,
            obj.ContainingNamespace.ToDisplayString(),
            GetHashCode(obj.BaseType?.ConstructedFrom!),
            obj.Interfaces.GetContentsHashCode(this.GetHashCode)
          );
}
