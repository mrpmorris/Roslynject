using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Morris.Roslynject.Extensions;
using Morris.Roslynject.IncrementalValueProviders.RegistrationClassOutputs;
using Morris.Roslynject.StaticResources;
using System.Collections.Immutable;

namespace Morris.Roslynject.IncrementalValueProviders.DeclaredRegistrationClasses;

internal class DeclaredRegisterClassesDescendedFromAttribute : DeclaredRegisterAttributeBase, IEquatable<DeclaredRegisterClassesDescendedFromAttribute>
{
	public readonly INamedTypeSymbol BaseClassType;
	public readonly RegisterClassAs RegisterAs;
	public readonly string? ClassRegex;
	private readonly Lazy<int> CachedHashCode;

	public DeclaredRegisterClassesDescendedFromAttribute(
		AttributeSyntax attributeSyntax,
		INamedTypeSymbol baseClassType,
		ServiceLifetime serviceLifetime,
		RegisterClassAs registerAs,
		string? classRegex)
		: base(
			attributeSyntax: attributeSyntax,
			serviceLifetime: serviceLifetime)
	{
		BaseClassType = baseClassType;
		RegisterAs = registerAs;
		ClassRegex = classRegex;
		CachedHashCode = new Lazy<int>(() =>
			HashCode
			.Combine(
				base.GetHashCode(),
				SymbolEqualityComparer.Default.GetHashCode(baseClassType),
				RegisterAs,
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
			registerAs: RegisterAs,
			classRegex: ClassRegex,
			injectionCandidates: injectionCandidates);

	public override bool Equals(object? obj) =>
		obj is DeclaredRegisterClassesDescendedFromAttribute other
		&& Equals(other);

	public bool Equals(DeclaredRegisterClassesDescendedFromAttribute? other) =>
		base.Equals(other)
		&& RegisterAs == other.RegisterAs
		&& ClassRegex == other.ClassRegex
		&& SymbolEqualityComparer.Default.Equals(BaseClassType, other.BaseClassType);

	public override int GetHashCode() => CachedHashCode.Value;

	public override bool Matches(INamedTypeSymbol typeSymbol) =>
		typeSymbol.DescendsFrom(BaseClassType);
}
