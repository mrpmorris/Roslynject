using Microsoft.Extensions.DependencyInjection;

namespace Morris.RoslynjectorTests;

internal static class IServiceCollectionExtensions
{
    public static void AssertIsRegistered(
        this IServiceCollection services,
        ServiceLifetime lifetime,
        Type type)
    {
        int count = services.Count(x => x.Lifetime == lifetime && x.ServiceType == type);
        Assert.AreEqual(1, count, message: $"Expected 1 registration for \"{type.Name}\" but found {count}.");
    }
}
