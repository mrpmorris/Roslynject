namespace Morris.Roslynjector.Generator.IncrementalValueProviders.RegistrationClassOutputs;

internal abstract class RegisterAttributeOutputBase
{
    public readonly ServiceLifetime ServiceLifetime;

    protected RegisterAttributeOutputBase(ServiceLifetime serviceLifetime)
    {
        ServiceLifetime = serviceLifetime;
    }
}
