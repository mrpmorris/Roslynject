using Microsoft.CodeAnalysis;
using Morris.Roslynjector.Generator.IncrementalValueProviders.RegistrationClassMetas;

namespace Morris.Roslynjector.Generator.IncrementalValueProviders.RegistrationClassOutputs;

internal static class RegistrationClassOutputsFactory
{
    public static IncrementalValuesProvider<RegistrationClassMeta> CreateValuesProvider(
        IncrementalValuesProvider<RegistrationClassMeta> registrationClasses,
        IncrementalValuesProvider<INamedTypeSymbol> candidateClasses)
    =>
        registrationClasses
        .Combine(candidateClasses.Collect())
        .Select((pair, cancellationToken) =>
            pair.Left.CloneWithCandidateClasses(pair.Right)
        );
}
