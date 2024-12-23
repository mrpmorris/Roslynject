using Microsoft.CodeAnalysis;
using Morris.Roslynject.Extensions;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Morris.Roslynject;

internal class RoslynjectAttributeData : IEqualityComparer<RoslynjectAttributeData>
{
	public readonly Find Find;
	public readonly INamedTypeSymbol Type;
	public readonly Register Register;
	public readonly WithLifetime WithLifetime;
	public readonly string? ServiceImplementationRegex;
	public readonly string? ServiceKeyRegex;

	public RoslynjectAttributeData(ImmutableDictionary<string, object?> dictionary)
	{
		Find = dictionary.GetValue<Find>(nameof(RoslynjectAttribute.Find));
		Type = dictionary.GetValue<INamedTypeSymbol>(nameof(RoslynjectAttribute.Type));
		Register = dictionary.GetValue<Register>(nameof(RoslynjectAttribute.Register));
		WithLifetime = dictionary.GetValue<WithLifetime>(nameof(RoslynjectAttribute.WithLifetime));
		ServiceImplementationRegex = dictionary.GetValueOrDefault<string?>(nameof(RoslynjectAttribute.ServiceImplementationRegex));
		ServiceKeyRegex = dictionary.GetValueOrDefault<string?>(nameof(RoslynjectAttribute.ServiceKeyRegex));
	}

	public bool Equals(RoslynjectAttributeData x, RoslynjectAttributeData y) =>
		x.Find == y.Find
		&& x.Register == y.Register
		&& x.WithLifetime == y.WithLifetime
		&& x.ServiceImplementationRegex == y.ServiceImplementationRegex
		&& x.ServiceKeyRegex == y.ServiceKeyRegex
		&& SymbolEqualityComparer.Default.Equals(x.Type, y.Type);

	public int GetHashCode(RoslynjectAttributeData other) =>
		HashCode.Combine(Find, Type, Register, WithLifetime, ServiceImplementationRegex, ServiceKeyRegex);
}
