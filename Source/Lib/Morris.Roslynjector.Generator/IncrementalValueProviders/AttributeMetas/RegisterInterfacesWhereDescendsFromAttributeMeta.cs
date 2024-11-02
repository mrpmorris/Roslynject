using Microsoft.CodeAnalysis;
using Morris.Roslynjector.Generator.Extensions;
using System.Collections.Immutable;

namespace Morris.Roslynjector.Generator.IncrementalValueProviders.AttributeMetas;

internal class RegisterInterfacesWhereDescendsFromAttributeMeta : RegisterAttributeMetaBase, IEquatable<RegisterInterfacesWhereDescendsFromAttributeMeta>
{
    public readonly INamedTypeSymbol BaseInterface;
    private readonly Lazy<int> CachedHashCode;

    public RegisterInterfacesWhereDescendsFromAttributeMeta(
        ServiceLifetime serviceLifetime,
        INamedTypeSymbol baseInterface,
        ImmutableArray<INamedTypeSymbol> classesToRegister)
        : base(serviceLifetime, classesToRegister)
    {
        BaseInterface = baseInterface;
        CachedHashCode = new Lazy<int>(() => 
            HashCode
            .Combine(
                ServiceLifetime,
                BaseInterface.ToDisplayString(),
                ClassesToRegister.GetContentsHashCode(ClassSignatureComparer.Instance.GetHashCode)
             )
        );
    }

    public static bool operator ==(RegisterInterfacesWhereDescendsFromAttributeMeta left, RegisterInterfacesWhereDescendsFromAttributeMeta right) => left.Equals(right);
    public static bool operator !=(RegisterInterfacesWhereDescendsFromAttributeMeta left, RegisterInterfacesWhereDescendsFromAttributeMeta right) => !(left == right);
    public override bool Equals(object obj) => obj is RegisterInterfacesWhereDescendsFromAttributeMeta other && Equals(other);

    public override RegisterAttributeMetaBase CloneWithClassesToRegister(
        ImmutableArray<INamedTypeSymbol> classes)
    =>
        new RegisterInterfacesWhereDescendsFromAttributeMeta(
            serviceLifetime: ServiceLifetime,
            baseInterface: BaseInterface,
            classesToRegister: classes);

    public bool Equals(RegisterInterfacesWhereDescendsFromAttributeMeta other) =>
        ServiceLifetime == other.ServiceLifetime
        && ClassSignatureComparer.Instance.Equals(BaseInterface, other.BaseInterface)
        && Enumerable.SequenceEqual(
            ClassesToRegister,
            other.ClassesToRegister,
            ClassSignatureComparer.Instance);

    public override void GenerateCode(Action<string> writeLine)
    {
    }

    public override int GetHashCode() => CachedHashCode.Value;

    public override bool Matches(INamedTypeSymbol typeSymbol) => false;

}
