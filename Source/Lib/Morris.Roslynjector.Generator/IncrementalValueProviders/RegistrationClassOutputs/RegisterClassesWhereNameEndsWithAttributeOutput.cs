using Microsoft.CodeAnalysis;
using Morris.Roslynjector.Generator.Helpers;
using System.Collections.Immutable;

namespace Morris.Roslynjector.Generator.IncrementalValueProviders.RegistrationClassOutputs;

internal class RegisterClassesWhereNameEndsWithAttributeOutput : RegisterAttributeOutputBase
{
    public readonly ImmutableArray<string> ClassesToRegister;

    public RegisterClassesWhereNameEndsWithAttributeOutput? Create(
        ServiceLifetime serviceLifetime,
        string suffix,
        ImmutableArray<INamedTypeSymbol> injectionCandidates)
    {
        var classesToRegister = injectionCandidates
            .Where(x => x.Name.EndsWith(suffix, StringComparison.Ordinal))
            .Select(x =>
                NamespaceHelper.Combine(
                    namespaceSymbol: x.ContainingNamespace,
                    className: x.Name
                )
            )
            .ToImmutableArray();

        return
            classesToRegister.Length == 0
            ? null
            : new RegisterClassesWhereNameEndsWithAttributeOutput(
                serviceLifetime: serviceLifetime,
                classesToRegister: classesToRegister);

    }

    private RegisterClassesWhereNameEndsWithAttributeOutput(
        ServiceLifetime serviceLifetime,
        ImmutableArray<string> classesToRegister)
        : base(serviceLifetime)
    {
        ClassesToRegister = classesToRegister;
    }
}
