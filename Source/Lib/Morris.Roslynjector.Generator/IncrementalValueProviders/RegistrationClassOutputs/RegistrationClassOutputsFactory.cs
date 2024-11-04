using Microsoft.CodeAnalysis;
using Morris.Roslynjector.Generator.IncrementalValueProviders.DeclaredRegistrationClasses;

namespace Morris.Roslynjector.Generator.IncrementalValueProviders.RegistrationClassOutputs;

internal static class RegistrationClassOutputsFactory
{
    public static IncrementalValuesProvider<RegistrationClassOutput> CreateValuesProvider(
        IncrementalValuesProvider<DeclaredRegistrationClass> registrationClasses,
        IncrementalValuesProvider<INamedTypeSymbol> candidateClasses)
    =>
        registrationClasses
        .Combine(candidateClasses.Collect())
        .Select((pair, cancellationToken) =>
            pair.Left.CreateOutput(pair.Right)
        )
        .Where(x => x is not null);
}
