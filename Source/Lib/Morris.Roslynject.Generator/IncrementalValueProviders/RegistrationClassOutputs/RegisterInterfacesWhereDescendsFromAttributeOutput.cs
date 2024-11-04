using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Morris.Roslynject.Generator.IncrementalValueProviders.RegistrationClassOutputs;

internal class RegisterInterfacesWhereDescendsFromAttributeOutput : RegisterAttributeOutputBase
{
    public RegisterInterfacesWhereDescendsFromAttributeOutput(
        string attributeSourceCode,
        ServiceLifetime serviceLifetime)
        : base(
            attributeSourceCode: attributeSourceCode,
            serviceLifetime: serviceLifetime)
    {
    }

    public override void GenerateCode(Action<string> writeLine)
    {
        throw new NotImplementedException();
    }

}
