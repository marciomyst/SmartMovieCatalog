using System.Reflection;

namespace SmartMovieCatalog.Contracts;

public sealed class ContractsReference
{
    public static readonly Assembly Assembly = typeof(ContractsReference).Assembly;

    private ContractsReference()
    {
    }
}
