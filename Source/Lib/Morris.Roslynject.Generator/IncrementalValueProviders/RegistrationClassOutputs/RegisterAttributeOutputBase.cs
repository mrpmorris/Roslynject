namespace Morris.Roslynject.Generator.IncrementalValueProviders.RegistrationClassOutputs;

internal abstract class RegisterAttributeOutputBase
{
    public readonly string AttributeSourceCode;
    public readonly ServiceLifetime ServiceLifetime;
    private readonly Lazy<int> CachedHashCode;

    public abstract void GenerateCode(Action<string> writeLine);

    protected RegisterAttributeOutputBase(
        string attributeSourceCode,
        ServiceLifetime serviceLifetime)
    {
        AttributeSourceCode = attributeSourceCode;
        ServiceLifetime = serviceLifetime;
        CachedHashCode = new Lazy<int>(() =>
            HashCode
            .Combine(
                AttributeSourceCode,
                ServiceLifetime
            )
        );
    }

    public override bool Equals(object obj) =>
        obj is RegisterAttributeOutputBase other
        && ServiceLifetime == other.ServiceLifetime
        && AttributeSourceCode.Equals(
            other.AttributeSourceCode,
            StringComparison.Ordinal
        );

    public override int GetHashCode() => CachedHashCode.Value;

}
