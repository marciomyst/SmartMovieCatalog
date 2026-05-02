using System.Reflection;

namespace SmartMovieCatalog.Infrastructure;

public sealed class InfrastructureReference
{
    public static readonly Assembly Assembly = typeof(InfrastructureReference).Assembly;

    private InfrastructureReference()
    {
    }
}
