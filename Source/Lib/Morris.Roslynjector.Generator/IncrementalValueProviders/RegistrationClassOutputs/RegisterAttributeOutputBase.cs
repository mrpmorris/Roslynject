namespace Morris.Roslynjector.Generator.IncrementalValueProviders.RegistrationClassOutputs;

internal abstract class RegisterAttributeOutputBase
{
    public readonly ServiceLifetime ServiceLifetime;

    public abstract void GenerateCode(Action<string> writeLine);

    protected RegisterAttributeOutputBase(ServiceLifetime serviceLifetime)
    {
        ServiceLifetime = serviceLifetime;
    }
}
