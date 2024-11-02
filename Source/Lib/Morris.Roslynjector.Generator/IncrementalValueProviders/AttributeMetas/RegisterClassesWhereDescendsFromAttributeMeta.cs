using Microsoft.CodeAnalysis;
using Morris.Roslynjector.Generator.Extensions;
using System.Collections.Immutable;

namespace Morris.Roslynjector.Generator.IncrementalValueProviders.AttributeMetas;

internal class RegisterClassesWhereDescendsFromAttributeMeta : RegisterAttributeMetaBase, IEquatable<RegisterClassesWhereDescendsFromAttributeMeta>
{
    public readonly INamedTypeSymbol BaseClass;
    private readonly Lazy<int> CachedHashCode;

    public RegisterClassesWhereDescendsFromAttributeMeta(
        ServiceLifetime serviceLifetime,
        INamedTypeSymbol baseClass,
        ImmutableArray<INamedTypeSymbol> classesToRegister)
        : base(serviceLifetime, classesToRegister)
    {
        BaseClass = baseClass;
        CachedHashCode = new Lazy<int>(() =>
            HashCode
            .Combine(
                ServiceLifetime,
                BaseClass.ToDisplayString(),
                ClassesToRegister.GetContentsHashCode(ClassSignatureComparer.Instance.GetHashCode)
             )
        );
    }

    public static bool operator ==(RegisterClassesWhereDescendsFromAttributeMeta left, RegisterClassesWhereDescendsFromAttributeMeta right) => left.Equals(right);
    public static bool operator !=(RegisterClassesWhereDescendsFromAttributeMeta left, RegisterClassesWhereDescendsFromAttributeMeta right) => !(left == right);
    public override bool Equals(object obj) => obj is RegisterClassesWhereDescendsFromAttributeMeta other && Equals(other);

    public override RegisterAttributeMetaBase CloneWithClassesToRegister(
        ImmutableArray<INamedTypeSymbol> classes)
    =>
        new RegisterClassesWhereDescendsFromAttributeMeta(
            serviceLifetime: ServiceLifetime,
            baseClass: BaseClass,
            classesToRegister: classes);

    public bool Equals(RegisterClassesWhereDescendsFromAttributeMeta other) =>
        ServiceLifetime == other.ServiceLifetime
        && SymbolEqualityComparer.Default.Equals(BaseClass, other.BaseClass)
        && Enumerable.SequenceEqual(
            ClassesToRegister,
            other.ClassesToRegister,
            ClassSignatureComparer.Instance);

    public override void GenerateCode(Action<string> writeLine)
    {
        string baseClassName = BaseClass.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        writeLine($"// RegisterClassesWhereDescendsFrom(ServiceLifetime.{ServiceLifetime}, typeof({baseClassName}))");
        foreach (INamedTypeSymbol type in ClassesToRegister)
        {
            string className = type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            writeLine($"services.Add{ServiceLifetime}(typeof({className}));");
        }
    }

    public override int GetHashCode() => CachedHashCode.Value;

    public override bool Matches(INamedTypeSymbol typeSymbol) =>
        typeSymbol.DescendsFrom(BaseClass);
}
