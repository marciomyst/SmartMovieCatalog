using System.Reflection;

namespace SmartMovieCatalog.Application;

public sealed class ApplicationReference
{
    public static readonly Assembly Assembly = typeof(ApplicationReference).Assembly;

    private ApplicationReference()
    {
    }
}
