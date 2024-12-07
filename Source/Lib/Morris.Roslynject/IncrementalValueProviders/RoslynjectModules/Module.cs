using Microsoft.CodeAnalysis;
using Morris.Roslynject.Extensions;
using Morris.Roslynject.IncrementalValueProviders.DeclaredRoslynjectModuleAttributes;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace Morris.Roslynject.IncrementalValueProviders.RoslynjectModules;

internal sealed class Module
{
	public readonly DeclaredRoslynjectModule DeclaredModule;
	public readonly ImmutableArray<AttributeAndRegistrations> AttributeAndRegistrations;

	private readonly Lazy<int> CachedHashCode;

	public Module(
		DeclaredRoslynjectModule declaredModule,
		IEnumerable<INamedTypeSymbol> injectionCandidates)
	{
		DeclaredModule = declaredModule;
		if (declaredModule.ClassRegex is not null)
			injectionCandidates = injectionCandidates
				.Where(x => Regex.IsMatch(declaredModule.ClassRegex, x.ToDisplayString()));

		AttributeAndRegistrations = CreateAttributeRegistrations(declaredModule, injectionCandidates);

		CachedHashCode = new Lazy<int>(() =>
			HashCode.Combine(
				DeclaredModule,
				AttributeAndRegistrations.GetContentsHashCode()
			)
		);
	}

	private ImmutableArray<AttributeAndRegistrations> CreateAttributeRegistrations(
		DeclaredRoslynjectModule declaredModule,
		IEnumerable<INamedTypeSymbol> injectionCandidates)
	{
		throw new NotImplementedException();
	}
}
