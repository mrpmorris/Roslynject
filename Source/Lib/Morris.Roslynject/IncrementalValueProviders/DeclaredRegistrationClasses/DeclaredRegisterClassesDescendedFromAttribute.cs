using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.DependencyInjection;
using Morris.Roslynject.Extensions;
using Morris.Roslynject.IncrementalValueProviders.RegistrationClassOutputs;
using System.Collections.Immutable;

namespace Morris.Roslynject.IncrementalValueProviders.DeclaredRegistrationClasses;

internal class DeclaredRegisterClassesDescendedFromAttribute : DeclaredRegisterAttributeBase, IEquatable<DeclaredRegisterClassesDescendedFromAttribute>
{
	public readonly INamedTypeSymbol BaseClassType;
	public readonly RegisterClassAs RegisterClassAs;
	public readonly string? ClassRegex;
	private readonly Lazy<int> CachedHashCode;

	public DeclaredRegisterClassesDescendedFromAttribute(
		AttributeSyntax attributeSyntax,
		INamedTypeSymbol baseClassType,
		ServiceLifetime serviceLifetime,
		RegisterClassAs registerClassAs,
		string? classRegex)
		: base(
			attributeSyntax: attributeSyntax,
			serviceLifetime: serviceLifetime)
	{
		BaseClassType = baseClassType;
		RegisterClassAs = registerClassAs;
		ClassRegex = classRegex;
		CachedHashCode = new Lazy<int>(() =>
			HashCode
			.Combine(
				base.GetHashCode(),
				SymbolEqualityComparer.Default.GetHashCode(baseClassType),
				RegisterClassAs,
				ClassRegex
			 )
		);
	}

	public override RegisterAttributeOutputBase? CreateOutput(
		ImmutableArray<INamedTypeSymbol> injectionCandidates)
	=>
		RegisterClassesDescendedFromOutput.Create(
			attributeSourceCode: AttributeSourceCode,
			baseClassType: BaseClassType,
			serviceLifetime: ServiceLifetime,
			registerClassAs: RegisterClassAs,
			classRegex: ClassRegex,
			injectionCandidates: injectionCandidates);

	public override bool Equals(object? obj) =>
		obj is DeclaredRegisterClassesDescendedFromAttribute other
		&& Equals(other);

	public bool Equals(DeclaredRegisterClassesDescendedFromAttribute? other) =>
		base.Equals(other)
		&& RegisterClassAs == other.RegisterClassAs
		&& ClassRegex == other.ClassRegex
		&& SymbolEqualityComparer.Default.Equals(BaseClassType, other.BaseClassType);

	public override int GetHashCode() => CachedHashCode.Value;

	public override bool Matches(INamedTypeSymbol typeSymbol) =>
		typeSymbol.DescendsFrom(BaseClassType);
}
