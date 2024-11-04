namespace Morris.Roslynjector.Generator.IncrementalValueProviders.RegistrationClassOutputs;

internal class RegisterInterfaceAttributeOutput : RegisterAttributeOutputBase
{
    public RegisterInterfaceAttributeOutput(ServiceLifetime serviceLifetime)
        : base(serviceLifetime)
    {
    }

    public override void GenerateCode(Action<string> writeLine)
    {
        throw new NotImplementedException();
    }

}
