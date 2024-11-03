using Microsoft.CodeAnalysis;
using Morris.Roslynjector.Generator.IncrementalValueProviders.DeclaredRegistrationClasses;

namespace Morris.Roslynjector.Generator.IncrementalValueProviders.RegistrationClassOutputs;

internal static class RegistrationClassOutputsFactory
{
    public static IncrementalValuesProvider<DeclaredRegistrationClass> CreateValuesProvider(
        IncrementalValuesProvider<DeclaredRegistrationClass> registrationClasses,
        IncrementalValuesProvider<INamedTypeSymbol> candidateClasses)
    =>
        registrationClasses
        .Combine(candidateClasses.Collect())
        .Select((pair, cancellationToken) =>
            pair.Left.CloneWithCandidateClasses(pair.Right)
        );
}
