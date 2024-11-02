using Microsoft.Extensions.DependencyInjection;

namespace Morris.RoslynjectorTests;

internal static class IServiceCollectionExtensions
{
    public static void AssertIsRegistered(
        this IServiceCollection services,
        ServiceLifetime lifetime,
        Type serviceType)
    {
        int count = services.Count(x => x.Lifetime == lifetime && x.ServiceType == serviceType);
        Assert.AreEqual(
            1,
            count,
            message:
                $"Expected 1 registration for \"{serviceType.Name}\""
                + $" but found {count}.");
    }

    public static void AssertIsRegistered(
        this IServiceCollection services,
        ServiceLifetime lifetime,
        Type serviceType,
        Type implementationType)
    {
        int count = services.Count(x => 
            x.Lifetime == lifetime
            && x.ServiceType == serviceType
            && x.ImplementationType == implementationType);
        Assert.AreEqual(
            1,
            count,
            message:
                $"Expected 1 registration for \"{serviceType.Name}\""
                + $" with implementation \"{implementationType.Name}\""
                + $" but found {count}."
        );
    }
}
