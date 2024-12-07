using Microsoft.CodeAnalysis;
using Morris.Roslynject.Extensions;
using Morris.Roslynject.IncrementalValueProviders.DeclaredRoslynjectModuleAttributes;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Morris.Roslynject.IncrementalValueProviders.RoslynjectModules;

internal sealed class RoslynjectModule
{
	public readonly DeclaredRoslynjectModule DeclaredModule;
	public readonly ImmutableArray<RoslynjectAttributeRegistrations> AttributeRegistrations;

	private readonly Lazy<int> CachedHashCode;

	public RoslynjectModule(
		DeclaredRoslynjectModule declaredModule,
		IEnumerable<INamedTypeSymbol> injectionCandidates)
	{
		DeclaredModule = declaredModule;
		if (declaredModule.ClassRegex is not null)
			injectionCandidates = injectionCandidates
				.Where(x => Regex.IsMatch(declaredModule.ClassRegex, x.ToDisplayString()));

		AttributeRegistrations = CreateAttributeRegistrations(declaredModule, injectionCandidates);

		CachedHashCode = new Lazy<int>(() =>
			HashCode.Combine(
				DeclaredModule,
				AttributeRegistrations.GetContentsHashCode()
			)
		);
	}

	private ImmutableArray<RoslynjectAttributeRegistrations> CreateAttributeRegistrations(
		DeclaredRoslynjectModule declaredModule,
		IEnumerable<INamedTypeSymbol> injectionCandidates)
	{
		throw new NotImplementedException();
	}
}
