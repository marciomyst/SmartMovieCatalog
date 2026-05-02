using System.Reflection;

namespace SmartMovieCatalog.Api;

public sealed class ApiReference
{
    public static readonly Assembly Assembly = typeof(ApiReference).Assembly;

    private ApiReference()
    {
    }
}
