using Microsoft.Extensions.DependencyInjection;
using SmartMovieCatalog.Application.Features.Auth;

namespace SmartMovieCatalog.Application.Tests;

public sealed class DependencyInjectionTests
{
    [Fact]
    public void AddApplication_RegistersExpectedServices()
    {
        ServiceCollection services = new();

        IServiceCollection returnedServices = SmartMovieCatalog.Application.DependencyInjection.AddApplication(services);

        Assert.Same(services, returnedServices);
        Assert.Contains(services, service => service.ServiceType == typeof(AuthenticateUser));
        Assert.Contains(services, service => service.ServiceType == typeof(GetCurrentUser));
    }
}
