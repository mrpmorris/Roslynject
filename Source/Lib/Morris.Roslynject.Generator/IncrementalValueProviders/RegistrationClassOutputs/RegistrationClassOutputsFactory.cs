﻿using Microsoft.CodeAnalysis;
using Morris.Roslynject.Generator.IncrementalValueProviders.DeclaredRegistrationClasses;

namespace Morris.Roslynject.Generator.IncrementalValueProviders.RegistrationClassOutputs;

internal static class RegistrationClassOutputsFactory
{
    public static IncrementalValuesProvider<RegistrationClassOutput> CreateValuesProvider(
        IncrementalValuesProvider<DeclaredRegistrationClass> declaredRegistrationClasses,
        IncrementalValuesProvider<INamedTypeSymbol> injectionCandidates)
    =>
        declaredRegistrationClasses
        .Combine(injectionCandidates.Collect())
        .Select((pair, cancellationToken) =>
            pair.Left.CreateOutput(pair.Right)!
        )
        .Where(x => x is not null);
}