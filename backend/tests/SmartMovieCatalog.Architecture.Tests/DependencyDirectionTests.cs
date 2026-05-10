using System.Reflection;
using Xunit;

namespace SmartMovieCatalog.Architecture.Tests;

public sealed class DependencyDirectionTests
{
    private static readonly string[] InternalAssemblyNames =
    [
        "SmartMovieCatalog.Api",
        "SmartMovieCatalog.Application",
        "SmartMovieCatalog.Contracts",
        "SmartMovieCatalog.Domain",
        "SmartMovieCatalog.Infrastructure"
    ];

    [Fact]
    public void Domain_Should_Not_Depend_On_Any_Internal_Project()
    {
        IReadOnlyCollection<string> dependencies = GetInternalProjectDependencies("SmartMovieCatalog.Domain");

        Assert.Empty(dependencies);
    }

    [Fact]
    public void Contracts_Should_Not_Depend_On_Any_Internal_Project()
    {
        IReadOnlyCollection<string> dependencies = GetInternalProjectDependencies("SmartMovieCatalog.Contracts");

        Assert.Empty(dependencies);
    }

    [Fact]
    public void Application_Should_Depend_On_Domain_Only()
    {
        IReadOnlyCollection<string> dependencies = GetInternalProjectDependencies("SmartMovieCatalog.Application");

        Assert.Equal(["SmartMovieCatalog.Domain"], dependencies.OrderBy(name => name, StringComparer.Ordinal));
    }

    [Fact]
    public void Infrastructure_Should_Depend_On_Application_And_Domain_Only()
    {
        IReadOnlyCollection<string> dependencies = GetInternalProjectDependencies("SmartMovieCatalog.Infrastructure");

        Assert.Equal(
            ["SmartMovieCatalog.Application", "SmartMovieCatalog.Domain"],
            dependencies.OrderBy(name => name, StringComparer.Ordinal));
    }

    [Fact]
    public void Api_Should_Depend_On_Application_Infrastructure_And_Contracts_Only()
    {
        IReadOnlyCollection<string> dependencies = GetInternalProjectDependencies("SmartMovieCatalog.Api");

        Assert.Equal(
            ["SmartMovieCatalog.Application", "SmartMovieCatalog.Contracts", "SmartMovieCatalog.Infrastructure"],
            dependencies.OrderBy(name => name, StringComparer.Ordinal));
    }

    private static IReadOnlyCollection<string> GetInternalProjectDependencies(string assemblyName)
    {
        HashSet<string> allowed = InternalAssemblyNames.ToHashSet(StringComparer.Ordinal);
        allowed.Remove(assemblyName);

        Assembly assembly = Assembly.Load(new AssemblyName(assemblyName));
        return assembly
            .GetReferencedAssemblies()
            .Select(reference => reference.Name)
            .Where(referenceName => referenceName is not null && allowed.Contains(referenceName))
            .Select(referenceName => referenceName!)
            .ToArray();
    }
}
