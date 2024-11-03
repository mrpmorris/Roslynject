using Microsoft.CodeAnalysis;
using Morris.Roslynjector.Generator.IncrementalValueProviders.DiscoveredRegistrationClasses;

namespace Morris.Roslynjector.Generator.IncrementalValueProviders.RegistrationClassOutputs;

internal static class RegistrationClassOutputsFactory
{
    public static IncrementalValuesProvider<DiscoveredRegistrationClass> CreateValuesProvider(
        IncrementalValuesProvider<DiscoveredRegistrationClass> registrationClasses,
        IncrementalValuesProvider<INamedTypeSymbol> candidateClasses)
    =>
        registrationClasses
        .Combine(candidateClasses.Collect())
        .Select((pair, cancellationToken) =>
            pair.Left.CloneWithCandidateClasses(pair.Right)
        );
}
