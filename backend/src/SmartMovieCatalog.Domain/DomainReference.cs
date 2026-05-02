using System.Reflection;

namespace SmartMovieCatalog.Domain;

public sealed class DomainReference
{
    public static readonly Assembly Assembly = typeof(DomainReference).Assembly;

    private DomainReference()
    {
    }
}
