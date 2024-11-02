using Microsoft.CodeAnalysis;
using Morris.Roslynjector.Generator.Extensions;
using System.Collections.Immutable;

namespace Morris.Roslynjector.Generator.IncrementalValueProviders.AttributeMetas;

internal class RegisterClassesWhereNameEndsWithAttributeMeta : RegisterAttributeMetaBase, IEquatable<RegisterClassesWhereNameEndsWithAttributeMeta>
{
    public readonly string Suffix;
    private readonly Lazy<int> CachedHashCode;

    public RegisterClassesWhereNameEndsWithAttributeMeta(
        ServiceLifetime serviceLifetime,
        string suffix,
        ImmutableArray<INamedTypeSymbol> classesToRegister)
        : base(serviceLifetime, classesToRegister)
    {
        Suffix = suffix;
        CachedHashCode = new Lazy<int>(() => 
            HashCode
            .Combine(
                ServiceLifetime,
                Suffix,
                ClassesToRegister.GetContentsHashCode(ClassSignatureComparer.Instance.GetHashCode)
             )
        );
    }

    public static bool operator ==(RegisterClassesWhereNameEndsWithAttributeMeta left, RegisterClassesWhereNameEndsWithAttributeMeta right) => left.Equals(right);
    public static bool operator !=(RegisterClassesWhereNameEndsWithAttributeMeta left, RegisterClassesWhereNameEndsWithAttributeMeta right) => !(left == right);
    public override bool Equals(object obj) => obj is RegisterClassesWhereNameEndsWithAttributeMeta other && Equals(other);

    public override RegisterAttributeMetaBase CloneWithClassesToRegister(
        ImmutableArray<INamedTypeSymbol> classes)
    =>
        new RegisterClassesWhereNameEndsWithAttributeMeta(
            serviceLifetime: ServiceLifetime,
            suffix: Suffix,
            classesToRegister: classes);

    public bool Equals(RegisterClassesWhereNameEndsWithAttributeMeta other) =>
        ServiceLifetime == other.ServiceLifetime
        && Suffix == other.Suffix
        && Enumerable.SequenceEqual(
            ClassesToRegister,
            other.ClassesToRegister,
            ClassSignatureComparer.Instance);

    public override void GenerateCode(Action<string> writeLine)
    {
        writeLine($"// RegisterClassesWhereNameEndsWith(ServiceLifetime.{ServiceLifetime}, \"{Suffix}\")");
        foreach(INamedTypeSymbol type in ClassesToRegister)
        {
            string className = type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            writeLine($"services.Add{ServiceLifetime}(typeof({className}));");
        }
    }

    public override int GetHashCode() => CachedHashCode.Value;

    public override bool Matches(INamedTypeSymbol typeSymbol) =>
        typeSymbol.Name.EndsWith(Suffix);
}
